using UnityEngine;

public class RoomActivator : MonoBehaviour
{
    [SerializeField] private Door _door;

    private void OnTriggerExit(Collider other)
    {
        _door.StartRoomBattle();

        Destroy(gameObject);
    }
}
