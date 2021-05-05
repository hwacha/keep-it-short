using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAnimation : MonoBehaviour
{

    public float leftBoundary = 5;
    public float rightBoundary = -5;

    public NavMeshAgent navMeshAgent;

    private float zRotationDefaultSpeed = 5f;
    private float zRotationSpeed;

    private float radius;

    private Transform playerTransform;

    public Transform wobbler;

    public GameController game;
    public bool incited = false;
    private float epsilon = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameController").GetComponent<GameController>();

        radius = 0.5f * transform.lossyScale.x;
        playerTransform = GameObject.Find("Player").transform;
        zRotationSpeed = zRotationDefaultSpeed * Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {

        // if (wobbler.rotation.z <= rightBoundary ||
        //     wobbler.rotation.z >= leftBoundary) {
        //     zRotationVelocity *= -1;
        // }
        // wobbler.Rotate(0, 0, zRotationVelocity);

        transform.rotation = transform.rotation *
            Quaternion.Lerp(
                Quaternion.AngleAxis(leftBoundary, Vector3.forward),
                Quaternion.AngleAxis(rightBoundary, Vector3.forward),
                Mathf.PingPong(Time.time * zRotationSpeed, 1));

        if (incited && game.gameState == 1)
        {
            navMeshAgent.SetDestination(playerTransform.position);

            // TODO do distance directly
            var remainingDistance = Vector3.Distance(playerTransform.position, transform.position);
            if (0 < remainingDistance &&
                remainingDistance < epsilon)
            {
                game.gameState = 2;
            }
        }
        else
        {
            // maybe wobbler?
            transform.LookAt(playerTransform);
        }
    }

    // returns false if already incited
    public bool Incite()
    {
        if (!incited)
        {
            var cube = wobbler.GetChild(1);
            var mat = cube.GetComponent<Renderer>().material;
            mat.color = Color.red;

            incited = true;
            return true;
        }
        return false;
    }

    public bool TalkAbout()
    {
        if (!incited)
        {
            var cube = wobbler.GetChild(1);
            var mat = cube.GetComponent<Renderer>().material;
            mat.color = Color.blue;
            return true;
        }
        return false;
    }
    public bool StopTalkingAbout()
    {
        if (!incited)
        {
            var cube = wobbler.GetChild(1);
            var mat = cube.GetComponent<Renderer>().material;
            mat.color = Color.white;
            return true;
        }
        return false;
    }
    public bool Pacify()
    {
        if (incited)
        {
            var cube = wobbler.GetChild(1);
            var mat = cube.GetComponent<Renderer>().material;
            mat.color = Color.white;
            incited = false;
            navMeshAgent.ResetPath();
            return true;
        }
        return false;
    }
}
