using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bird : MonoBehaviour
{
    public BirdData birdData;

    public string GetBirdName()
    {
        return birdData != null ? birdData.birdName : "Unknown";
    }
 protected virtual void Fly()
    {
        Debug.Log("Is Flying");
    }
}
