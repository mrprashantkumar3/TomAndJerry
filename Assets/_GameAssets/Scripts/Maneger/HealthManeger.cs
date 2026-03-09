using System;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManeger : MonoBehaviour
{
    
    public static HealthManeger Instance { get; private set; }
    public event Action OnPlayerDeath;
    [SerializeField] PlayerHealthUI playerHealthUI;
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void Damage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount; 
            playerHealthUI.AnimateDamage();

            if(currentHealth <= 0)
            {
               OnPlayerDeath?.Invoke();
               // GameManeger.Instance.PlayeGameOver();// playerdead
            }
        }
       
    }
    public void Health(int healthAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthAmount; 
            playerHealthUI.AnimateHealth();

            if(currentHealth == maxHealth)
            {
                Debug.Log("Player Health is max");
                
               //OnPlayerDeath?.Invoke();
               //GameManeger.Instance.PlayeGameOver();// playerdead
            }
        }
       
    }
    public void Heal(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
          //currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
           playerHealthUI.AnimateHeal();
            if(currentHealth <= 0)
            {
               //OnPlayerDeath?.Invoke();
               // GameManeger.Instance.PlayeGameOver();// playerdead
            }
        }
    }

}
