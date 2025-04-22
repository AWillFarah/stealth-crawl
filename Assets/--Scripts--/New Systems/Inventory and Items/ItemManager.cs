using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum ItemType
{
    Health, Throwable, Equippable 
}
/// <summary>
/// This sets the Interface for items as well as keeps track of what items the player currently is holding
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static List<TemplateItem> INVENTORY;
    public static ItemManager S;
    void Start()
    {
        if(INVENTORY == null) INVENTORY = new List<TemplateItem>();
        if(S == null) S = this;
    } 
    
    // Update is called once per frame
    public void AddItem(TemplateItem item)
    {
        INVENTORY.Add(item);
    }
}
