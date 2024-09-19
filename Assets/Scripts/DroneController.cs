using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    Rigidbody rigidbody;
    float up_down_axis, forward_backward_axis, right_left_axis;
    float forward_backward_angle = 0, right_left_angle = 0;
    [SerializeField]
    float speed = 1.3f, angle = 25;
    //Animator //animator;
    bool isOnGround = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Controlls();
        transform.localEulerAngles = Vector3.back * right_left_angle + Vector3.right * forward_backward_angle;
    }

    private void FixedUpdate()
    {
        rigidbody.AddRelativeForce(right_left_axis,up_down_axis,forward_backward_axis);
    }

    void Controlls()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            up_down_axis = 10 * speed;
            //animator.SetBool("Fly", true);
            isOnGround = false;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            up_down_axis = 8;
            //animator.SetBool("Fly", false);
        }
        else
        {
            up_down_axis = 9.81f;
            //animator.SetBool("Fly", false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, angle, Time.deltaTime);
            forward_backward_axis = speed;
            //animator.SetBool("Fly", true); ;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, -angle, Time.deltaTime);
            forward_backward_axis = -speed;
            //animator.SetBool("Fly", true); ;
        }
        else
        {
            Mathf.Lerp(forward_backward_angle, 0, Time.deltaTime);
            forward_backward_axis = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            right_left_angle = Mathf.Lerp(right_left_angle, angle, Time.deltaTime);
            right_left_axis = speed;
            //animator.SetBool("Fly", true); ;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            right_left_angle = Mathf.Lerp(right_left_angle, -angle, Time.deltaTime);
            right_left_axis = -speed;
            //animator.SetBool("Fly", true); ;
        }
        else
        {
            Mathf.Lerp(right_left_angle, 0, Time.deltaTime);
            right_left_axis = 0;
        }

        if(Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.D))
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, angle, Time.deltaTime);
            forward_backward_axis = 0.5f * speed;
            right_left_axis = 0.5f * speed;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, -angle, Time.deltaTime);
            forward_backward_axis = 0.5f * speed;
            right_left_axis = -0.5f * speed;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, -angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, angle, Time.deltaTime);
            forward_backward_axis = -0.5f * speed;
            right_left_axis = 0.5f * speed;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, -angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, -angle, Time.deltaTime);
            forward_backward_axis = -0.5f * speed;
            right_left_axis = -0.5f * speed;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
            isOnGround = true;
    }
}
