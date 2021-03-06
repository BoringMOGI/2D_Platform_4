using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ITEMTYPE
    {
        Meat,               // ?Ϲ? ????
        OldMeat,            // ???? ????
        AnimalBone,         // ???? ??
        Leather,            // ????
        Toenail,            // ????
    }

    public string itemName;
    public string itemContent;
    public Sprite itemSprite;
    public ITEMTYPE itemType;

    public Item(string itemName, string itemContent, Sprite itemSprite, ITEMTYPE itemType)
    {
        this.itemName = itemName;
        this.itemContent = itemContent;
        this.itemSprite = itemSprite;
        this.itemType = itemType;
    }
    public Item(DataFormat.CSVData data, int index)
    {
        itemName = data.Get(index, "name");
        itemContent = data.Get(index, "content");

        string type = data.Get(index, "type");

        // string.Trim() : ???? ????.
        string spritePath = string.Concat("Sprite/Item/", type).Trim();

        itemType = (ITEMTYPE)System.Enum.Parse(typeof(ITEMTYPE), type);
        itemSprite = Resources.Load<Sprite>(spritePath);
    }
}
