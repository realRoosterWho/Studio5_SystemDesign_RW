using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2U_test : MonoBehaviour
{
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.01f;
    public float jumpForce = 3f;
    public bool isGrounded = true;

    //摩擦力
    public float friction = 0.5f;

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

        // 从 InputHandler 获取输入数据
        InputData data = InputHandler.Instance.GetInputData();

        // 如果数据不为 null，调用 Move 函数
        if (data != null)
        {
            Move(data);
        }

        //无论在不在地上，都有摩擦力，如果速度大于0，就减小速度
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity -= rb.velocity.normalized * friction * Time.deltaTime;
        }

        //最大速度限制
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Move(InputData data)
    {
        //帮我写个死区，就是说如果data的值小于50，就不要动
        if (Mathf.Abs(data.x) < 50)
        {
            data.x = 0;
        }
        if (Mathf.Abs(data.y) < 50)
        {
            data.y = 0;
        }

        rb.AddForce(0,0, -data.x * sensitivity * Time.deltaTime, ForceMode.VelocityChange); //VelocityChange是一个力的模式,作用在物体上的力会立即改变物体的速度，而不受物体的质量影响
        rb.AddForce(-data.y * sensitivity * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        Debug.Log(isGrounded);
        //如果物体在地面上，可以跳跃
        if (isGrounded && data.z == 1.0)
        {
            Debug.Log("Jump");
            rb.AddForce(0,jumpForce,0, ForceMode.Impulse);
        }
    }
}