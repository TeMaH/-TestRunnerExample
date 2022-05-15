using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculation
{
    public static int CalculateDamage(float Damage)
    {
        return Mathf.FloorToInt(Damage);
    }
}
