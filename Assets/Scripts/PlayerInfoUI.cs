using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] Text playerText;      // 플레이어 이름.
    [SerializeField] Text hpText;          // 체력 텍스트.
    [SerializeField] Image hpImage;        // 체력 이미지.

    void Start()
    {
        SetPlayerName("테스터00");
    }

    public void SetPlayerName(string name)
    {
        playerText.text = name;
    }

}
