using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//读取json
using System.IO;

public class A2U_test : MonoBehaviour
{
    
    //获取json
    public string json;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public float jumpForce = 3f;
    public bool isGrounded = true;
    
    //对物体增加最大速度限制
    public float maxSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {

            //get the rigidbody component
            rb = GetComponent<Rigidbody>();
            test_data = this.gameObject;
    }

    void Update()
    {
        // Check if the object is grounded，用碰撞逻辑来
        isGrounded = Physics.Raycast(test_data.transform.position, Vector3.down, 5f);
        Debug.Log(isGrounded);

        
        //读取json
        json = File.ReadAllText(Application.dataPath + "/Python/data.json");
        Debug.Log(json);
        //解析json。其结构为：{"x": 6.0, "y": -10.0, "z": 0.0}。分别获取三个变量
        //如果为空，不解析，否则解析        Vector3 data = JsonUtility.FromJson<Vector3>(json);
        if (json != "")
        {
            Vector3 data = JsonUtility.FromJson<Vector3>(json);
            Debug.Log(data);
            //调用Move函数
            Move(data);
        }
        

       
        
        //最大速度限制
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
        
    }
    


    void Move(Vector3 data)
    {
        rb.AddForce(0,0,data.x * sensitivity * Time.deltaTime, ForceMode.VelocityChange); //VelocityChange是一个力的模式,作用在物体上的力会立即改变物体的速度，而不受物体的质量影响
        rb.AddForce(data.y * sensitivity * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        Debug.Log(isGrounded);
        //如果物体在地面上，可以跳跃
        if (isGrounded && data.z == 1.0)
        {
            Debug.Log("Jump");
            rb.AddForce(0,jumpForce,0, ForceMode.Impulse);
        }
    }
}
