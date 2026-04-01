using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public bool isPickedUp = false;

    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        if (isPickedUp) return;

        isPickedUp = true;

        // Turn off physics
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;

        // Attach to hand
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

        // Apply your rotation
        transform.localRotation = Quaternion.Euler(-79.4f, 0f, 0f);
    }

    public void Drop()
    {
        if (!isPickedUp) return;

        isPickedUp = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
    }
}
