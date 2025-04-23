using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public enum ItemType
{
    Health, Throwable, Equippable 
}
/// <summary>
/// This sets the Interface for items as well as keeps track of what items the player currently is holding
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static List<ItemSO> INVENTORY;
    public List<ItemSO> debugList;
    
    public static InventoryManager S;
    
    [SerializeField] InventorySlot[] slots;
    void Awake()
    {
        if(INVENTORY == null) INVENTORY = new List<ItemSO>();
        
        // We need to make sure that our slots keep our items between floors
        
        else if (INVENTORY.Count > 0)
        {
            foreach (ItemSO item in INVENTORY)
            {
                AddItem(item);
            }
        } 
        
        if(S == null) S = this;
    } 
    
    // Update is called once per frame
    void Update()
    {
      debugList = INVENTORY;  
    }
    
    public void AddItem(ItemSO item)
    {
        int slotIndex = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.thisItem == null)
            {
                slot.AddItemToInventory(item);
                slot.slotNum = slotIndex;
                
                break;
            }
            slotIndex++;
        }
        
    }
}
