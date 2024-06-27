using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLogic : MonoBehaviour
{
    private Light myLight; // 你的Light组件
    public float barSpringValue; // 用于存储knob_bar_spring的值


    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>(); // 获取Light组件
    }

    // Update is called once per frame
    void Update()
    {
        var inputData = InputHandler.Instance.GetInputData(); // 获取InputData
        if (inputData != null && inputData.knob_bar_spring != null) // 如果knob_bar_spring不是null
        {
            barSpringValue = inputData.knob_bar_spring; // 更新barSpringValue的值
        }
        
        
        if (inputData != null) // 如果knob不是null
        {
            // 根据knob的值来调整Light组件的Range
            myLight.range = Mathf.Lerp(10, 2.8f, barSpringValue / 150f);
        }
        
        //如果在DialogueMode，关闭灯光，如果在FreeMode，打开灯光
        if (ControlModeManager.Instance.m_controlMode == ControlMode.Free) // 如果当前是Free模式
        {
            myLight.enabled = true; // 打开灯光
        }
        else // 如果不是Free模式
        {
            myLight.enabled = false; // 关闭灯光
        }
    }
}