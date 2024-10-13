using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public PlayerMovement player;
    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isMobileControls == false)
        {
            for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
        }
    }
}
