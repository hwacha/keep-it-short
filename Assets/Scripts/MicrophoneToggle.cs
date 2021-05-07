using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneToggle : MonoBehaviour
{
    bool isMicrophoneUp;
    public GameController controller;

    public Vector3 downPosition = new Vector3(0.25f, -0.45f, 0.58f);
    public Vector3 downRotation = new Vector3(20, 0, 15);

    public Vector3 upPosition = new Vector3(0.15f, -0.18f, 0.63f);
    public Vector3 upRotation = new Vector3(-65, 0, 15);

    public PlayerMovement pm;

    void Start()
    {
        isMicrophoneUp = false;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) && controller.gameState == 1)
        {
            toggleMic(!isMicrophoneUp);
        }
    }

    public void toggleMic(bool raiseUp)
    {
        isMicrophoneUp = raiseUp;
        if (isMicrophoneUp)
        {
            transform.localPosition = new Vector3(upPosition.x, upPosition.y, upPosition.z);
            transform.localRotation = Quaternion.Euler(upRotation);

            pm.speed = pm.micUpSpeed;

            controller.MicUp();

        }
        else
        {
            // we're switching to a down position
            transform.localPosition = new Vector3(downPosition.x, downPosition.y, downPosition.z);
            transform.localRotation = Quaternion.Euler(downRotation);

            pm.speed = pm.micDownSpeed;

            controller.MicDown();
        }
    }
}
