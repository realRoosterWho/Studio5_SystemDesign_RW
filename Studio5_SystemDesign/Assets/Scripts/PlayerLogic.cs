using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private Transform m_initialPoint;

    [SerializeField]
    private float speed = 30.0f; // 控制移动的速度

    [SerializeField]
    private float deadZone = 50f; // 设置摇杆的死区

    [SerializeField]
    private float rotationSpeed = 100.0f; // 控制旋转的速度

    // Start is called before the first frame update
    void Start()
    {
        // 获取 CharacterController 组件
        controller = GetComponent<CharacterController>();
        ResetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // 从 InputHandler 获取输入数据
        InputData data = InputHandler.Instance.GetInputData();
        
 
        //自动按照速度前进
        controller.Move(transform.forward * speed * Time.deltaTime);
        
        //如果按下G健，重新加载场景，用Application
        if (data != null && data.Grip == 1.0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        
    }
    
    // 重置玩家位置
    public void ResetPlayerPosition()
    {
        transform.position = m_initialPoint.position;
        transform.rotation = m_initialPoint.rotation;
    }
}