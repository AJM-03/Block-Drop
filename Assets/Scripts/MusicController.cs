using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
    public AudioSource version1, version2, version3;
    public int currentVersion = 1;
    public float fadeSpeed;

    void Awake()
    {
        if (MusicController.instance == null)
        {
            DontDestroyOnLoad(this);
            MusicController.instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (currentVersion == 1)
        {
            version1.volume = Mathf.Clamp(version1.volume + fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
            version2.volume = Mathf.Clamp(version2.volume - fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
            version3.volume = Mathf.Clamp(version3.volume - fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
        }
        else if (currentVersion == 2)
        {
            version1.volume = Mathf.Clamp(version1.volume - fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
            version2.volume = Mathf.Clamp(version2.volume + fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
            version3.volume = Mathf.Clamp(version3.volume - fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
        }
        else if (currentVersion == 3)
        {
            version1.volume = Mathf.Clamp(version1.volume - fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
            version2.volume = Mathf.Clamp(version2.volume - fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
            version3.volume = Mathf.Clamp(version3.volume + fadeSpeed * Time.deltaTime, 0.0f, 1.0f);
        }
    }

    public void ChangeVersion(int version)
    {
        if (version != currentVersion)
        {
            currentVersion = version;
        }
    }
}
