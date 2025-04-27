using UnityEngine;

public enum SlotTag {None, Body}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
   public string itemName;
   
   public GameObject itemPrefab;
   
   [Header("Sprite")]
   public Sprite sprite;
   public Color spriteColor = Color.white;
   
   [Header("If the item can be equipped")]
   public GameObject equipmentPrefab;
}
