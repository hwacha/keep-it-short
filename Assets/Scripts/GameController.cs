using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region references
    public GameObject canvas;
    public GameObject subtitle;
    public GameObject options;
    public GameObject optionsText;
    #endregion

    #region speech
    public string playerIntroText;
    public AudioSource playerIntroAudio;
    #endregion

    private int state = 0;

    // Start is called before the first frame update
    /*
        - references to the UI elements
        - references to game state
            we'll have some sort of game over state
            we have the statuses of the NPCs
                NPCs attributes
                    - color
                    - speed
                    - wobble intensity? / some sort of angrier animation
            we have the speech
                it's broken up in some chunks
                there are decision points with hardcoded options that will
                trigger different dialogue

                we


    */
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0) {
            if (canvas.active && Input.GetKeyDown(KeyCode.Q)) {
                options.active = false;

                subtitle.GetComponent<Text>().text = playerIntroText;
                subtitle.active = true;

                playerIntroAudio.Play();

                state = 1;
            }

            // if there's no prompt, then we need to increment the
            // state and move to the next subtitle/audio clip
            // once the full audio clip has played through.
        }
    }

    public void MicUp() {
        canvas.active = true;
    }

    public void MicDown() {
        canvas.active = false;

        // we need to gracefully interrupt the audio.
    }
}
