using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMerge : MonoBehaviour
{
    public Collider object1;

    public Collider object2;

    void Start()
    {
        Physics.IgnoreCollision(object1, object2);
    }
}
