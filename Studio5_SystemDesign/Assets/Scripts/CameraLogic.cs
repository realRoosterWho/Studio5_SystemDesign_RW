using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public Vector3 currentRotation;
    public Vector3 initialRotation;
    public Vector3 targetRotation; // 新增：目标旋转
    
    public float timer = 0f; // 新增：计时器
    public float timeLimit = 2f; // 新增：时间限制


    [SerializeField]
    private Vector3 axisMapping = new Vector3(1, 2, 3); // 1 for x, 2 for y, 3 for z

    [SerializeField]
    private bool invertX = false; // 是否反转X轴的输入
    [SerializeField]
    private bool invertY = false; // 是否反转Y轴的输入
    [SerializeField]
    private bool invertZ = false; // 是否反转Z轴的输入

    [SerializeField]
    private float gyroThreshold = 0.1f; // 陀螺仪的输入小于这个值时，我们认为没有输入数据

    [SerializeField]
    private float recoverySpeed = 1f; // 控制摄像机回复到初始角度的速度

    [SerializeField]
    private bool enableMovement = false; // 是否通过遥感的x, y来完成前进后退和左右移动

    [SerializeField]
    private float movementSpeed = 0.01f; // 控制移动的速度

	//一个布尔，用于说明摄像头锁定
    [SerializeField]
	private bool isLocked = false;
    
    [SerializeField]
    private Transform main; // 父物体

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.rotation.eulerAngles;
        initialRotation = currentRotation;
        targetRotation = currentRotation; // 初始化目标旋转为当前旋转


		//锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        // 从 InputHandler 获取输入数据


        switch (ControlModeManager.Instance.m_controlMode)
        {
            case ControlMode.Free:
                FreeMode();
                break;
            case ControlMode.Dialogue:
                DialogueMode();
                break;
        }


    }

    private void FreeMode()
    {
        InputData data = InputHandler.Instance.GetInputData();
        // 如果按下扳机键，回到初始角度
        if (data != null && data.Trigger == 1.0)
        {
            targetRotation = initialRotation;
        }
        // 如果数据不为 null，调整摄像机的旋转
        else if (data != null && !isLocked && (Mathf.Abs(data.gyr_x) > gyroThreshold || Mathf.Abs(data.gyr_y) > gyroThreshold || Mathf.Abs(data.gyr_z) > gyroThreshold))
        {
            // 使用陀螺仪的数据来调整摄像机的旋转
            Vector3 gyroData = new Vector3(data.gyr_x, data.gyr_y, data.gyr_z);
            targetRotation.x += (invertX ? -1 : 1) * gyroData[(int)axisMapping.x - 1] * Time.deltaTime;
            targetRotation.y += (invertY ? -1 : 1) * gyroData[(int)axisMapping.y - 1] * Time.deltaTime;
            targetRotation.z += (invertZ ? -1 : 1) * gyroData[(int)axisMapping.z - 1] * Time.deltaTime;

            // 限制头部旋转在-10度到10度之间
            targetRotation.z = Mathf.Clamp(targetRotation.z, -10f, 10f);
            targetRotation.x = Mathf.Clamp(targetRotation.x, -20f, 20f);
            targetRotation.y = Mathf.Clamp(targetRotation.y, -50f, 50f);
            
            timer = 0f; // 重置计时器

        }
        else
        {
            // 如果没有输入数据，开始计时
            timer += Time.deltaTime;

            // 如果计时器超过两秒，慢慢地回复到初始角度
            if (timer >= timeLimit)
            {
                targetRotation = initialRotation;
            }
        }

        // 使用 Vector3.Lerp 平滑地过渡到目标旋转
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, recoverySpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentRotation);
    }

    private void DialogueMode()
    {
        targetRotation = initialRotation;
        // 使用 Vector3.Lerp 平滑地过渡到目标旋转
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, recoverySpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentRotation);
    }
}