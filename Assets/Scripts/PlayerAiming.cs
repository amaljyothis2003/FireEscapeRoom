using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [Header("References")]
    public Transform hand;          // Hand object (child of Player)
    public Transform mainCamera;    // Main Camera (child of Player)

    private Transform originalParent;
    private bool isAiming = false;

    void Start()
    {
        // Store original parent (Player object)
        originalParent = hand.parent;
    }

    void Update()
    {
        // RMB Hold → Aim
        if (Input.GetMouseButton(1) && !isAiming)
        {
            StartAiming();
        }

        // RMB Release → Stop Aim
        if (Input.GetMouseButtonUp(1) && isAiming)
        {
            StopAiming();
        }
    }

    void StartAiming()
    {
        isAiming = true;
        hand.SetParent(mainCamera, worldPositionStays: true);
    }

    void StopAiming()
    {
        isAiming = false;
        hand.SetParent(originalParent, worldPositionStays: true);
    }
}
