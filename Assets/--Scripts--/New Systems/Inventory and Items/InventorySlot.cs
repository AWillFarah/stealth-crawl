using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{

    [HideInInspector] public ItemSO thisItem;
    Sprite sprite;
    public Image image;
    TMP_Text text;
    [HideInInspector] public int slotNum; //This will tell us which inventory slot we are

    void Awake()
    {
       text = GetComponentInChildren<TMP_Text>();
    }

    public void AddItemToInventory(ItemSO item)
    { 
        if(item == null) return;
        thisItem = item;
        image.sprite = thisItem.sprite;
        text.text = thisItem.itemName;
    }
    
    public void UseItem()
    {
        if (thisItem != null)
        {
            GameObject newItem = Instantiate(thisItem.itemPrefab, transform.position, Quaternion.identity);
            newItem.GetComponent<TemplateItem>().Use();
            thisItem = null;
            image.sprite = null;
            text.text = "";
            InventoryManager.INVENTORY.RemoveAt(slotNum);
        }
    }

}
