using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public float interactDistance = 3f;          // how close the player must be
    public LayerMask doorMask;                  // assign "Door" layer or leave at Default
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, doorMask))
        {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
    }
}
