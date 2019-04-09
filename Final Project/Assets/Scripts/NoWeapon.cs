using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWeapon : WeaponSprite
{
    public void OnEnable()
    {
        uses = -1;
    }
}
