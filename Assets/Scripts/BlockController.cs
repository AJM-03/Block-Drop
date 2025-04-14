using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BlockController : MonoBehaviour
{
    public GameObject currentBlock;
    private Rigidbody2D currentBlockRB;
    private Block currentBlockType;
    public float maxFallSpeed;
    public float maxQuickDropSpeed;
    private bool quickDrop;
    public int dropHeight;

    public float settleTime;
    private float settleTimer;

    public GameObject ghost;
    private Block ghostType;
    private PlayerMovement movement;


    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        if (currentBlock != null)
        {
            if (quickDrop)
                currentBlockRB.velocity = new Vector2(currentBlockRB.velocity.x, maxQuickDropSpeed);
            else
                currentBlockRB.velocity = new Vector2(currentBlockRB.velocity.x, maxFallSpeed);

            ContactFilter2D filter = new ContactFilter2D();
            RaycastHit2D[] results = new RaycastHit2D[10];
            float distance = 0.05f;
            int cast = currentBlockRB.Cast(-transform.up, filter, results, distance);

            for (int i = 0; i < cast; i++)
            {
                if (results[i].transform.gameObject.tag == "Ground" || results[i].transform.gameObject.tag == "Block")
                {
                    settleTimer -= Time.deltaTime;

                    if (settleTimer <= 0)
                    {
                        currentBlock = null;
                        movement.isControlling = false;
                        quickDrop = false;
                    }
                }

                if (results[i].transform.gameObject.tag == "Player")
                {
                    GetComponent<BlockSelector>().ResetPuzzle();
                }
            }


            if (ghost != null)
            {
                ghost.transform.position = GetDropPosition(ghostType);
            }
        }
    }


    public void DropBlock(Block block)
    {
        currentBlockType = ghostType;
        GameObject newBlock = Instantiate(block.block, GetDropPosition(currentBlockType), Quaternion.identity);
        currentBlock = newBlock;
        currentBlockRB = newBlock.GetComponent<Rigidbody2D>();
        GetComponent<BlockSelector>().droppedBlocks.Add(currentBlock);

        if (ghost != null)
            Destroy(ghost);
    }

    public void ChangeGhostBlock(Block block)
    {
        if (ghost != null)
        {
            Destroy(ghost);
        }

        if (block != null)
        {
            GameObject newGhost = Instantiate(block.ghost, transform.position, Quaternion.identity);
            ghost = newGhost;
            ghostType = block;
            ghost.transform.position = GetDropPosition(ghostType);
        }
    }


    private void OnMovement(InputValue value)
    {
        if (movement.isControlling && currentBlock != null)
        {
            if (value.Get<Vector2>().x > 0)
            {
                bool canMove = true;
                ContactFilter2D filter = new ContactFilter2D();
                RaycastHit2D[] results = new RaycastHit2D[10];
                float distance = 1f;
                int cast = currentBlockRB.Cast(transform.right, filter, results, distance);

                for (int i = 0; i < cast; i++)
                {
                    if (results[i].transform.gameObject.tag == "Ground" || results[i].transform.gameObject.tag == "Player" || 
                        results[i].transform.gameObject.tag == "Block")
                        canMove = false;
                }

                if (canMove)
                {
                    currentBlock.transform.Translate(1f, 0, 0);
                    settleTimer = settleTime;
                }
            }

            else if (value.Get<Vector2>().x < 0)
            {
                bool canMove = true;
                ContactFilter2D filter = new ContactFilter2D();
                RaycastHit2D[] results = new RaycastHit2D[10];
                float distance = 1f;
                int cast = currentBlockRB.Cast(-transform.right, filter, results, distance);

                for (int i = 0; i < cast; i++)
                {
                    if (results[i].transform.gameObject.tag == "Ground" || results[i].transform.gameObject.tag == "Player" || 
                        results[i].transform.gameObject.tag == "Block")
                        canMove = false;
                }

                if (canMove)
                {
                    currentBlock.transform.Translate(-1f, 0, 0);
                    settleTimer = settleTime;
                }
            }

            else if (value.Get<Vector2>().y < 0)
                quickDrop = true;

            else if (value.Get<Vector2>().y > 0)
                quickDrop = false;
        }
    }



    private void OnRotation(InputValue value)
    {
        if (movement.isControlling && currentBlock != null && value.Get<Vector2>().x != 0)
        {
            Vector3 offset = Vector3.zero;

            if (value.Get<Vector2>().x > 0)
                currentBlock.transform.GetChild(0).Rotate(new Vector3(0, 0, 90));

            else if (value.Get<Vector2>().x < 0)
                currentBlock.transform.GetChild(0).Rotate(new Vector3(0, 0, -90));

            // Check at same height

            if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
            {
                offset = new Vector3(-1, 0, 0);
                if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                {
                    offset = new Vector3(1, 0, 0);
                    if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                    {
                        offset = new Vector3(-2, 0, 0);
                        if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                        {
                            offset = new Vector3(2, 0, 0);
                            if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                            {

                                // Check below block

                                offset = new Vector3(0, 1, 0);
                                if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                {
                                    offset = new Vector3(-1, -1, 0);
                                    if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                    {
                                        offset = new Vector3(1, -1, 0);
                                        if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                        {
                                            offset = new Vector3(-2, -1, 0);
                                            if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                            {
                                                offset = new Vector3(2, -1, 0);
                                                if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                                {

                                                    // Check above block

                                                    offset = new Vector3(0, 1, 0);
                                                    if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                                    {
                                                        offset = new Vector3(-1, 1, 0);
                                                        if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                                        {
                                                            offset = new Vector3(1, 1, 0);
                                                            if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                                            {
                                                                offset = new Vector3(-2, 1, 0);
                                                                if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                                                {
                                                                    offset = new Vector3(2, 1, 0);
                                                                    if (CollisionCheck(currentBlock.transform.GetChild(0), offset))
                                                                    {
                                                                        offset = new Vector3(0, 0, 0);
                                                                        CollisionCheck(currentBlock.transform.GetChild(0), offset);
                                                                        if (value.Get<Vector2>().x > 0)
                                                                            currentBlock.transform.GetChild(0).Rotate(new Vector3(0, 0, -90));

                                                                        else if (value.Get<Vector2>().x < 0)
                                                                            currentBlock.transform.GetChild(0).Rotate(new Vector3(0, 0, 90));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            currentBlock.transform.Translate(offset);
            settleTimer = settleTime;
        }
    }


    private Vector3 GetDropPosition(Block blockType)
    {
        float playerPos = Mathf.RoundToInt(transform.position.x);
        playerPos += blockType.blockOffset.x;
        Vector3 offset = new Vector3(playerPos, Mathf.RoundToInt(transform.position.y) + dropHeight + blockType.blockOffset.y, 0);

        if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
        {
            offset = new Vector3(playerPos + -1, offset.y, 0);
            if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
            {
                offset = new Vector3(playerPos + 1, offset.y, 0);
                if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                {
                    offset = new Vector3(playerPos + -2, offset.y, 0);
                    if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                    {
                        offset = new Vector3(playerPos + 2, offset.y, 0);
                        if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                        {

                            offset = new Vector3(playerPos + -3, offset.y, 0);
                            if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                            {
                                offset = new Vector3(playerPos + 3, offset.y, 0);
                                if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                                {
                                    offset = new Vector3(playerPos + -4, offset.y, 0);
                                    if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                                    {
                                        offset = new Vector3(playerPos + 4, offset.y, 0);
                                        if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                                        {
                                            offset = new Vector3(playerPos + -5, offset.y, 0);
                                            if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                                            {
                                                offset = new Vector3(playerPos + 5, offset.y, 0);
                                                if (CollisionCheck(ghost.transform.GetChild(0), offset, true))
                                                {
                                                    return Vector3.zero;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return offset;
    }


    private bool CollisionCheck(Transform obj, Vector3 offset, bool drop = false)
    {
        foreach (Transform child in obj)
        {
            Vector2 point;
            if (!drop) point =  child.position + offset;
            else point = offset + child.localPosition;
            var colliders = Physics2D.OverlapPointAll(point);

            foreach (var collider in colliders)
            {
                if (collider.transform.gameObject.tag == "Ground" || collider.transform.gameObject.tag == "Player" || 
                    (collider.transform.gameObject.tag == "Block" && collider.transform != obj.parent))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
