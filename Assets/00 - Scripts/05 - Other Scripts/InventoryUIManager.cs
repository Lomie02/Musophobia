using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    // Start is called before the first frame update 

    public Text UIItemName;
    public ItemManager itemManager;
    
    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        itemManager.OnUpdatedSlot.AddListener(UpdateUI);
    }

    public void UpdateUI()
    {
        UIItemName.text = itemManager.GetCurrentItemName();
    }
}
