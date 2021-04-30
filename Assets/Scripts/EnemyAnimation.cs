using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAnimation : MonoBehaviour
{

    public float leftBoundary = 0.05f;
    public float rightBoundary = -0.05f;

    public NavMeshAgent navMeshAgent;

    private float zRotationVelocity = -0.2f;

    private float radius;

    public Transform playerTransform;

    public Transform wobbler;

    public GameObject menuScreen;

    public bool incited = false;
    private float epsilon = 0.5f;

    // Start is called before the first frame update
    void Start() {
        radius = 0.5f * transform.lossyScale.x;
        menuScreen = GameObject.Find("MenuScreen");
    }

    // Update is called once per frame
    void Update() {
        // TODO
        // // "wobble"
        // if (wobbler.rotation.z <= rightBoundary ||
        //     wobbler.rotation.z >= leftBoundary) {
        //     zRotationVelocity *= -1;
        // }
        // wobbler.Rotate(0, 0, zRotationVelocity);

        if (incited) {
            navMeshAgent.SetDestination(playerTransform.position);

            // TODO do distance directly
            if (0 < navMeshAgent.remainingDistance &&
                navMeshAgent.remainingDistance < epsilon) {
                menuScreen.transform.GetChild(0).gameObject.active = true;
            }
        } else {
            // maybe wobbler?
            transform.LookAt(playerTransform);
        }
    }
    
    // returns false if already incited
    public bool Incite() {
        if (!incited) {
            var cube = wobbler.GetChild(1);
            var mat = cube.GetComponent<Renderer>().material;
            mat.color = Color.red;

            incited = true;
            return true;
        }
        return false;
    }

    public void Pacify() {
        // if we want a place for the enemy to go when
        // it's "pacified", we'll want to set the destination
        // to a neutral location.
    }
}
