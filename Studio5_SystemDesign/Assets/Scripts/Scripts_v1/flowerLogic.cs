using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerLogic : MonoBehaviour
{
    [SerializeField]public GameObject flowerPrefab; // 预制体
    [SerializeField]public float heightAboveGround = 1.0f; // 预制体生成的高度

    // Update is called once per frame
    void Update()
    {
        // 从 InputHandler 获取输入数据
        InputData data = InputHandler.Instance.GetInputData();
        
        if (data != null && data.Up == 1.0) // 当鼠标左键点击时
        {
            Debug.Log("Up");
            //从屏幕中心发射一条射线
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // 如果射线碰到了物体
            {
                Vector3 spawnPosition = hit.point + new Vector3(0, heightAboveGround, 0); // 计算预制体的生成位置
                Instantiate(flowerPrefab, spawnPosition, Quaternion.identity); // 在计算出的位置生成预制体
            }
        }
    }
}