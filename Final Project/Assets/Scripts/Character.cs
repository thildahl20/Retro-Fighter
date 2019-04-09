using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float maxSpeed = 10f;
    public int maxHealth;
    [HideInInspector] public int health;

    public Image healthBar;

    public Text healthText;

    [HideInInspector] public bool facingRight = true;
}

