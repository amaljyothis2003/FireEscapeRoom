using UnityEngine;

public class PickupInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public Transform hand;                     // Assign the "Hand" object
    public LayerMask pickupMask;               // Set to Pickup Layer

    private FireExtinguisher heldItem;

    void Update()
    {
        // Press E → pickup
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
                TryPickup();
        }

        // (Optional) Drop with G
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldItem != null)
                DropItem();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, pickupMask))
        {
            FireExtinguisher fe = hit.collider.GetComponent<FireExtinguisher>();

            if (fe != null)
            {
                heldItem = fe;

                // Pick it up normally
                fe.PickUp(hand);

                // Tell the use/shoot script it is being held
                FireExtinguisherUse useScript = fe.GetComponent<FireExtinguisherUse>();
                if (useScript != null)
                    useScript.SetHeld(true);
            }
        }
    }

    void DropItem()
    {
        // Drop item
        heldItem.Drop();

        // Stop spray script
        FireExtinguisherUse useScript = heldItem.GetComponent<FireExtinguisherUse>();
        if (useScript != null)
            useScript.SetHeld(false);

        heldItem = null;
    }
}
