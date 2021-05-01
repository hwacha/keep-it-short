using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private GameObject background;
    private GameObject winScreen;
    private GameObject loseScreen;
    private GameObject startScreen;

    public GameController game;
    void Start()
    {
        background = GameObject.Find("Background");
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
        if (game.gameState == 1)
        {
            // empty
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
