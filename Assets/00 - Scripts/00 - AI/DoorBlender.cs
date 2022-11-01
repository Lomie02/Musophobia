using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorBlender : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            other.gameObject.GetComponent<AIModule>().CompressBlendshape();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            other.gameObject.GetComponent<AIModule>().CancelCompression();
        }
    }

}
