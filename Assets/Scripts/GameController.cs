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
    public GameObject bar;
    public GameObject progress;
    #endregion

    #region speech
    public string playerText;
    public AudioSource playerAudio;

    public float visual_cutoff_start = 5f;
    public float visual_cutoff_end = 10f;
    public float audio_cutoff_start = 10f;
    public float audio_cutoff_end = 12f;
    public int currentSentence = 0;
    private int nextSentence = -1;
    private bool audioStarted = false;
    #endregion

    public int gameState = 0;
    private HashSet<string> currTalkabout = new HashSet<string>();
    private HashSet<string> currIncitement = new HashSet<string>();
    private Dictionary<string, Vector3> enemyPositions = new Dictionary<string, Vector3>();
    private Vector3 playerStart;
    private Sentence[] sentences = new Sentence[]{
        new Sentence() {
            // 0
            o1NextState = 1,
        },
        new Sentence() {
            audiofile = "1",
            subtitles = "Let’s give it up for the bride and groom! Natalie and Greg! Woooo!",
            o1Text = "[Q] Lightly roast :)",
            o1NextState = 2,
            talkabout = new string[] {"Greg", "Natalie"},
        },
        new Sentence() {
            audiofile = "2",
            subtitles = "I mean, who can believe it? I never thought Greg could talk to a girl, let alone get one to marry him!",
            o1Text = "[Q] Keep it going.",
            o1NextState = 3,
            talkabout = new string[] {"Greg"},
            incitement = new string[] {"Greg"},
        },
        new Sentence() {
            audiofile = "3",
            subtitles = "Nobody thought it would last! I remember talking to our buddy Ed when they started dating, and he said he didn't think it would last a month!",
            o1Text = "[Q] Elaborate.",
            o1NextState = 4,
            talkabout = new string[] {"Ed"},
        },
        new Sentence() {
            audiofile = "4",
            subtitles = "Not even a month! Can you believe it? Come on, Lowball Ed.",
            o1Text = "[Q] Riff.",
            o1NextState = 5,
            talkabout = new string[] {"Ed"},
        },
        new Sentence() {
            audiofile = "5",
            subtitles = "I mean, I gave them a whole 60 days!",
            o1Text = "[Q] Move on.",
            o1NextState = 6,
        },
        new Sentence() {
            audiofile = "6",
            subtitles = "For real, though, I've known both of them a long time, and it's been wonderful to see them grow together as a couple.",
            o1Text = "[Q] Reminisce.",
            o1NextState = 7,
        },
        new Sentence() {
            audiofile = "7",
            subtitles = "Greg and I, we go back to college. We were roommates together freshman year. You were a character, weren't you, Greg?",
            o1Text = "[Q] Talk about his hygiene",
            o1NextState = 8,
            o2Text = "[E] Talk about his social skills.",
            o2NextState = 12,
        },
        new Sentence() {
            audiofile = "8",
            subtitles = "I mean this guy was a slob! The man didn’t know how to throw anything away. You could see where his side of the room started because from the wall of tissues and leftover food.",
            o1Text = "[Q] Elaborate.",
            o1NextState = 9
        },
        new Sentence() {
            audiofile = "9",
            subtitles = "I mean, hell, I don't think he discovered deodorant until sophomore year!",
            o1Text = "[Q] Walk it back.",
            o1NextState = 10,
        },
        new Sentence() {
            audiofile = "10",
            subtitles = "I mean, Greg, you do clean yourself up pretty well.",
            o1Text = "[Q] Unwalk it a little.",
            o1NextState = 11,
        },
        new Sentence() {
            audiofile = "11",
            subtitles = "But man, I wish you got clean a little more often!",
            o1Text = "[Q] Share some love.",
            o1NextState = 15,
        },
        new Sentence() {
            audiofile = "12",
            subtitles = "You were a pretty awkward guy. For the first few weeks, you wouldn't make eye contact at all!",
            o1Text = "[Q] Now that I think of it...",
            o1NextState = 13,
        },
        new Sentence() {
            audiofile = "13",
            subtitles = "He still hasn't figured out how to talk to people, has he? Always interjecting with some book he read. Kinda ruins the flow of conversation when you butt in like that, Greg!",
            o1Text = "[Q] Pun it up.",
            o1NextState = 14,
        },
        new Sentence() {
            audiofile = "14",
            subtitles = "We get it! You read a lot of books. Have you ever tried reading the room?",
            o1Text = "[Q] Share some love.",
            o1NextState = 15
        },
        new Sentence() {
            audiofile = "15",
            subtitles = "Anyway, despite some rough patches, me and Greg have become pretty close friends.",
            o1Text = "[Q] Time for Natalie.",
            o1NextState = 16,
        },
        new Sentence() {
            audiofile = "16",
            subtitles = "Now, Natalie. I actually met her before Greg did, if you can believe it.",
            o1Text = "[Q] Describe.",
            o1NextState = 17,
            talkabout = new string[] {"Natalie"},
        },
        new Sentence() {
            audiofile = "17",
            subtitles = "Man, back then, Nat was beautiful. I mean, she's pretty enough now, but God, back then, she was a smokeshow.",
            o1Text = "[Q] Reminisce.",
            o1NextState = 18,
            talkabout = new string[] {"Natalie"},
        },
        new Sentence() {
            audiofile = "18",
            subtitles = "I sat behind her in organic chemistry, and I'd spend the whole time just staring at her, thinking all sorts of thoughts, if you know what I mean.",
            o1Text = "[Q] Move on.",
            o1NextState = 19,
            talkabout = new string[] {"Natalie"},
        },
        new Sentence() {
            audiofile = "19",
            subtitles = "I mean, I don't want to dwell on it...",
            o1Text = "[Q] Maybe dwell on it a little, actually.",
            o1NextState = 20,
            talkabout = new string[] {"Natalie"},
        },
        new Sentence() {
            audiofile = "20",
            subtitles = "But damnnnnnnnnn, you know? She was a total babe. I couldn't keep my eyes off her!",
            o1Text = "[Q] Alright, move on for real this time.",
            o1NextState = 21,
            talkabout = new string[] {"Natalie"},
            incitement = new string[] {"Natalie"}
        },
        new Sentence() {
            audiofile = "21",
            subtitles = "Alright, sorry, sorry, I'll keep going.",
            o1Text = "[Q] Keep going.",
            o1NextState = 22
        },
        new Sentence() {
            audiofile = "22",
            subtitles = "It took the whole semester for me to get the courage to ask her out, and what do you know, she turned me down.",
            o1Text = "[Q] Topical humor.",
            o1NextState = 23
        },
        new Sentence() {
            audiofile = "23",
            subtitles = "It broke my heart. I thought we had some organic chemistry of our own!",
            o1Text = "[Q] They need to know the truth.",
            o1NextState = 24
        },
        new Sentence() {
            audiofile = "24",
            subtitles = "Like, honestly, more than you and Greg have. I mean, we have so much more in common!",
            o1Text = "[Q] Continue.",
            o1NextState = 25
        },
        new Sentence() {
            audiofile = "25",
            subtitles = "But that's life, you know. Sometimes, someone breaks your heart, and you gotta move on.",
            o1Text = "[Q] Connect it.",
            o1NextState = 26
        },
        new Sentence() {
            audiofile = "26",
            subtitles = "Anyway, so flash forward to senior year. Jamie's throwing one of his ragers at his house, and boy, is it going crazy.",
            o1Text = "[Q] Reminisce.",
            o1NextState = 27,
            talkabout = new string[] {"Jamie"},
        },
        new Sentence() {
            audiofile = "27",
            subtitles = "Remember Jamie's parties? I don't think Jamie does!",
            o1Text = "[Q] Check in.",
            o1NextState = 28,
            talkabout = new string[] {"Jamie"},
        },
        new Sentence() {
            audiofile = "28",
            subtitles = "Now that I think of it, Jamie, I hope you got some professional help. I don't think you were in a good place. Really a burden to your friends and family.",
            o1Text = "[Q] Move on.",
            o1NextState = 29,
            talkabout = new string[] {"Jamie"},
            incitement = new string[] {"Jamie"},
        },
        new Sentence() {
            audiofile = "29",
            subtitles = "Anyway, it's late and I'm trying to find Greg so we can both head home and play some Mario Kart.",
            o1Text = "[Q] Keep going.",
            o1NextState = 30
        },
        new Sentence() {
            audiofile = "30",
            subtitles = "I'm searching upstairs, and I open the door to Jamie's room, and who do I see?",
            o1Text = "[Q] Kicker.",
            o1NextState = 31
        },
        new Sentence() {
            audiofile = "31",
            subtitles = "Greg and Natalie, just going right at it!",
            o1Text = "[Q] Clarify.",
            o1NextState = 32
        },
        new Sentence() {
            audiofile = "32",
            subtitles = "We're not talking PG-13 stuff either, this was some real intense action.",
            o1Text = "[Q] Maybe they're not getting it?",
            o1NextState = 33
        },
        new Sentence() {
            audiofile = "33",
            subtitles = "They were fucking, folks!",
            o1Text = "[Q] Move on!",
            o1NextState = 34,
            incitement = new string[] {"Susan"}
        },
        new Sentence() {
            audiofile = "34",
            subtitles = "Alright, alright, I know, TMI!",
            o1Text = "[Q] Bring it back home.",
            o1NextState = 35
        },
        new Sentence() {
            audiofile = "35",
            subtitles = "But that’s honestly how I found out Greg and Natalie were seeing each other. Greg, my awkward best friend, and Natalie, someone way out of my league, let alone his.",
            o1Text = "[Q] Congratulate Greg.",
            o1NextState = 36,
            o2Text = "[E] Congratulate Natalie.",
            o2NextState = 37
        },
        new Sentence() {
            audiofile = "36",
            subtitles = "Greg, you are the luckiest motherfucker alive. I mean, never thought you’d touch a woman, let alone stumble into Natalie’s you-know-what!",
            o1Text = "[Q] Give the newlyweds some advice.",
            o1NextState = 39
        },
        new Sentence() {
            audiofile = "37",
            subtitles = "Natalie, you may not have landed a looker... or a charmer... or some one all that pleasant to be around, but who you did get is the most faithful and dependable man a woman could ever hope to get! ",
            o1Text = "[Q] Caveat.",
            o1NextState = 38
        },
        new Sentence() {
            audiofile = "38",
            subtitles = "Now, I just hope you don’t regret choosing him over a certain other eligible bachelor, if you know what I mean.",
            o1Text = "[Q] Give the newlyweds some advice.",
            o1NextState = 39
        },
        new Sentence() {
            audiofile = "39",
            subtitles = "Now, I want the best for you two. So remember, treat each other well.",
            o1Text = "[Q] Explain.",
            o1NextState = 40
        },
        new Sentence() {
            audiofile = "40",
            subtitles = "Because God knows, we’ve seen the dysfunctional messes our friends get themselves into.",
            o1Text = "[Q] Give more advice.",
            o1NextState = 41
        },
        new Sentence() {
            audiofile = "41",
            subtitles = "Make sure to always...",
            o1Text = "[Q] Be honest.",
            o1NextState = 42,
            o2Text = "[E] Be supportive.",
            o2NextState = 46
        },
        new Sentence() {
            audiofile = "42",
            subtitles = "Be honest with each other. Secrets are the death knell for any relationship.",
            o1Text = "[Q] Elaborate.",
            o1NextState = 43
        },
        new Sentence() {
            audiofile = "43",
            subtitles = "I mean, think of our friend Steve. First he was lying to his wife about working late when we were at the bar. Now I hear him and Jenny are separated. ",
            o1Text = "[Q] Check in.",
            o1NextState = 44,
            talkabout = new string[] {"Steve"},
        },
        new Sentence() {
            audiofile = "44",
            subtitles = "How’s that going for you, Steve? Don’t see Jenny here today, do I.",
            o1Text = "[Q] Comfort.",
            o1NextState = 45,
            talkabout = new string[] {"Steve"},
        },
        new Sentence() {
            audiofile = "45",
            subtitles = "Honesty is the best policy, my dude.",
            o1Text = "[Q] Give long-term advice.",
            o1NextState = 50,
            talkabout = new string[] {"Steve"},
            incitement = new string[] {"Steve"},
        },
        new Sentence() {
            audiofile = "46",
            subtitles = "It’s important to support your partner.",
            o1Text = "[Q] Spread the love.",
            o1NextState = 47
        },
        new Sentence() {
            audiofile = "47",
            subtitles = "Look at Maddie. She’s never even entertained Derek’s dreams of becoming a jazz guitarist.",
            o1Text = "[Q] Sympathize.",
            o1NextState = 48,
            talkabout = new string[] {"Maddie", "Derek"},
        },
        new Sentence() {
            audiofile = "48",
            subtitles = "I mean, we’ve all had to listen to Derek perform, so we know he’s not going anywhere. He doesn’t have the talent or the vision.",
            o1Text = "[Q] Motivate.",
            o1NextState = 49,
            talkabout = new string[] {"Maddie", "Derek"},
            incitement = new string[] {"Maddie", "Derek"},
        },
        new Sentence() {
            audiofile = "49",
            subtitles = "But you agreed to marry the hack, and that still means something! You can’t just threaten to leave with the kids every six months.",
            o1Text = "[Q] Give long-term advice.",
            o1NextState = 50,
        },
        new Sentence() {
            audiofile = "50",
            subtitles = "As years go by, make sure you...",
            o1Text = "[Q] Keep up your communication.",
            o1NextState = 51,
            o2Text = "[E] Keep up the intimacy :)",
            o2NextState = 54
        },
        new Sentence() {
            audiofile = "51",
            subtitles = "...talk to each other, and stay on the same page.",
            o1Text = "[Q] Contextualize.",
            o1NextState = 52
        },
        new Sentence() {
            audiofile = "52",
            subtitles = "Jerry, I get you were born in the silent generation, but you gotta keep your wife looped in.",
            o1Text = "[Q] Keep the audience looped in.",
            o1NextState = 53,
            talkabout = new string[] {"Jerry"},
        },
        new Sentence() {
            audiofile = "53",
            subtitles = "This man saw how great a daughter Natalie turned out and started a whole second family! Seems like the kind of thing you should tell Susan about, though!",
            o1Text = "[Q] Wrap up.",
            o1NextState = 57,
            talkabout = new string[] {"Jerry"},
            incitement = new string[] {"Jerry"} // or Jerry
        },
        new Sentence() {
            audiofile = "54",
            subtitles = "... keep up the intimacy. Look, I’ve seen you two go at it, I know you have a lot going on in the bedroom. But marriage can be hard.",
            o1Text = "[Q] Offer a cautionary example.",
            o1NextState = 55
        },
        new Sentence() {
            audiofile = "55",
            subtitles = "Just look at Mike. He has to take these meds for his depression, and now Jessica says he’s never in the mood.",
            o1Text = "[Q] Inspire.",
            o1NextState = 56,
            talkabout = new string[] {"Mike", "Jessica"},
        },
        new Sentence() {
            audiofile = "56",
            subtitles = "Come on Mike! Jessica wants you to show her what it was like on your honeymoon. Are you really “tired” every night?",
            o1Text = "[Q] Wrap up.",
            o1NextState = 57,
            talkabout = new string[] {"Mike", "Jessica"},
            incitement = new string[] {"Mike", "Jessica"}
        },
        new Sentence() {
            audiofile = "57",
            subtitles = "Alright, well, I’ve said a lot, and I’m almost done. Nothing but love for the both of you, you’ll be a wonderful couple.",
            o1Text = "[Q] One last thing...",
            o1NextState = 58
        },
        new Sentence() {
            audiofile = "58",
            subtitles = "But before I go, I’d like to share a few thoughts on my theory of trialectics. Now, we all know that artistic standards in the West have been in decline for the last century...",
        }
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
        public int o1NextState { get; set; } = -1;
        public string o2Text { get; set; }
        public int o2NextState { get; set; } = -1;
        public string[] incitement { get; set; }
        public string[] talkabout { get; set; }

    }
    void Start()
    {
        var guests = GameObject.Find("Guests");
        foreach (Transform t in guests.transform)
        {
            enemyPositions.Add(t.gameObject.name, t.position);
        }
        var player = GameObject.Find("Player");
        playerStart = player.transform.position;

        GameObject.Find("Camera").GetComponent<MouseLook>().Disable();



        InitGame();
    }
    public void InitGame()
    {
        currentSentence = 0; // should be set to 0
        nextSentence = 2; // should be set to 2
        //var enemy = GameObject.Find("Guests/Natalie");
        //enemy.GetComponent<EnemyAnimation>().Incite();
        //enemy = GameObject.Find("Guests/Greg");
        //enemy.GetComponent<EnemyAnimation>().Incite();
        var o1Text = "[Q] Get 'em goin'";
        string o2Text = null;
        if (currentSentence != 0)
        {
            o1Text = sentences[currentSentence].o1Text;
            o2Text = sentences[currentSentence].o2Text;
        }
        optionsText.GetComponent<Text>().text = o1Text + "\n" + o2Text;
        MicDown();
        // store original enemy positions
    }
    // Update is called an undefined number of times per frame
    void Update()
    {
        //Debug.Log(currentSentence);
        if (gameState == 0)
        {
            if (playerAudio.clip == null) {
                // we load and play the wedding music.
                AudioClip audioClip = Resources.Load("wedding-march-short") as AudioClip;
                playerAudio.clip = audioClip;

                playerAudio.Play();    
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                gameState = 1;
                GameObject.Find("Camera").GetComponent<MouseLook>().Enable();
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

                    sentence = sentences[nextSentence];
                    subtitle.GetComponent<Text>().text = sentence.subtitles;
                    ToggleSubtitles(true);

                    foreach (var x in currTalkabout)
                    {
                        var enemy = GameObject.Find("Guests/" + x);
                        enemy.GetComponent<EnemyAnimation>().StopTalkingAbout();
                    }
                    currTalkabout = new HashSet<string>();
                    if (sentence.talkabout != null)
                    {
                        for (var i = 0; i < sentence.talkabout.Length; i++)
                        {
                            var enemy = GameObject.Find("Guests/" + sentence.talkabout[i]);
                            enemy.GetComponent<EnemyAnimation>().TalkAbout();
                            currTalkabout.Add(sentence.talkabout[i]);
                        }
                    }

                    AudioClip audioClip = Resources.Load("Speech/" + sentence.audiofile) as AudioClip;
                    playerAudio.clip = audioClip;
                    playerAudio.Play();
                    audioStarted = true;
                }

                // we're playing this audio... still in currentTextClip 0...

                // we only get here after the audio's done!
            }

            if (playerAudio.isPlaying) {
                // not actually a percentage
                float percentDone = Mathf.Lerp(0, 1, playerAudio.time / playerAudio.clip.length);

                var progressRt = progress.GetComponent<RectTransform>();
                progressRt.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, 500 * percentDone);
                progressRt.localPosition = new Vector3(250 * (-1 + percentDone),
                    progressRt.localPosition.y, progressRt.localPosition.z);
            }

            if (currentSentence == 57 && playerAudio.isPlaying)
            {
                Debug.Log(playerAudio.time);

                if (playerAudio.time > visual_cutoff_end)
                {
                    gameState = 3;
                }

            }
            // as we go to the next state, we must set the text
            // of the options menu to the next state.
            if (!playerAudio.isPlaying && audioStarted)
            {
                audioStarted = false;

                ToggleSubtitles(false);

                currentSentence = nextSentence;
                sentence = sentences[nextSentence];

                optionsText.GetComponent<Text>().text = sentence.o1Text + "\n" + sentence.o2Text;

                options.active = true;

                if (sentence.incitement != null)
                {
                    for (var i = 0; i < sentence.incitement.Length; i++)
                    {
                        var enemy = GameObject.Find("Guests/" + sentence.incitement[i]);
                        enemy.GetComponent<EnemyAnimation>().Incite();
                        currIncitement.Add(sentence.incitement[i]);
                    }
                }
            }
        }
        if (gameState == 3)
        {
            if (!playerAudio.isPlaying)
            {
                this.MicDown();
                ToggleSubtitles(false);
            }
            else
            {
                playerAudio.volume = Mathf.Lerp(1, 0, (playerAudio.time - audio_cutoff_start) / (audio_cutoff_end - audio_cutoff_start));
            }
        }
        if (gameState == 2)
        {
            this.MicDown();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                RestartGame();
            };
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReloadCheckpoint();
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
        ToggleSubtitles(false);
        options.active = true;

        // we collapse the canvas.
        canvas.active = false;
    }

    public void ReloadCheckpoint()
    {
        ResetObjects();
        var guests = GameObject.Find("Guests");
        foreach (Transform t in guests.transform)
        {

            if (currTalkabout.Contains(t.gameObject.name))
            {
                var enemy = t.gameObject.GetComponent<EnemyAnimation>();
                enemy.TalkAbout();
            }

            if (currIncitement.Contains(t.gameObject.name))
            {
                var enemy = t.gameObject.GetComponent<EnemyAnimation>();
                enemy.Incite();
            }
        }
        gameState = 1;

    }

    void ToggleSubtitles(bool active) {
        subtitle.active = active;
        bar.active = active;
        progress.active = active;
    }

    public void RestartGame()
    {
        ResetObjects();
        InitGame();
        gameState = 1;
        currentSentence = 0;
        nextSentence = 2;
    }
    public void ResetObjects()
    {
        var guests = GameObject.Find("Guests");
        foreach (Transform t in guests.transform)
        {
            var enemy = t.gameObject.GetComponent<EnemyAnimation>();
            enemy.Pacify();
            enemy.StopTalkingAbout();
            t.position = enemyPositions[t.gameObject.name];
        }
        var player = GameObject.Find("Player");
        player.transform.position = playerStart;
    }
}
