using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private UnityEvent OnHealthChange;
    [SerializeField] private UnityEvent OnDie;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private SFXObject hurtClip;
    [SerializeField] private SFXObject deathClip;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Gradient healthGradient;

    [SerializeField] private RectTransform healthbar;
    [SerializeField] private Image healthImage;

    private float health;

    private void Awake()
    {
        health = maxHealth;
        OnHealthChange.Invoke();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();

        OnHealthChange.Invoke();
    }

    public void Heal(float heal)
    {
        health += heal;

        if (health > maxHealth)
            health = maxHealth;

        OnHealthChange.Invoke();
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;

        if (health <= 0)
            Die();
        if (health > maxHealth)
            maxHealth = health;

        OnHealthChange.Invoke();
    }

    public bool HealthIsFull()
    {
        return health == maxHealth;
    }

    private void Die()
    {
        OnDie.Invoke();
        Destroy(gameObject);
    }

    public void CreateParticles()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

    public string GetDisplay()
    {
        return $"{Mathf.RoundToInt(health)}/{maxHealth}";
    }

    public void UpdateUI()
    {
        spriteRenderer.color = healthGradient.Evaluate(health / maxHealth);
    }

    public void UpdateHealthBar()
    {
        healthbar.localScale = new Vector3(health / maxHealth, 1, 1);
        healthImage.color = healthGradient.Evaluate(health / maxHealth);
    }

    public void UpdateVignette()
    {
        if (health / maxHealth >= 0.2f) return;

        Vignette vignette;
        if (FindObjectOfType<Volume>().profile.TryGet(out vignette))
        {
            vignette.active = true;
        }
    }
}
