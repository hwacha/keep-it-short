using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneToggle : MonoBehaviour
{
    bool isMicrophoneUp;

    public Vector3 downPosition = new Vector3(0.25f, -0.45f, 0.58f);
    public Vector3 downRotation = new Vector3(20, 0, 15);

    public Vector3 upPosition = new Vector3(0.15f, -0.18f, 0.63f);
    public Vector3 upRotation = new Vector3(-65, 0, 15);

    void Start() {
        isMicrophoneUp = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            isMicrophoneUp = !isMicrophoneUp;

            // we're switching to an up position
            if (isMicrophoneUp) {
                transform.localPosition = new Vector3(upPosition.x, upPosition.y, upPosition.z);
                transform.localRotation = Quaternion.Euler(upRotation);
            } else {
                // we're switching to a down position
                transform.localPosition = new Vector3(downPosition.x, downPosition.y, downPosition.z);
                transform.localRotation = Quaternion.Euler(downRotation);
            }
        }
    }
}
