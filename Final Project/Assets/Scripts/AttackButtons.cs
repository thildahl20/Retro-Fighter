using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButtons : MonoBehaviour
{
    public Punch punch;
    public SpearSprite spear;
    public Mace mace;
    public GrenadeSprite grenade;
    public BowSprite bow;
    public BoomerangSprite boomerang;
    public GunSprite gun;

    public void AttackOne()
    {
        punch.gameObject.SetActive(true);
    }

    public void AttackTwo(Character thrower)
    {
        if (mace.gameObject.activeInHierarchy)
        {
            mace.Swing();
        }
        else if (grenade.gameObject.activeInHierarchy)
        {
            grenade.Throw(thrower);
        }
        else if (bow.gameObject.activeInHierarchy)
        {
            bow.Shoot(thrower);
        }
        else if (boomerang.gameObject.activeInHierarchy)
        {
            boomerang.Throw(thrower);
        }
        else if (spear.gameObject.activeInHierarchy)
        {
            spear.Throw(thrower);
        }
        else if (gun.gameObject.activeInHierarchy)
        {
            gun.Shoot(thrower);
        }
    }

}
