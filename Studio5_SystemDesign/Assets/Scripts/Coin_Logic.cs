using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
//如果碰撞
    private void OnCollisionEnter(Collision other)
    {
        //如果碰撞的是Player
        if (other.gameObject.CompareTag("Player"))
        {
            //销毁金币
            Destroy(gameObject);
        }
    }
}
