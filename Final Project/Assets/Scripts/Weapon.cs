using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected void Damage(GameObject hitObject, int damageDone)
    {
        if (hitObject.tag == "Player" || hitObject.tag == "Enemy") // ||| hitObject.tag == "Crate"
            hitObject.SendMessage("takeDamage", damageDone);
    }
}
