using UnityEngine;

public class ExtinguisherGas : MonoBehaviour
{
    public float extinguishPower = 10f; // damage to fire per second

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"💨 ExtinguisherGas trigger with: {other.name} (Tag: {other.tag})");
        
        if (other.CompareTag("Fire"))
        {
            FireBehaviour fire = other.GetComponent<FireBehaviour>();
            if (fire != null)
            {
                fire.TakeDamage(extinguishPower * Time.deltaTime);
                Debug.Log($"💨 Gas damaging fire '{fire.name}' | Health: {fire.fireHealth:F1}/{fire.maxHealth}");
            }
            else
            {
                Debug.LogWarning($"⚠️ Collider {other.name} has 'Fire' tag but no FireBehaviour component!");
            }
        }
    }
}
