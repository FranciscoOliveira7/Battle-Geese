using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum ItemType
{
    weapon,
    hat,
    consumable
}

public class ItemPickup : MonoBehaviour, IInteractable
{
    private GameObject _eToInteractUI;
    public GameObject fToInteract; // Reference to the prefab

    [SerializeField] public ScriptableObject _item;
    [SerializeField] public ItemType _itemType;

    private void Start()
    {
        if (fToInteract == null)
        {
            Debug.LogError("eToInteractPrefab is not assigned in the Inspector.");
            return;
        }

        // Instantiate the prefab half a unit higher and -1z from the object and set it to inactive
        _eToInteractUI = Instantiate(fToInteract, transform.position + Vector3.up * 0.8f + Vector3.back * 0.3f, Quaternion.identity, transform);
        _eToInteractUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_eToInteractUI != null)
            {
                _eToInteractUI.SetActive(true);
                UpdateUIPosition();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_eToInteractUI != null)
            {
                _eToInteractUI.SetActive(false);
            }
        }
    }

    private void UpdateUIPosition()
    {
        // Set the position of the UI element in world space, rotate it 45 degrees on the x-axis, and move it 1 unit back
        _eToInteractUI.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.8f - Vector3.back * 0.3f, Quaternion.Euler(45, 0, 0));
    }

    private void PickUpItem()
    {
        Player player = MainManager.Instance.myPlayer;

        switch (_itemType)
        {
            case ItemType.weapon:
                SwapWeapon(player);
                break;
            case ItemType.hat:
                player.PickUpHat(_item as HatSO);
                break;
            case ItemType.consumable:
                player.PickUpConsumable(_item as ConsumableDataSO);
                // Destroy the old item on the ground
                if (PhotonNetwork.IsConnectedAndReady)
                {
                    PhotonView.Get(this).RPC("KMS", RpcTarget.AllBuffered);
                }
                else Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void SwapWeapon(Player player)
    {
        // Pick up the new weapon
        WeaponDataSO newWeaponData = _item as WeaponDataSO;

        // Get the current weapon the player is holding
        WeaponDataSO currentWeaponData = player.Weapons[player.EquipedWeapon].Data;
        if (currentWeaponData != null)
        {
            GameObject droppedWeapon;
            if (PhotonNetwork.IsConnectedAndReady)
            {
                droppedWeapon = Server.Instance.InstantiateGameObject("WeaponItems/" + currentWeaponData.WeaponPrefab.name, transform.position, Quaternion.identity);
            }
            // Replace the particle with the actual item
            else droppedWeapon = Instantiate(currentWeaponData.WeaponPrefab, transform.position, Quaternion.identity);

            ItemPickup itemPickup = droppedWeapon.GetComponent<ItemPickup>();
            itemPickup._item = currentWeaponData;
            itemPickup._itemType = ItemType.weapon;

            // Ensure the UI element is properly managed
            if (itemPickup._eToInteractUI != null)
            {
                Destroy(itemPickup._eToInteractUI);
                itemPickup._eToInteractUI = Instantiate(fToInteract, droppedWeapon.transform.position + Vector3.up * 0.02f + Vector3.back * 1f, Quaternion.identity, droppedWeapon.transform);
                itemPickup._eToInteractUI.SetActive(false);
            }
        }

        // Pick up the new weapon
        player.PickUpWeapon(newWeaponData);

        // Destroy the old item on the ground
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonView.Get(this).RPC("KMS", RpcTarget.AllBuffered);
        }
        else Destroy(gameObject);
    }

    [PunRPC]
    public void KMS()
    {
        if (gameObject == null) return;
        if (PhotonView.Get(this).IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Interact()
    {
        PickUpItem();
    }
}

