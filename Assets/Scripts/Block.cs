using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Blocks")]
public class Block : ScriptableObject
{
    public GameObject block;
    public GameObject ghost;
    public GameObject HUD;
    public Vector2 blockOffset;
}
