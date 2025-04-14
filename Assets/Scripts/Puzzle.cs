using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Puzzle : MonoBehaviour
{
    public List<Block> blocks = new List<Block>();
    public Transform startPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BlockSelector.instance.StartPuzzle(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BlockSelector.instance.EndPuzzle();
        }
    }
}