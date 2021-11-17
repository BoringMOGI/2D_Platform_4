using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

public class ItemManager : MonoBehaviour
{
    // ������ ���� (Degisn Pattern)
    //  - �̱��� ���� (Singleton)
    static ItemManager instance;
    public static ItemManager Instance => instance;

    [SerializeField] private ItemObject itemPrefab;
    [SerializeField] private Item[] items;
    
    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("CSV/LoadItemData")]
    public void LoadItemData()
    {
        CSVData data = CSV.ReadCSV("ItemData");

        items = new Item[data.Count];           // csv�������� ������ŭ �迭 �Ҵ�.
        for (int i = 0; i < data.Count; i++)    // ������ŭ �ݺ��ϸ鼭
            items[i] = new Item(data, i);       // �� item������ ������ �Ҵ�.
    }

    // ItemObject�� ��û�ϴ� �Լ�.
    public ItemObject GetItemObject(Item.ITEMTYPE type)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemType == type)
            {
                ItemObject newItem = Instantiate(itemPrefab);       // ������ Ŭ�� ����.
                newItem.Setup(items[i]);                            // items[i]�� ������ ����. �ʱ�ȭ.

                return newItem;                                     // �ش� ������ ������Ʈ ����.
            }
        }

        return null;
    }

}
