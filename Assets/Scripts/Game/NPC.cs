using UnityEngine;

public class NPC : MonoBehaviour
{
    private bool isInideInteractCollider;
    public BoxCollider interactCollider;

    private void OnTriggerEnter(Collider other)
    {
        isInideInteractCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInideInteractCollider = false;
    }

    private void Update()
    {
        if (isInideInteractCollider)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OnInteract();

                
            }
        }
    }

    public virtual void OnInteract()
    {
        Debug.Log("Interacted on: " + gameObject.name);
    }
}
