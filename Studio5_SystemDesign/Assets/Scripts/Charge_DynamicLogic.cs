using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charge_DynamicLogic : MonoBehaviour
{
    public Image myImage; // 你的Image组件
    public float barSpringValue; // 用于存储knob_bar_spring的值

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>(); // 获取Image组件
    }

    // Update is called once per frame
    void Update()
    {
        var inputData = InputHandler.Instance.GetInputData(); // 获取InputData
        if (inputData != null && inputData.knob_bar_spring != null) // 如果knob_bar_spring不是null
        {
            barSpringValue = inputData.knob_bar_spring; // 更新barSpringValue的值
        }

        if (ControlModeManager.Instance.m_controlMode == ControlMode.Free) // 如果当前是Free模式
        {
            myImage.fillAmount = Mathf.Clamp(barSpringValue / 150f, 0, 1); // 更新fillAmount的值
        }
        else // 如果不是Free模式
        {
            myImage.fillAmount = 0; // 将fillAmount设置为0
        }
    }
}