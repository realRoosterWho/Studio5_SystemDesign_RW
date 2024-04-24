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
}

public class InputHandler : MonosingletonTemp<InputHandler>
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public InputData GetInputData()
    {
        //读取json
        string json = File.ReadAllText(Application.dataPath + "/Python/data.json");
        Debug.Log(json);
        //解析json。其结构为：{"x": 5.0, "y": -13.0, "z": 0.0, "Trigger": 0.0, "Menu": 0.0, "Down": 0.0, "Up": 0.0, "Grip": 0.0, "acc_x": -4.03, "acc_y": -9.02, "acc_z": 8.19, "temp": 29.19, "gyr_x": 0.07, "gyr_y": -1.35, "gyr_z": 2.59}
        //如果为空，不解析，否则解析
        if (json != "")
        {
            InputData data = JsonUtility.FromJson<InputData>(json);
            Debug.Log(data);
            return data;
        }
        else
        {
            return null;
        }
    }
}