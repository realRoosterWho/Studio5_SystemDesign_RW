using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode
{
    Free,
    Dialogue,
}

public class ControlModeManager : MonosingletonTemp<ControlModeManager>
{
    public ControlMode m_controlMode = ControlMode.Free;
    private ControlMode m_lastControlMode;
	[field:SerializeField] public OutputData m_FreeData;
	[field:SerializeField] public OutputData m_DialogueData;

    // Start is called before the first frame update
    void Start()
    {
        m_lastControlMode = m_controlMode;
		InputHandler.Instance.WriteData(m_FreeData);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_lastControlMode != m_controlMode)
        {
            switch (m_controlMode)
            {
                case ControlMode.Free:
                    // 在这里添加进入Free模式的第一帧要执行的操作
					InputHandler.Instance.WriteData(m_FreeData);
                    break;
                case ControlMode.Dialogue:
                    // 在这里添加进入Dialogue模式的第一帧要执行的操作
					InputHandler.Instance.WriteData(m_DialogueData);
					Debug.Log("Dialogue Mode");
                    break;
            }

            m_lastControlMode = m_controlMode;
        }
    }
}