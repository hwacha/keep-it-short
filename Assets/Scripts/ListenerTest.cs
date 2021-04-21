using UnityEngine;
// using UnityEngine.AudioModule;

public class ListenerTest : MonoBehaviour
{
    void Start()
    {
        foreach (var device in Microphone.devices) {
            Debug.Log("Name: " + device);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
