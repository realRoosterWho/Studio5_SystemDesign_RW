using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class deathzone_Logic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the death zone");
            other.gameObject.GetComponent<PlayerLogic>().ResetPlayerPosition();
            
            //触发事件CoinRemake
            //重新加载场景，不要调用Scemanager，直接重新加载场景
            // Application.LoadLevel(Application.loadedLevel);
            
        }
    }
}
