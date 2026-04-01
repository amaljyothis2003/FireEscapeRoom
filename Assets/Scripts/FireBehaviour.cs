using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    public float fireHealth = 100f;
    public float maxHealth = 100f;
    public ParticleSystem fireParticles;
    public ParticleSystem smokeParticles;
    public bool isOut = false;
    public bool canSpread = true;
    public float spreadRadius = 3f;

    private void Start()
    {
        maxHealth = fireHealth;
        fireParticles.Play();
        smokeParticles.Stop();
        
        Debug.Log($"🔥 Fire '{name}' started with {fireHealth} health");

        FireController.Instance.RegisterFire(this);
        
        if (FireSpreadManager.Instance != null)
            FireSpreadManager.Instance.RegisterFire(this);
    }
    
    public bool IsBurning()
    {
        return !isOut && fireHealth > 0;
    }

    public void TakeDamage(float amount)
    {
        if (isOut)
        {
            Debug.Log($"❌ Fire '{name}' already extinguished, ignoring damage");
            return;
        }

        fireHealth -= amount;
        Debug.Log($"🔥 Fire '{name}' took {amount:F2} damage | Health: {fireHealth:F1}/{maxHealth}");

        if (fireHealth <= 0)
        {
            ExtinguishFire();
        }
    }

    void ExtinguishFire()
    {
        isOut = true;
        Debug.Log($"✅ Fire '{name}' EXTINGUISHED!");

        fireParticles.Stop();
        smokeParticles.Play();

        FireController.Instance.RemoveFire(this);
        
        // Notify game manager
        if (GameManager.Instance != null)
            GameManager.Instance.OnFireExtinguished();

        Destroy(gameObject, 5f);
    }

    public void Ignite()
    {
        isOut = false;
        fireHealth = maxHealth;
        fireParticles.Play();
        smokeParticles.Stop();
        
        Debug.Log($"🔥 Fire '{name}' IGNITED with {fireHealth} health");
        
        FireController.Instance.RegisterFire(this);
        
        // Notify game manager
        if (GameManager.Instance != null)
            GameManager.Instance.OnFireReignited();
    }
    
    private void OnDestroy()
    {
        if (FireController.Instance != null)
            FireController.Instance.RemoveFire(this);
            
        if (FireSpreadManager.Instance != null)
            FireSpreadManager.Instance.RemoveFire(this);
    }
}
