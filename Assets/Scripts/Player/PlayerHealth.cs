using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        UpdateUI();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateUI()
    {
        healthBar.value = health;
        healthText.text = health.ToString();
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateUI();
        if (health <= 0) PassAway();
    }

    [NaughtyAttributes.Button]
    private void Damage()
    {
        TakeDamage(10);
    }


    private void PassAway()
    {
        Time.timeScale = 0f;
    }
} 
