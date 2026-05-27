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
    [SerializeField] private AudioClip hurtClip;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Gradient healthGradient;

    [SerializeField] private RectTransform healthbar;
    [SerializeField] private Image healthImage;

    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private bool useVignette = false;

    [SerializeField] private SpriteRenderer sp;

    private float health;
    private AudioSource audioSource;
    private Material hurtMaterial;
    private Material originalMaterial;

    private Vignette vignette;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
        OnHealthChange.Invoke();
        hurtMaterial = Resources.Load<Material>("HurtMaterial");

        if (sp == null)
            sp = spriteRenderer;

        originalMaterial = sp.material;

        if (useVignette)
            FindObjectOfType<Volume>().profile.TryGet(out vignette);
    }

    private void Update()
    {
        if (useVignette)
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, Time.deltaTime * 0.1f);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (audioSource != null && hurtClip != null && health > 0)
            audioSource.PlayOneShot(hurtClip);

        StopAllCoroutines();
        StartCoroutine(DamageFlashRoutine());

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
        if (health / maxHealth >= 0.1f) return;

        if (useVignette)
        {
            vignette.active = true;
            vignette.intensity.value = 0.4f;
        }
    }

    private IEnumerator DamageFlashRoutine()
    {
        sp.material = hurtMaterial;
        yield return new WaitForSeconds(flashTime);
        sp.material = originalMaterial;
    }
}
