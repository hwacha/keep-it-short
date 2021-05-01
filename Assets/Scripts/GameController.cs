using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region references
    public GameObject canvas;
    public GameObject subtitle;
    public GameObject options;
    public GameObject optionsText;
    #endregion

    #region speech
    public string playerText;
    public AudioSource playerAudio;

    private int currentSentence = 0;
    private int nextSentence = -1;
    private bool audioStarted = false;
    #endregion

    public int gameState = 0;

    private Sentence[] sentences = new Sentence[]{
        new Sentence() {
            // 0
            o1NextState = 1,
        },
        new Sentence() {
            // 0
            audiofile = "intro",
            subtitles = "Let’s give it up for the bride and groom! Natalie and Greg!",
            o1Text = "[Q] Take it away",
            o1NextState = 2,
        },
        new Sentence() {
            // 1
            audiofile = "intro2",
            subtitles = "Natalie and Greg! Who can believe it!",
            o1Text = "[Q] Share memories of Greg",
            o1NextState = 3,
            o2Text = "[E] Share memories of Natalie",
            o2NextState = 4,
        },
        new Sentence() {
            // 2
            audiofile = "intro",
            subtitles = "Greg and I, we go back to college. We were roommates together freshman year.",
            o1Text = "[Q] Funny putdown, haha",
            o1NextState = -1,
            incitement = "Natalie"
        },
        new Sentence() {
            // 3
            audiofile = "KERNKRAFT",
            subtitles = "you know the thing",
            o1Text = "[Q] another test hahhahaha",
            o1NextState = -1,
            incitement = "Natalie"
        },
    };
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
    public class Sentence
    {
        public string audiofile { get; set; }
        public string subtitles { get; set; }
        public string o1Text { get; set; }
        public int o1NextState { get; set; }
        public string o2Text { get; set; }
        public int o2NextState { get; set; }
        public string incitement { get; set; }

    }
    void Start()
    {
        currentSentence = 0;
        nextSentence = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                gameState = 1;
            };
        }
        if (gameState == 1)
        {
            var sentence = sentences[currentSentence];
            if (canvas.active && !audioStarted)
            {
                var hasQ = (Input.GetKeyDown(KeyCode.Q) && sentence.o1NextState != -1);
                var hasE = (Input.GetKeyDown(KeyCode.E) && sentence.o2NextState != -1);

                if (hasQ || hasE)
                {

                    nextSentence = sentence.o1NextState;
                    if (!hasQ)
                    {
                        nextSentence = sentence.o2NextState;
                    }
                    options.active = false;

                    subtitle.GetComponent<Text>().text = sentences[nextSentence].subtitles;
                    subtitle.active = true;

                    AudioClip audioClip = Resources.Load(sentences[nextSentence].audiofile) as AudioClip;
                    playerAudio.clip = audioClip;
                    playerAudio.Play();
                    audioStarted = true;
                }

                // we're playing this audio... still in currentTextClip 0...

                // we only get here after the audio's done!
            }

            // as we go to the next state, we must set the text
            // of the options menu to the next state.
            if (!playerAudio.isPlaying && audioStarted)
            {
                audioStarted = false;

                subtitle.active = false;
                currentSentence = nextSentence;
                sentence = sentences[nextSentence];

                optionsText.GetComponent<Text>().text = sentence.o1Text + "\n" + sentence.o2Text;

                options.active = true;

                if (sentence.incitement != null)
                {
                    var enemy = GameObject.Find("Guests/" + sentence.incitement);
                    enemy.GetComponent<EnemyAnimation>().Incite();
                }
            }
        }
        if (gameState == 2 || gameState == 3)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("test scene");
            };
        }
        // if there's no prompt, then we need to increment the
        // currentTextClip and move to the next subtitle/audio clip
        // once the full audio clip has played through.
    }

    public void MicUp()
    {
        canvas.active = true;
    }

    public void MicDown()
    {
        // we need to gracefully interrupt the audio.
        playerAudio.Stop();
        audioStarted = false;

        // we go back to the options menu.
        subtitle.active = false;
        options.active = true;

        // we collapse the canvas.
        canvas.active = false;
    }
}
