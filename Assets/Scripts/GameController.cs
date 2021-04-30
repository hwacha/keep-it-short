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

    public string playerIntroText2;
    public AudioSource playerIntroAudio2;

    public string gregText1;
    public AudioSource gregAudio1;

    public string natalieText1;
    public AudioSource natalieAudio1;
    #endregion

    private int currentTextClip = 0;
    private int nextTextClip = 1;
    private bool audioHasBeenPlaying = false;

    // Start is called before the first frame update
    /*
        - references to the UI elements
        - references to game currentTextClip
            we'll have some sort of game over currentTextClip
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
        // the first currentTextClip of the game.
        // canvas has options: [Q] get it started
        if (currentTextClip == 0) {
            if (canvas.active && Input.GetKeyDown(KeyCode.Q) && !audioHasBeenPlaying) {
                options.active = false;

                subtitle.GetComponent<Text>().text = playerIntroText;
                subtitle.active = true;

                playerIntroAudio.Play();
                audioHasBeenPlaying = true;

                // we're playing this audio... still in currentTextClip 0...

                // we only get here after the audio's done!
            }

            // as we go to the next state, we must set the text
            // of the options menu to the next state.
            if (!playerIntroAudio.isPlaying && audioHasBeenPlaying) {
                audioHasBeenPlaying = false;

                subtitle.active = false;

                optionsText.GetComponent<Text>().text = "[Q] Take it away";
                options.active = true;
                nextTextClip = 2;
                currentTextClip = 1;
            }

            // if there's no prompt, then we need to increment the
            // currentTextClip and move to the next subtitle/audio clip
            // once the full audio clip has played through.
        }

        if (currentTextClip == 1) {
            if (canvas.active && Input.GetKeyDown(KeyCode.Q) && !audioHasBeenPlaying) {
                options.active = false;

                subtitle.GetComponent<Text>().text = playerIntroText2;
                subtitle.active = true;

                playerIntroAudio2.Play();
                audioHasBeenPlaying = true;
            }

            if (!playerIntroAudio2.isPlaying && audioHasBeenPlaying) {
                audioHasBeenPlaying = false;

                subtitle.active = false;

                optionsText.GetComponent<Text>().text =
                    "[Q] Share memories of Greg\n[E] Share memories of Natalie";
                options.active = true;
                currentTextClip = nextTextClip;
            }
        }

        if (currentTextClip == 2) {
            if (canvas.active && !audioHasBeenPlaying) {
                if (Input.GetKeyDown(KeyCode.Q)) {
                    options.active = false;

                    subtitle.GetComponent<Text>().text = gregText1;
                    subtitle.active = true;

                    gregAudio1.Play();
                    audioHasBeenPlaying = true;

                    nextTextClip = 3;
                }

                if (Input.GetKeyDown(KeyCode.E)) {
                    options.active = false;

                    subtitle.GetComponent<Text>().text = natalieText1;
                    subtitle.active = true;

                    natalieAudio1.Play();
                    audioHasBeenPlaying = true;

                    nextTextClip = 4;
                }

                if (nextTextClip == 3 && !gregAudio1.isPlaying && audioHasBeenPlaying) {
                    audioHasBeenPlaying = false;

                    subtitle.active = false;

                    optionsText.GetComponent<Text>().text = "[Q] Funny putdown, haha";
                    options.active = true;

                    currentTextClip = nextTextClip;
                }

                if (nextTextClip == 4 && !natalieAudio1.isPlaying && audioHasBeenPlaying) {
                    audioHasBeenPlaying = false;

                    subtitle.active = false;

                    optionsText.GetComponent<Text>().text = "[Q] Uh oh! Dating story!";
                    options.active = true;

                    currentTextClip = nextTextClip;
                }
            }
        }
    }

    public void MicUp() {
        canvas.active = true;
    }

    public void MicDown() {
        // we need to gracefully interrupt the audio.
        if (currentTextClip == 0) {
            playerIntroAudio.Stop();
        }
        if (currentTextClip == 1) {
            playerIntroAudio2.Stop();
        }
        if (nextTextClip == 3) {
            gregAudio1.Stop();
        }
        if (nextTextClip == 4) {
            natalieAudio1.Stop();
        }
        audioHasBeenPlaying = false;

        // we go back to the options menu.
        subtitle.active = false;
        options.active = true;

        // we collapse the canvas.
        canvas.active = false;        
    }
}
