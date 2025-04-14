using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public bool loading = false;

    private void Awake()
    {
        if (SaveManager.instance == null)
        {
            DontDestroyOnLoad(this);
            SaveManager.instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveGame(int puzzleNum)
    {
        PlayerPrefs.SetInt("PuzzleNum", puzzleNum);
    }

    public int LoadGame()
    {
        int g = 0;
        if (PlayerPrefs.HasKey("PuzzleNum"))
            g = PlayerPrefs.GetInt("PuzzleNum");
        return g;
    }
}
