using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float maxVelocity;

    private Rigidbody rb;
    private Transform cam;
    private bool fly;
    private bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        fly = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        Vector3 force = Camera.main.transform.TransformVector(movement);
        force.y = 0;

        if (Input.GetKey("space") & onGround) //Jump mechanic, Can only jump when touching ground
        {
            force.y = 15;
        }

        if (Input.GetKey(KeyCode.Mouse2) & fly)
        {
            if(rb.velocity.y < 0)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            force.y = 3;
        }


        //print(force.ToString() + " + " + rb.velocity.magnitude.ToString());
        rb.AddForce(force * speed);

        //Limit velocity
        if (rb.velocity.magnitude >= maxVelocity)
        {
            rb.AddForce(-force * speed);
        }
    }

    float forceAngle(Vector3 from, Vector3 to)
    {
        float angDeg = Vector3.Angle(from, to);
        return Mathf.Deg2Rad * angDeg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground")) //Check is collided object is with ground
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground")) //Check is collided object is with ground
        {
            onGround = false;
        }
    }
}
