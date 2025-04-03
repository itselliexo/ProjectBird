using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBird", menuName = "BirdWatching/BirdData")]
public class BirdData : ScriptableObject
{
    public string birdName;
    public string description;
    public Sprite birdImage;
    public bool isUnlocked = false;
}
