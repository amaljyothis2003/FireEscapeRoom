using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public static FireController Instance;

    public GameObject firePrefab;
    public float spreadInterval = 5f;
    public float spreadRadius = 3f;

    private float timer;
    private List<FireBehaviour> activeFires = new List<FireBehaviour>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterFire(FireBehaviour fire)
    {
        if (!activeFires.Contains(fire))
            activeFires.Add(fire);
    }

    public void RemoveFire(FireBehaviour fire)
    {
        if (activeFires.Contains(fire))
            activeFires.Remove(fire);
    }

    void Update()
    {
        if (activeFires.Count == 0) return;

        timer += Time.deltaTime;

        if (timer >= spreadInterval)
        {
            timer = 0f;
            TrySpreadFire();
        }
    }

    void TrySpreadFire()
    {
        // Clean up null references
        activeFires.RemoveAll(fire => fire == null);
        
        foreach (var fire in activeFires)
        {
            if (fire == null || fire.isOut) continue;

            // Random position within spread radius (only X and Z, keep Y same)
            Vector2 randomCircle = Random.insideUnitCircle * spreadRadius;
            Vector3 position = fire.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // Raycast downward to find ground
            RaycastHit hit;
            if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out hit, 5f))
            {
                position.y = hit.point.y + 0.1f; // Slightly above ground
            }
            else
            {
                position.y = fire.transform.position.y; // Same height as source fire
            }

            // Instantiate new fire at calculated position
            GameObject newFire = Instantiate(firePrefab, position, Quaternion.identity);
            Debug.Log($"🔥 Fire spread - new fire created at {position}");
        }
    }
}
