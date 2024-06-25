using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetermineBoxLogic : MonoBehaviour
{
    public bool isCursorIn = false; // 添加一个布尔变量来跟踪光标是否在碰撞体内部

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        //如果isCursorIn为true，并且Trigger键被按下，进入DIalogueMode
        if (isCursorIn && InputHandler.Instance.GetInputData().Trigger == 1.0)
        {
            ControlModeManager.Instance.m_controlMode = ControlMode.Dialogue;
        }
    }

    private void DialogueMode()
    {
        
    }

    // 当其他碰撞体进入触发器时，该方法会被调用
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞体是否带有 "Cursor" 标签
        if (other.gameObject.CompareTag("Cursor"))
        {
            // Debug.Log("Cursor has entered the box");
            // 如果带有 "Cursor" 标签，将 isCursorIn 设置为 true
            isCursorIn = true;
        }
    }

    // 当其他碰撞体离开触发器时，该方法会被调用
    private void OnTriggerExit(Collider other)
    {
        // 检查碰撞体是否带有 "Cursor" 标签
        if (other.gameObject.CompareTag("Cursor"))
        {
            // 如果带有 "Cursor" 标签，将 isCursorIn 设置为 false
            isCursorIn = false;
        }
    }
}