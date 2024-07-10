using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerParticalLogic : MonoBehaviour
{
    private ParticleSystem particleSystem; // 粒子系统

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>(); // 获取粒子系统
        Invoke("StopParticleSystem", 5.0f); // 5秒后停止粒子系统
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 停止粒子系统
    void StopParticleSystem()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop(); // 停止粒子系统
        }
    }
}