using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Logic : MonoBehaviour
{
    
    
    [SerializeField]public Light m_light;
    
    //定义Emission强度
    [SerializeField] public float emission = 30000;
    
    //定义颜色
    [SerializeField] public Color m_color = Color.white;

    
    // Start is called before the first frame update
    void Start()
    {
        //Get the light component
        m_light = GetComponent<Light>();
        
        //Check if the light is on
        if (m_light.enabled)
        {
            Debug.Log("The light is on");
        }
        else
        {
            Debug.Log("The light is off");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Change the intensity of the light
        m_light.intensity = emission;
        
        //Change the color of the light
        m_light.color = m_color;
        
    }
}
