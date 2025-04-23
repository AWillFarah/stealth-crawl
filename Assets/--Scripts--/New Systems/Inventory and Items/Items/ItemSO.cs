using UnityEngine;

public enum SlotTag {None, Body}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
   public string itemName;
   public Sprite sprite;
   public SlotTag slotTag;
   
   [Header("If the item can be equipped")]
   public GameObject equipmentPrefab;
}
