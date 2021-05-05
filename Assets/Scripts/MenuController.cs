using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private GameObject background;
    private GameObject winScreen;
    private GameObject loseScreen;
    private GameObject startScreen;

    // private Image backgroundImage;
    public GameController game;
    void Start()
    {
        background = GameObject.Find("Background");
        // backgroundImage.
        winScreen = GameObject.Find("Win");
        loseScreen = GameObject.Find("Lose");
        startScreen = GameObject.Find("Start");
    }

    // Update is called once per frame
    void Update()
    {
        background.active = false;
        winScreen.active = false;
        loseScreen.active = false;
        startScreen.active = false;
        if (game.gameState == 0)
        {
            background.active = true;
            startScreen.active = true;

        }
        if (game.gameState == 1 && game.currentSentence == 57)
        {
            var color = new Color(1, 1, 1, 0);
            if (game.playerAudio.time > game.visual_cutoff_start)
            {
                color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (game.playerAudio.time - game.visual_cutoff_start) / (game.visual_cutoff_end - game.visual_cutoff_start));
                Debug.Log(color.a);
            }

            background.GetComponent<CanvasRenderer>().SetColor(color);
            background.active = true;

        }
        if (game.gameState == 2)
        {
            background.active = true;
            loseScreen.active = true;
        }
        if (game.gameState == 3)
        {
            background.active = true;
            winScreen.active = true;
        }
    }


}
