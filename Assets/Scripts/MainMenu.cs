using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public enum Screen
    {
        TitleScreen,
        ReadyScreen
    }

	public Camera Camera;

    public RectTransform TitleScreenPanel;
    public RectTransform ReadyScreenPanel;

	public Text ReadyPlayer1Text;
	public Text ReadyPlayer2Text;
	public Text CountdownText;

	private Screen CurrentScreen = Screen.TitleScreen;

	private bool ReadyPlayer1 = false;
	private bool ReadyPlayer2 = false;
	private bool ReadyTimerCountingDown = false;
	private float ReadyTimer;

    // Start is called before the first frame update
    void Start()
    {
		TitleScreenPanel.gameObject.SetActive(true);
		ReadyScreenPanel.gameObject.SetActive(false);
		ReadyPlayer1Text.text = "Not ready";
		ReadyPlayer2Text.text = "Not ready";

		ReadyTimer = 3.0f;
	}

    // Update is called once per frame
    void Update()
    {
        switch (CurrentScreen)
        {
            case Screen.TitleScreen:

				if (GetButtonDownAny(XboxButton.A))
				{
					CurrentScreen = Screen.ReadyScreen;
					TitleScreenPanel.gameObject.SetActive(false);
					ReadyScreenPanel.gameObject.SetActive(true);
				}
				else if (GetButtonDownAny(XboxButton.B))
				{
				    Debug.Log("Quitting!");
				    Application.Quit();
				}

				break;

            case Screen.ReadyScreen:

				if (ReadyTimerCountingDown)
				{
					ReadyTimer -= Time.deltaTime;
					CountdownText.text = ((int)Mathf.Ceil(ReadyTimer)).ToString();
					CountdownText.fontSize = 100 - ((int)Mathf.Ceil(ReadyTimer)) * 15;
				}
				if (ReadyTimer <= 0.0f)
				{
					CountdownText.text = "";
					SceneManager.LoadScene(1);
				}


				if (GetButtonDownAny(XboxButton.A))
				{
					if (XCI.GetButtonDown(XboxButton.A, XboxController.First))
					{
						ReadyPlayer1 = true;
						ReadyPlayer1Text.text = "Ready!";
					}
					if (XCI.GetButtonDown(XboxButton.A, XboxController.Second))
					{
						ReadyPlayer2 = true;
						ReadyPlayer2Text.text = "Ready!";
					}

					if (ReadyPlayer1 && ReadyPlayer2 && !ReadyTimerCountingDown)
					{
						ReadyTimer = 3.0f;
						ReadyTimerCountingDown = true;
					}
				}
				else if (GetButtonDownAny(XboxButton.B))
				{
					CurrentScreen = Screen.TitleScreen;
					ReadyScreenPanel.gameObject.SetActive(false);
					TitleScreenPanel.gameObject.SetActive(true);

					ReadyTimer = 3.0f;
					ReadyTimerCountingDown = false;
					ReadyPlayer1 = false;
					ReadyPlayer2 = false;
					ReadyPlayer1Text.text = "Not ready";
					ReadyPlayer2Text.text = "Not ready";
					CountdownText.text = "";
				}

				break;
        }
    }

    //Checks if any controller has the specified button pressed
    public bool GetButtonDownAny(XboxButton button)
    {
        if (XCI.GetButtonDown(button, XboxController.First)
            || XCI.GetButtonDown(button, XboxController.Second)
            || XCI.GetButtonDown(button, XboxController.Third)
            || XCI.GetButtonDown(button, XboxController.Fourth))
            return true;
        return false;
    }
}
