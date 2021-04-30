using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxisRaw("Horizontal");
        float dz = Input.GetAxisRaw("Vertical");
        Vector3 move = Vector3.Normalize(transform.right * dx + transform.forward * dz) * speed;
        controller.Move(move * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 1.05f, transform.position.z);
    }
}
