using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Logic : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        //订阅事件
        EventManager.Instance.AddEvent("CoinRemake", OnCoinRemake);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
//如果碰撞
    private void OnTriggerEnter(Collider other)
    {
        //如果碰撞的是Player
        if (other.gameObject.CompareTag("AJ"))
        {
            //取消渲染
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
    //当金币重生
    private void OnCoinRemake(GameEventArgs args)
    {
        //重新渲染
        GetComponent<MeshRenderer>().enabled = true;
    }
}
