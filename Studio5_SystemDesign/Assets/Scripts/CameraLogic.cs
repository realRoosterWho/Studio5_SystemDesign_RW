using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 initialRotation;

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

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.rotation.eulerAngles;
        initialRotation = currentRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 从 InputHandler 获取输入数据
        InputData data = InputHandler.Instance.GetInputData();

        // 如果按下扳机键，回到初始角度
        if (data != null && data.Trigger == 1.0)
        {
            currentRotation = initialRotation;
        }
        // 如果数据不为 null，调整摄像机的旋转
        else if (data != null && (Mathf.Abs(data.gyr_x) > gyroThreshold || Mathf.Abs(data.gyr_y) > gyroThreshold || Mathf.Abs(data.gyr_z) > gyroThreshold))
        {
            // 使用陀螺仪的数据来调整摄像机的旋转
            Vector3 gyroData = new Vector3(data.gyr_x, data.gyr_y, data.gyr_z);
            currentRotation.x += (invertX ? -1 : 1) * gyroData[(int)axisMapping.x - 1] * Time.deltaTime;
            currentRotation.y += (invertY ? -1 : 1) * gyroData[(int)axisMapping.y - 1] * Time.deltaTime;
            currentRotation.z += (invertZ ? -1 : 1) * gyroData[(int)axisMapping.z - 1] * Time.deltaTime;

            // 限制头部旋转在-10度到10度之间
            currentRotation.z = Mathf.Clamp(currentRotation.z, -10f, 10f);
        }
        else
        {
            // 如果没有输入数据，慢慢地回复到初始角度
            currentRotation = Vector3.Lerp(currentRotation, initialRotation, recoverySpeed * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(currentRotation);
    }
}