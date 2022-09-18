using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActive : MonoBehaviour
{
    
    void Update()
    {
       // Turn off the cursor -
       // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        // Turn on the cursor -
        Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;
    }

}
