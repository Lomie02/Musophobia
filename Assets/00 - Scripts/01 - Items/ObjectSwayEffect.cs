using UnityEngine;
using System.Collections;

public class ObjectSwayEffect : MonoBehaviour
{
    float amount = 50f;
    float maxamount = 100f;
    float smooth = 3;
    private Quaternion def;
    private bool Paused = false;

    ItemManager m_ItemManager = null;
    private void Start()
    {
        m_ItemManager = FindObjectOfType<ItemManager>();
        def = transform.localRotation;
    }
    void Update()
    {
        float factorX = (Input.GetAxis("Mouse Y")) * amount;
        float factorY = -(Input.GetAxis("Mouse X")) * amount;
        float factorZ = -Input.GetAxis("Vertical") * amount;
        //float factorZ = 0 * amount;

        if (!Paused && !m_ItemManager.GetRotationState())
        {
            if (factorX > maxamount)
                factorX = maxamount;

            if (factorX < -maxamount)
                factorX = -maxamount;

            if (factorY > maxamount)
                factorY = maxamount;

            if (factorY < -maxamount)
                factorY = -maxamount;

            if (factorZ > maxamount)
                factorZ = maxamount;

            if (factorZ < -maxamount)
                factorZ = -maxamount;

            Quaternion Final = Quaternion.Euler(def.x + factorX, def.y + factorY, def.z + factorZ);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Final, (Time.deltaTime * smooth));
        }
    }
}