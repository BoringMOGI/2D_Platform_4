using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] Text playerText;      // �÷��̾� �̸�.
    [SerializeField] Text hpText;          // ü�� �ؽ�Ʈ.
    [SerializeField] Image hpImage;        // ü�� �̹���.

    void Start()
    {
        SetPlayerName("�׽���00");
    }

    public void SetPlayerName(string name)
    {
        playerText.text = name;
    }
    public void SetHp(float current, float max)
    {
        hpImage.fillAmount = current / max;
        hpText.text = string.Format("{0}/{1}", current, max);
    }

}
