using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotKyle : MonoBehaviour
{
	public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
  		float h = Input.GetAxisRaw("Horizontal");
  		float v = Input.GetAxisRaw("Vertical");
  		Debug.Log(h);
  		Debug.Log(v);
  		gameObject.transform.position = new Vector3(transform.position.x + h*speed, 0, transform.position.z + v * speed);
    }
}
