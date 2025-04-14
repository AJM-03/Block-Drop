using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class BlockSelector : MonoBehaviour
{
    public static BlockSelector instance;

    public Puzzle currentPuzzle;
    public Puzzle[] puzzles;

    public List<Block> availableBlocks = new List<Block>();
    public List<GameObject> HUDOptions = new List<GameObject>();
    public List<GameObject> droppedBlocks = new List<GameObject>();

    private int selectionIndex;

    public GameObject puzzleHUD;
    public GameObject blockSelectPrompt, selectionPrompt;
    private Animation HUDAnim;
    private PlayerMovement movement;

    void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;

        HUDAnim = puzzleHUD.GetComponent<Animation>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (SaveManager.instance != null)
            LoadPuzzle(SaveManager.instance.LoadGame());
    }


    private void Update()
    {
        selectionPrompt.transform.position = new Vector3(puzzleHUD.transform.position.x, selectionPrompt.transform.position.y, 0);

        /*foreach (GameObject block in HUDOptions)
        {
            if (block != null)
                block.GetComponent<RectTransform>().localScale = Vector3.one;
        }*/
    }


    private void OnMovement(InputValue value)
    {
        if (movement.isSelecting)
        {
            float input = value.Get<Vector2>().y;

            if (input > 0)
            {
                if (selectionIndex == 0) selectionIndex = availableBlocks.Count - 1;
                else selectionIndex--;
                GetComponent<BlockController>().ChangeGhostBlock(availableBlocks[selectionIndex]);
                blockSelectPrompt.transform.position = new Vector3(blockSelectPrompt.transform.position.x, HUDOptions[selectionIndex].transform.position.y, 0);
            }

            else if (input < 0)
            {
                if (selectionIndex == availableBlocks.Count - 1) selectionIndex = 0;
                else selectionIndex++;
                GetComponent<BlockController>().ChangeGhostBlock(availableBlocks[selectionIndex]);
                if (HUDOptions[selectionIndex] != null)
                    blockSelectPrompt.transform.position = new Vector3(blockSelectPrompt.transform.position.x, HUDOptions[selectionIndex].transform.position.y, 0);
            }
        }
    }


    private void OnJump(InputValue value)
    {
        if (movement.isSelecting)
        {
            GetComponent<BlockController>().DropBlock(availableBlocks[selectionIndex]);
            movement.isSelecting = false;
            movement.isControlling = true;
            Color c = puzzleHUD.GetComponent<Image>().color;
            c.a = 0.5f;
            puzzleHUD.GetComponent<Image>().color = c;
            blockSelectPrompt.SetActive(false);
            availableBlocks.Remove(availableBlocks[selectionIndex]);
            HUDOptions = new List<GameObject>();
            foreach (Transform child in puzzleHUD.transform)
                Destroy(child.gameObject);
            foreach (Block block in availableBlocks)
            {
                if (block != null)
                {
                    GameObject newBlock = Instantiate(block.HUD);
                    newBlock.transform.parent = puzzleHUD.transform;
                    HUDOptions.Add(newBlock);
                }
            }
            selectionIndex = 0;
        }
    }


    private void OnSelector()
    {
        if (currentPuzzle != null && GetComponent<BlockController>().currentBlock == null && movement.isGrounded)
        {
            Color c = puzzleHUD.GetComponent<Image>().color;

            if (!movement.isSelecting && HUDOptions.Count != 0)
            {
                selectionIndex = 0;
                movement.isSelecting = true;
                movement.facingRight = false;
                GetComponent<SpriteRenderer>().flipX = true;
                c.a = 1f;
                GetComponent<BlockController>().ChangeGhostBlock(availableBlocks[selectionIndex]);
                blockSelectPrompt.SetActive(true);
                foreach (GameObject t in HUDOptions)
                {
                    if (t != null)
                    {
                        blockSelectPrompt.transform.position = new Vector3(blockSelectPrompt.transform.position.x, t.transform.position.y, 0);
                        break;
                    }
                }
            }

            else if (movement.isSelecting)
            {
                movement.isSelecting = false;
                c.a = 0.5f;
                GetComponent<BlockController>().ChangeGhostBlock(null);
                blockSelectPrompt.SetActive(false);
            }

            puzzleHUD.GetComponent<Image>().color = c;
        }

        else if (currentPuzzle != null && GetComponent<BlockController>().currentBlock != null)
        {
            movement.isControlling = !movement.isControlling;
        }
    }


    private void OnReset()
    {
        if (currentPuzzle != null)
        {
            ResetPuzzle();
        }
    }


    public void StartPuzzle(Puzzle newPuzzle)
    {
        currentPuzzle = newPuzzle;
        availableBlocks = new List<Block>(currentPuzzle.blocks);
        droppedBlocks = new List<GameObject>();
        HUDOptions = new List<GameObject>();
        selectionIndex = 0;

        foreach (Transform child in puzzleHUD.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (Block block in availableBlocks)
        {
            GameObject newBlock = Instantiate(block.HUD);
            newBlock.transform.parent = puzzleHUD.transform;
            HUDOptions.Add(newBlock);
        }

        HUDAnim.Play("HUDIn");

        for (int i = 0; i < puzzles.Length; i++)
        {
            if (puzzles[i] == currentPuzzle)
            {
                SaveManager.instance.SaveGame(i);
                break;
            }
        }
    }

    public void EndPuzzle()
    {
        foreach (GameObject block in droppedBlocks)
        {
            Destroy(block);
        }

        currentPuzzle = null;
        HUDAnim.Play("HUDOut");
        GetComponent<PlayerMovement>().isSelecting = false;
        GetComponent<PlayerMovement>().isControlling = false;
    }

    public void ResetPuzzle()
    {
        foreach (GameObject block in droppedBlocks)
        {
            Destroy(block);
        }

        transform.position = currentPuzzle.startPoint.position;
        GetComponent<PlayerMovement>().inGround = true;
        GetComponent<PlayerMovement>().isSelecting = false;
        GetComponent<PlayerMovement>().isControlling = false;
        GetComponent<BlockController>().ghost = null;


        StartPuzzle(currentPuzzle);
    }

    private void LoadPuzzle(int puzzleNum)
    {
        if (puzzleNum != 0)
        {
            currentPuzzle = puzzles[puzzleNum];
            SaveManager.instance.loading = false;
            ResetPuzzle();
        }
    }
}
