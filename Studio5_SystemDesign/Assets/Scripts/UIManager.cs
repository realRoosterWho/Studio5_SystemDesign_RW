using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonosingletonTemp<UIManager>
{
    [SerializeField] private Image uiImage;  // 获取 Image 组件

    // Public 函数，接受一个 Sprite 变量
    public void UpdateImage(Sprite newSprite)
    {
        if (newSprite != null)
        {
            uiImage.sprite = newSprite;
            uiImage.gameObject.SetActive(true);  // 展示 Image 组件
        }
        else
        {
            uiImage.gameObject.SetActive(false);  // 关闭 Image 组件
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始化时可以选择关闭 Image 组件
        uiImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}