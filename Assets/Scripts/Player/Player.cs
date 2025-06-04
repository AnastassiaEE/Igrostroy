using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("References")]
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
       
    }

    void Update()
    {
  
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

}
