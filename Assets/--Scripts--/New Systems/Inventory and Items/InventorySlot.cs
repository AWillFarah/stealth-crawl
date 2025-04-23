using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{

    [HideInInspector] public ItemSO thisItem;
    Sprite sprite;
    public Image image;
    TMP_Text text;
    [HideInInspector] public int slotNum; //This will tell us which inventory 

    void Awake()
    {
       text = GetComponentInChildren<TMP_Text>();
    }

    public void AddItemToInventory(ItemSO item)
    { 
        thisItem = item;
        image.sprite = item.sprite;
        text.text = item.itemName;
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
