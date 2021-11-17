using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

public class ItemManager : MonoBehaviour
{
    // 디자인 패턴 (Degisn Pattern)
    //  - 싱글톤 패턴 (Singleton)
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

        items = new Item[data.Count];           // csv데이터의 개수만큼 배열 할당.
        for (int i = 0; i < data.Count; i++)    // 개수만큼 반복하면서
            items[i] = new Item(data, i);       // 각 item변수에 데이터 할당.
    }

    // ItemObject를 요청하는 함수.
    public ItemObject GetItemObject(Item.ITEMTYPE type)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemType == type)
            {
                ItemObject newItem = Instantiate(itemPrefab);       // 프리팹 클론 생성.
                newItem.Setup(items[i]);                            // items[i]의 데이터 전달. 초기화.

                return newItem;                                     // 해당 아이템 오브젝트 리턴.
            }
        }

        return null;
    }

}
