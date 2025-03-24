using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BirdDatabase", menuName = "Birdwatching/BirdDatabase")]
public class BirdDatabase : ScriptableObject
{
    public List<BirdData> birds = new List<BirdData>();

    public void UnlockBird(string birdName)
    {
        BirdData bird = birds.Find(b => b.birdName == birdName);
        if (bird != null && !bird.isUnlocked)
        {
            bird.isUnlocked = true;
            Debug.Log($"Unlocked {birdName}!");
        }
    }

    public void ResetProgress()
    {
        foreach (BirdData bird in birds)
        {
            bird.isUnlocked = false;
        }
    }
}
