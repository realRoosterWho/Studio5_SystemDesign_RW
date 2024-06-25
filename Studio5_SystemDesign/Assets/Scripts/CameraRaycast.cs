using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Cursor; // 新增：游戏对象 m_Cursor

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 创建一个射线，从摄像机的位置向前发射
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 如果射线碰到了物体
        if (Physics.Raycast(ray, out hit))
        {
            //如果碰撞到的物体带有 "Cursor" 标签，直接返回
            if (!hit.collider.gameObject.CompareTag("Cursor"))
            {
                m_Cursor.transform.position = hit.point;
            }
            // 将 m_Cursor 的位置设置为碰撞点的位置
        }
    }
}