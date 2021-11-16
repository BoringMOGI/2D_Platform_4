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

        items = new Item[data.Count];           // csv�������� ������ŭ �迭 �Ҵ�.
        for (int i = 0; i < data.Count; i++)    // ������ŭ �ݺ��ϸ鼭
            items[i] = new Item(data, i);       // �� item������ ������ �Ҵ�.
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
