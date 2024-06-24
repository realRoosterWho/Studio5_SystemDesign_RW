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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
