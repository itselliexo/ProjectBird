using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bird : MonoBehaviour
{
 protected virtual void Fly()
    {
        Debug.Log("Is Flying");
    }
}
