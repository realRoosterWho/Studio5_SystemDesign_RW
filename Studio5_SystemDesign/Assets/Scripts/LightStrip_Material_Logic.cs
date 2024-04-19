using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStrip_Material_Logic : MonoBehaviour
{
    public Material m_material;
    [SerializeField] public Color m_color = Color.white;
    public Color m_colortodeliver = Color.white;
    [SerializeField] public Gradient colorGradient;
    [SerializeField] public bool useGradient = false;
    [SerializeField] public float m_emissionintensity = 15f;  // 这里的单位是EV100

    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

void Update()
{
    if (useGradient)
    {
        m_colortodeliver = new Color(Mathf.Sin(Time.time), Mathf.Cos(Time.time), Mathf.Sin(Time.time));
    }
    else
    {
        m_colortodeliver = m_color;  // 这里可以改为你想要的常数颜色
    }

    m_material.SetColor("_EmissiveColor", m_colortodeliver * ConvertToLuminance(m_emissionintensity));  // 在HDRP中, 发光颜色通常需要乘以强度

    m_material.SetFloat("_EmissiveIntensity", ConvertToLuminance(m_emissionintensity));
}

    float ConvertToLuminance(float EV100)
    {
        // 将EV100转换为cd/m²
        return 0.0125f * Mathf.Pow(2, EV100) / 100;
    }
}