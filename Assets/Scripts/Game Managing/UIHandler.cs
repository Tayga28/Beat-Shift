using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public MenuOrbit menuCamera;
    public GameManager gameManager;
    public PlayerMovement player;
    public Button[] buttons;
    public TMP_Text title;
    public Button startButton;
    public Text score;
    // Start is called before the first frame update
    void Start()
    {
        //cam.isStartButtonPressed = false;
        //cam.orbitMode = true;;
        if (gameManager.menuMode)
        {
            player.userOnMenu = true;
            player.hasGameplayStarted = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isMobileControls == false)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        } else {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }

        // Disable the score for the main menu
        if (player.userOnMenu) 
        {
            score.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
            title.gameObject.SetActive(true);
        }
        if (player.hasGameplayStarted) 
        {
            score.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
        }
    }

    public void StartButtonPressed()
    {
        menuCamera.OnPlayButtonPressed();
        player.userOnMenu = false;
        gameManager.menuMode = false;
        gameManager.gameMode = true;
        player.hasGameplayStarted = true;
        startButton.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
    }
}
