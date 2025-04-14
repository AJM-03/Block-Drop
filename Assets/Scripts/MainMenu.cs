using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        if (SaveManager.instance.LoadGame() == 0)
        {
            continueButton.GetComponent<Button>().enabled = false;
            Color c = continueButton.GetComponent<Image>().color;
            c.r = .5f;
            c.g = .5f;
            c.b = .5f;
            continueButton.GetComponent<Image>().color = c;
        }
    }

    public void Continue()
    {
        SaveManager.instance.loading = true;
        SceneManager.LoadScene("Main");
    }

    public void NewGame()
    {
        SaveManager.instance.loading = false;
        SceneManager.LoadScene("Main");
    }
}
