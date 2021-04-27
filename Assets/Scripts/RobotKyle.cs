using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotKyle : MonoBehaviour
{
    public float speed;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.1f;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if ((h != 0) || (v != 0))
        {
            animator.SetFloat("character_speed", 1);
        }
        else
        {
            animator.SetFloat("character_speed", 0);
        }
        gameObject.transform.position = new Vector3(transform.position.x + h * speed, 0, transform.position.z + v * speed);
    }
}
