using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private XboxController Controller = XboxController.All; //Anybody can use the menu options

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.A, Controller))
        {
            SceneManager.LoadScene(1);
        }
        else if (XCI.GetButtonDown(XboxButton.B, Controller))
        {
            Debug.Log("Quitting!");
            Application.Quit();
        }
    }
}
