using UnityEngine;

public class FireExtinguisherUse : MonoBehaviour
{
    public ParticleSystem sprayParticle;
    public AudioSource sprayAudioSource;  
    public float extinguishPower = 15f;   // How fast fire dies
    public float extinguishRange = 6f;    // Distance to affect fire
    public LayerMask ignoreLayers;        // Layers to ignore (set to Player layer)

    private bool isHeld = false;
    private int raycastLayerMask;

    void Start()
    {
        // If ignoreLayers is set, create inverse mask. Otherwise hit everything
        if (ignoreLayers.value == 0)
        {
            raycastLayerMask = Physics.DefaultRaycastLayers;
            Debug.Log("🧯 No ignore layers set - using default raycast layers");
        }
        else
        {
            raycastLayerMask = ~ignoreLayers;
            Debug.Log($"🧯 Ignore layers mask: {ignoreLayers.value} | Raycast will use mask: {raycastLayerMask}");
        }
    }

    void Update()
    {
        if (!isHeld) return;

        bool spraying = Input.GetMouseButton(0) || Input.GetKey(KeyCode.H);

        if (spraying)
        {
            if (!sprayParticle.isPlaying)
            {
                sprayParticle.Play();
                
                // Play spray audio
                if (sprayAudioSource != null && !sprayAudioSource.isPlaying)
                {
                    sprayAudioSource.Play();
                }
                
                Debug.Log("🧯 Extinguisher spray started");
            }

            RaycastHit hit;

            // Raycast forward from camera, ignoring specified layers
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                out hit, extinguishRange, raycastLayerMask))
            {
                Debug.Log($"🎯 Extinguisher raycast hit: {hit.collider.name} (Tag: {hit.collider.tag}) on layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                
                // Try to get FireBehaviour from the hit object or its parent
                FireBehaviour fire = hit.collider.GetComponent<FireBehaviour>();
                if (fire == null)
                {
                    fire = hit.collider.GetComponentInParent<FireBehaviour>();
                }

                if (fire != null)
                {
                    Debug.Log($"💨 Attempting to damage fire '{fire.name}' with {extinguishPower * Time.deltaTime:F3} damage");
                    fire.TakeDamage(extinguishPower * Time.deltaTime);
                    Debug.Log($"💨 After damage - Fire '{fire.name}' | Health: {fire.fireHealth:F1}/{fire.maxHealth}");
                }
                else
                {
                    Debug.LogWarning($"⚠️ Hit {hit.collider.name} but no FireBehaviour component found on object or parent!");
                }
            }
            else
            {
                Debug.Log("🎯 Raycast hit nothing");
            }
        }
        else
        {
            if (sprayParticle.isPlaying)
                sprayParticle.Stop();
                
            // Stop spray audio
            if (sprayAudioSource != null && sprayAudioSource.isPlaying)
            {
                sprayAudioSource.Stop();
            }
        }
    }

    public void SetHeld(bool held)
    {
        isHeld = held;
        if (!held) sprayParticle.Stop();
    }
}
