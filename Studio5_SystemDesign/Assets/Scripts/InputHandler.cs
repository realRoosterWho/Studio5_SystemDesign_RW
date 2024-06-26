using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class InputData
{
    public float x;
    public float y;
    public float z;
    public float Trigger;
    public float Menu;
    public float Down;
    public float Up;
    public float Grip;
    public float acc_x;
    public float acc_y;
    public float acc_z;
    public float temp;
    public float gyr_x;
    public float gyr_y;
    public float gyr_z;
    public float TEMP;
    public int knob_mode;
    public int knob_bar_spring;
    public int knob_trigger_spring;
    public int knob_selection;
    public int knob_bar_smooth;
}

[System.Serializable]
public class OutputData
{
    public string mode;
    public float resistance;
    public float selectionNumber;
    public float breathingFreq;
    public float r;
    public float g;
    public float b;
}

public class InputHandler : MonosingletonTemp<InputHandler>
{
    [SerializeField] private InputData m_inputData;
	[SerializeField] public bool m_Trigger;
	[SerializeField] private bool canTrigger;
	public float m_timer;

    public bool DEBUGMODE;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //用GetInputData更新m_inputData
        m_inputData = GetInputData();
        //如果在DEBUG模式，m_inputData.Trigger = 按下空格键与否
        if (DEBUGMODE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_inputData.Trigger = 1.0f;
            }
            else
            {
                m_inputData.Trigger = 0.0f;
            }
        }
		if (m_inputData.Trigger == 1.0 && canTrigger == true)
		{
			m_Trigger = true;
			canTrigger = false;
            Debug.Log("Triggered");
		}

		if(canTrigger == false)
		{
			m_timer += Time.deltaTime;
			if (m_timer > 0.5)
            {
                canTrigger = true;
                m_timer = 0;
            }
		}
    }

    public InputData GetInputData()
    {
        //读取json
        string json = File.ReadAllText(Application.dataPath + "/Python/data.json");
        // Debug.Log(json);
        //解析json。其结构为：{"x": 5.0, "y": -13.0, "z": 0.0, "Trigger": 0.0, "Menu": 0.0, "Down": 0.0, "Up": 0.0, "Grip": 0.0, "acc_x": -4.03, "acc_y": -9.02, "acc_z": 8.19, "temp": 29.19, "gyr_x": 0.07, "gyr_y": -1.35, "gyr_z": 2.59, "TEMP": 31.35, "knob_mode": 3, "knob_bar_spring": 0, "knob_trigger_spring": null, "knob_selection": 0, "knob_bar_smooth": 0}
        //如果为空，不解析，否则解析
        if (json != "")
        {
            InputData data = JsonUtility.FromJson<InputData>(json);
            //Debug.Log(data);
            return data;
        }
        else
        {
            return null;
        }
    }
    
    //写一个函数，用于将收到的数据写入文件，写到Application.dataPath + "/Python/data_out.json"里面
    public void WriteData(OutputData data)
    {
        // Convert the OutputData object to a JSON string
        string json = JsonUtility.ToJson(data);
        //Debug
        // Debug.Log(json);
        // Call the WriteData method from the InputHandler class to write the JSON string to a file
        File.WriteAllText(Application.dataPath + "/Python/data_out.json", json);
    }
    
    //public OutputData
    
    
    
}