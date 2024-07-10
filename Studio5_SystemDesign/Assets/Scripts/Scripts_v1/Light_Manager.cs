using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//定义一个枚举，用于区分不同的灯的模式
public enum LightMode
{
    Manual,
    FocusMode,
    ExcitingMode,
    Flashing,
    Breathing
}

public class Light_Manager : MonosingletonTemp<Light_Manager>
{
    // 一个列表，用于存储所有的Light_Logic
    public List<Light_Logic> lightList = new List<Light_Logic>();
    // 一个列表，用于存储所有的LightStrip_Material_Logic
    public List<LightStrip_Material_Logic> lightStripList = new List<LightStrip_Material_Logic>();

    public OutputData m_outputdata;

    // 添加一些Serialize的变量，用于统一调整所有灯和灯带的属性
    [SerializeField] public float lightEmission = 30000;
    [SerializeField] public Color lightStripColor = Color.white;
    [SerializeField] public float lightStripEmissionIntensity = 15f;
    
    [SerializeField] public Color lightColor = Color.white;
    [SerializeField] public bool lightStripUseGradient = false;
    [SerializeField] public bool isTextOn = true;
    
    [SerializeField] private TextMeshPro text;
    [SerializeField] private LightMode m_currentMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_currentMode)
        {
            case LightMode.Manual:
                lightStripUseGradient = false;
                AdjustAllLights();
                AdjustAllLightStrips();
                break;
            case LightMode.FocusMode:
                lightStripUseGradient = false;
                SetAllLights(Color.blue, 50000);
                SetAllLightStrips(Color.blue, lightStripEmissionIntensity, false);
                break;
            case LightMode.ExcitingMode:
                lightStripUseGradient = false;
                SetAllLights(Color.red, 70000);
                SetAllLightStrips(Color.red, lightStripEmissionIntensity, true);
                break;
            case LightMode.Flashing:
                lightStripUseGradient = false;
                SetAllLights(Color.white, 30000);
                SetAllLightStrips(Color.white, lightStripEmissionIntensity, false);
                break;
            case LightMode.Breathing:
                lightStripUseGradient = true;
                SetAllLights(Color.blue, 40000);
                UpdateLightStrip();
                break;
        }


        SetLightStripUseGradient();
        UpdateText();
    }

    
    //Update地获取Lightstrip的状态，获取lightstrip.m_color并且write to json
    void UpdateLightStrip()
    {
        foreach (var lightStrip in lightStripList)
        {
            WriteLightColorToJson(lightStrip.m_colortodeliver);
        }
    }
    
    //如果lightStripUseGradient为true，那么将所有的灯带的useGradient设置为true
    void SetLightStripUseGradient()
    {
        foreach (var lightStrip in lightStripList)
        {
            lightStrip.useGradient = lightStripUseGradient;
        }
    }
    
    void AdjustAllLights()
    {
        foreach (var light in lightList)
        {
            // 使用lightEmission变量来统一调整所有灯的亮度
            light.emission = lightEmission;
            light.m_color = lightColor;
        }
    }

    void AdjustAllLightStrips()
    {
        foreach (var lightStrip in lightStripList)
        {
            // 使用lightStripColor和lightStripEmissionIntensity变量来统一调整所有灯带的颜色和强度
            lightStrip.m_color = lightStripColor;
            WriteLightColorToJson(lightStripColor);
            lightStrip.m_emissionintensity = lightStripEmissionIntensity;
            lightStrip.useGradient = lightStripUseGradient;
        }
    }
    
    void SetAllLights(Color color, float emission)
    {
        foreach (var light in lightList)
        {
            light.m_color = color;
            light.emission = emission;
        }
    }

    void SetAllLightStrips(Color color, float emissionIntensity, bool useGradient)
    {
        foreach (var lightStrip in lightStripList)
        {
            lightStrip.m_color = color;
            WriteLightColorToJson(color);
            lightStrip.m_emissionintensity = emissionIntensity;
            lightStrip.useGradient = useGradient;
        }
    }

    void UpdateText()
    {
        if (isTextOn)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
        
        text.text = m_currentMode.ToString();
    }
    
    //写一个函数，调用InputHandler里面的函数，用于将当前的灯的颜色数据写入json文件
    public void WriteLightColorToJson(Color datalightStripColor)
    {
        
        // 检查m_outputdata是否为null，如果为null，初始化它
        if (m_outputdata == null)
        {
            m_outputdata = new OutputData();
        }
        
        // Convert the color to a format that can be written to JSON
        //先映射成0,255，并且round一下，再赋值 
        m_outputdata.r = Mathf.Round(datalightStripColor.r * 255);
        m_outputdata.b = Mathf.Round(datalightStripColor.g * 255);
        m_outputdata.g = Mathf.Round(datalightStripColor.b * 255);

    

        // Call the WriteData method from the InputHandler class to write the JSON string to a file
        InputHandler.Instance.WriteData(m_outputdata);
    }
}