using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJ_Logic : MonoBehaviour
{
    // 用于存储 CharacterController 组件
    private CharacterController controller;
    
    [SerializeField] private float m_speed = 2.0f; // 控制移动的速度
    
    //前进的范围
    [SerializeField] private float m_deadZone = 50f;
    
    private float m_ymovement = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //获取 CharacterController 组件
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 从 InputHandler 获取输入数据
        InputData data = InputHandler.Instance.GetInputData();
        
        //获取x轴输入
        float x = data.y;
        
        //映射x轴除以500
        x = x / 500;
        
        //用CharacterController横向
        controller.Move(new Vector3(x, 0, 0) * m_speed * Time.deltaTime);
        
        //获取y轴输入
        float y = data.x;
        
        //映射y轴除以500
        y = y / 500;
        
        //如果y轴移动大于死区，那么别动
        if (Mathf.Abs(m_ymovement + y) > m_deadZone)
        {
            return;
        }
        else
        {
            //用CharacterController纵向
            controller.Move(new Vector3(0, 0, y) * m_speed * Time.deltaTime);
            
            //更新y轴移动
            m_ymovement += y;
        }

    }
}
