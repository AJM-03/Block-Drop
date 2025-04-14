using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent PressEvent, ReleaseEvent;
    public Sprite pressSprite, releaseSprite;
    private bool blockCollision;
    public LayerMask blockLayer;
    private float time, timer = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SpriteRenderer>().sprite = pressSprite;
            PressEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SpriteRenderer>().sprite = releaseSprite;
            ReleaseEvent.Invoke();
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = timer;
            var collider = Physics2D.OverlapPoint(transform.position, blockLayer);
            if (collider)
            {
                if (collider.transform.gameObject.tag == "Block" && !blockCollision)
                {
                    blockCollision = true;
                    transform.parent.GetComponent<SpriteRenderer>().sprite = pressSprite;
                    PressEvent.Invoke();
                }
            }

            else if (blockCollision)
            {
                blockCollision = false;
                transform.parent.GetComponent<SpriteRenderer>().sprite = releaseSprite;
                ReleaseEvent.Invoke();
            }
        }
    }
}
