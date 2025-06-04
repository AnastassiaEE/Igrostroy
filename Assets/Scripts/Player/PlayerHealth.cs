using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    private int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        if (health <= 0) PassAway();
    }

    private void PassAway()
    {
        Time.timeScale = 0f;
    }
} 
