using UnityEngine;
using System.Collections.Generic;

public class FireSpreadManager : MonoBehaviour
{
    public static FireSpreadManager Instance;

    public float globalSpreadInterval = 5f;
    private float timer = 0f;

    private List<FireBehaviour> allFires = new List<FireBehaviour>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterFire(FireBehaviour fire)
    {
        if (!allFires.Contains(fire))
            allFires.Add(fire);
    }
    
    public void RemoveFire(FireBehaviour fire)
    {
        if (allFires.Contains(fire))
            allFires.Remove(fire);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= globalSpreadInterval)
        {
            timer = 0f;
            SpreadFires();
        }
    }

    void SpreadFires()
    {
        // Remove null entries (destroyed fires)
        allFires.RemoveAll(fire => fire == null);
        
        // Create a copy to avoid modification during iteration
        List<FireBehaviour> firesCopy = new List<FireBehaviour>(allFires);
        
        foreach (FireBehaviour fire in firesCopy)
        {
            if (fire != null && fire.IsBurning() && fire.canSpread)
            {
                TrySpreadFrom(fire);
            }
        }
    }

    void TrySpreadFrom(FireBehaviour sourceFire)
    {
        if (sourceFire == null) return;
        
        foreach (FireBehaviour targetFire in allFires)
        {
            if (targetFire == null || targetFire == sourceFire) continue;

            if (!targetFire.IsBurning())
            {
                float distance = Vector3.Distance(sourceFire.transform.position, targetFire.transform.position);

                if (distance <= sourceFire.spreadRadius)
                {
                    Debug.Log("🔥 Fire spread from " + sourceFire.name + " ➜ " + targetFire.name);

                    // Light the new fire
                    targetFire.Ignite();
                }
            }
        }
    }
}
