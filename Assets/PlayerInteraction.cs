using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Player player;
    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }

    }
}
