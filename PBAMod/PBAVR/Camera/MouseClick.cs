using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PBAVR
{
    class MouseClick : MonoBehaviour
    {
        Button button,restart, checkpoint, settings,exitbutton;

        private void Start()
        {
            // main menu button
            if (SceneManager.GetActiveScene().name == "MainMenu_MASTER")
                button = GameObject.Find("ModeButton_Left_HITBOX").GetComponent<Button>();

            // pause menu buttons
            if (transform.name == "PauseMenuCanvas")
            {
                settings = GameObject.Find("SettingsButton_Hitbox").GetComponent<Button>();
                checkpoint = GameObject.Find("RestartFromCheckpointButton_Hitbox").GetComponent<Button>();
                restart = GameObject.Find("RestartLevelButton_Hitbox").GetComponent<Button>();
                exitbutton = GameObject.Find("LevelSelectButton_Hitbox").GetComponent<Button>();
            }

        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "MainMenu_MASTER")
            if (Input.GetButtonDown("joystick button 2") || Input.GetButtonDown("joystick button 1") || Input.GetButtonDown("joystick button 0") || Input.GetKeyDown(KeyCode.Return))
            { 
                    button.onClick.Invoke();     
            }

            if (transform.name == "PauseMenuCanvas")
            {
                if (Input.GetButtonDown("joystick button 0") || Input.GetKeyDown(KeyCode.V))
                {
                    settings.onClick.Invoke();
                }
                if (Input.GetButtonDown("joystick button 1") || Input.GetKeyDown(KeyCode.B))
                {
                    checkpoint.onClick.Invoke();
                }
                if (Input.GetButtonDown("joystick button 2") || Input.GetKeyDown(KeyCode.N))
                {
                    restart.onClick.Invoke();
                }
                if (Input.GetButtonDown("joystick button 3") || Input.GetKeyDown(KeyCode.M))
                {
                    exitbutton.onClick.Invoke();
                }
            }


        }
    }
}
