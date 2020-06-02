using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public float turnSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0.0f, -5f, 15f);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Move camera as mouse moves
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right) * offset;
        }
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = player.transform.position - offset; //Move camera with ball
        transform.LookAt(player.transform.position);
    }
}
