using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private float power;

    private void Awake()
    {
        CSVData data = CSV.ReadCSV("ItemData");

        items = new Item[data.Count];           // csv데이터의 개수만큼 배열 할당.
        for (int i = 0; i < data.Count; i++)    // 개수만큼 반복하면서
            items[i] = new Item(data, i);       // 각 item변수에 데이터 할당.
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
