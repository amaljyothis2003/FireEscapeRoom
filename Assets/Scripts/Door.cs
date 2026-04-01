using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;   // + for right door, - for left door
    public float speed = 3f;
    public float autoCloseDelay = 5f;

    private Quaternion closedRot;
    private Quaternion openRot;
    private float openTime = 0f;

    void Start()
    {
        closedRot = transform.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            openTime = Time.time; // remember when it opened
        }
    }

    void Update()
    {
        // Smooth rotation
        if (isOpen)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, openRot, Time.deltaTime * speed);

            // Auto-close after delay
            if (Time.time > openTime + autoCloseDelay)
            {
                isOpen = false;
            }
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, closedRot, Time.deltaTime * speed);
        }
    }
}
