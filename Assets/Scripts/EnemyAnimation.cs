using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAnimation : MonoBehaviour
{

    public float leftBoundary = 0.005f;
    public float rightBoundary = -0.005f;

    public NavMeshAgent navMeshAgent;

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
        transform.rotation *= Quaternion.Euler(1, 0, 1);
        navMeshAgent.SetDestination(playerTransform.position);

        if (active)
        {
            // "wobble"
            if (transform.rotation.z <= rightBoundary ||
                transform.rotation.z >= leftBoundary)
            {
                zRotationVelocity *= -1;
            }
            transform.Rotate(0, 0, zRotationVelocity);

            //transform.position += Vector3.Normalize(transform.forward) * 0.01f;

            // "walk" swivel
            // transform.parent.Rotate(0, yRotationVelocity, 0);
        }

    }
}
