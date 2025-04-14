using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private List<GameObject> parts = new List<GameObject>();
    public AudioClip openSound, closeSound;
    private AudioSource source;

    void Start()
    {
        foreach (Transform child in transform)
        {
            parts.Add(child.gameObject);
        }
        source = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        foreach (GameObject part in parts)
        {
            part.SetActive(false);
            source.PlayOneShot(openSound);
        }
    }

    public void CloseDoor()
    {
        foreach (GameObject part in parts)
        {
            part.SetActive(true);
            source.PlayOneShot(closeSound);
        }
    }
}
