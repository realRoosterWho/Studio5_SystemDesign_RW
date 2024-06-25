using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //循环遍历存在的所有屏幕
        for (int i = 0; i < Display.displays.Length; i++)
        {
            //开启存在的屏幕显示，激活显示器
            Display.displays[i].Activate();
            Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
