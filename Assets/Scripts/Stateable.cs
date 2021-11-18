using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stateable : MonoBehaviour
{
    public float hp;
    public float power;
    public float MAX_HP;

    public bool IsAlive => (hp > 0);
}
