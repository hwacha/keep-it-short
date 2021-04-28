using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    public float leftBoundary = 0.005f;
    public float rightBoundary = -0.005f;

    private float zRotationVelocity = -0.2f;
    //private float yRotationVelocity = 0.5f;

    private float radius;

    public Transform playerTransform;

    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        radius = 0.5f * transform.lossyScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);


        if (active)
        {
            // "wobble"
            if (transform.rotation.z <= rightBoundary ||
                transform.rotation.z >= leftBoundary)
            {
                zRotationVelocity *= -1;
            }
            transform.Rotate(0, 0, zRotationVelocity);

            transform.position += Vector3.Normalize(transform.forward) * 0.01f;

            // "walk" swivel
            // transform.parent.Rotate(0, yRotationVelocity, 0);
        }

    }
}
