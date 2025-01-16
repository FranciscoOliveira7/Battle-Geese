using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class RarityEffect
{
    public string RarityName;
    public GameObject ItemEffectPrefab; // New effect to be applied to the item
    public Color ParticleColor; // Color to be applied to the particle
}

[Serializable]
public class ItemSpawning
{
    public GameObject Item;
    public float spawnChance;
    public string Rarity; // Rarity of the item
}

public class ItemSpawn : MonoBehaviour
{
    private GameObject _particle;
    private Rigidbody _particleRb;
    private bool _buttonIsPressed = false;
    private bool _itemDropped = false;
    private Rigidbody Lid;

    [SerializeField] private Sprite closedChestSprite;
    [SerializeField] private Sprite openChestSprite;
    [SerializeField] private SpriteRenderer chestSpriteRenderer; // SpriteRenderer to be assigned in the inspector

    [SerializeField]
    private AudioClip _chestOpenSound;

    // List of item prefabs to be set in the inspector
    [SerializeField]
    private List<ItemSpawning> itemPrefabs;

    // List of rarity effects to be set in the inspector
    [SerializeField]
    private List<RarityEffect> rarityEffects;

    // Particle prefab to be set in the inspector
    [SerializeField]
    private GameObject particlePrefab;

    // Number of items to drop at the same time
    [SerializeField]
    private int numberOfItemsToDrop = 1;

    private void Awake()
    {
        // Find the lid of the chest
        Lid = transform.Find("Lid").GetComponent<Rigidbody>();
    }

    public void OnButtonPressed()
    {
        if (!_buttonIsPressed)
        {
            _buttonIsPressed = true;
            ActivateChest();
        }
    }

    private void ActivateChest()
    {
        OpenLid();
        if (!_itemDropped)
        {
            // Spawn multiple items from the list
            for (int i = 0; i < numberOfItemsToDrop; i++)
            {
                SpawnItemFromList();
            }
            _itemDropped = true;
        }
    }

    private void OpenLid()
    {
        // Change the chest sprite to the open chest sprite
        if (chestSpriteRenderer != null)
        {
            chestSpriteRenderer.sprite = openChestSprite;
        }

        // Activate lid gameobject
        Lid.gameObject.SetActive(true);

        SoundManager.instance.PlayClip(_chestOpenSound, transform, 0.2f);

        // Apply a random force within a cone
        float angle = UnityEngine.Random.Range(-15f, 15f);
        Vector3 forceDirection = Quaternion.Euler(0, angle, 0) * Vector3.up;
        Lid.AddForce(forceDirection * 7f, ForceMode.Impulse);

        // Apply random rotation to the lid
        Lid.AddTorque(Vector3.forward * 7f, ForceMode.Impulse);

        Destroy(Lid.gameObject, 2f);
    }

    private void SpawnItemFromList()
    {
        if (itemPrefabs.Count == 0) return;

        // Calculate the total spawn chance
        float totalSpawnChance = 0f;
        foreach (var item in itemPrefabs)
        {
            totalSpawnChance += item.spawnChance;
        }

        // Generate a random number between 0 and the total spawn chance
        float randomValue = UnityEngine.Random.Range(0f, totalSpawnChance);

        // Iterate through the list and spawn an item based on its normalized spawn chance
        float cumulativeChance = 0f;
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            var item = itemPrefabs[i];
            cumulativeChance += item.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                // Find the rarity effect associated with the item's rarity
                RarityEffect rarityEffect = rarityEffects.Find(effect => effect.RarityName == item.Rarity);
                if (rarityEffect != null)
                {
                    // Instantiate the shared particle prefab
                    GameObject particle = Instantiate(particlePrefab, new Vector3(transform.position.x, 0.63f, transform.position.z), Quaternion.identity);
                    particle.transform.SetParent(transform);

                    _particleRb = particle.GetComponent<Rigidbody>();

                    // Apply the SpawnItemParticle function to the particle
                    SpawnItemParticle(particle, rarityEffect);

                    // Start monitoring the Y position of the particle
                    StartCoroutine(MonitorYPosition(particle, item, rarityEffect));

                    // Remove the spawned item from the list
                    itemPrefabs.RemoveAt(i);

                    break; // Exit the loop after spawning an item
                }
                else
                {
                    Debug.LogError("RarityEffect not found for rarity: " + item.Rarity);
                }
            }
        }
    }

    private void SpawnItemParticle(GameObject particle, RarityEffect rarityEffect)
    {
        // Creates a Direction for the particle to jump towards
        Vector3 _particleDirection = new(2f, 5f, 0f);
        // Generate a Random angle for it
        float angle = UnityEngine.Random.Range(0, 360);
        _particleDirection = Quaternion.Euler(0, angle, 0) * _particleDirection;
        // Spawns the particle
        _particleRb.velocity = _particleDirection;

        // Apply the color to the particle
        Renderer particleRenderer = particle.GetComponent<Renderer>();
        if (particleRenderer != null)
        {
            particleRenderer.material.color = rarityEffect.ParticleColor;
        }

        // Find the VFX object within the particle
        _particle = particle.transform.Find("vfxGraph_ItemAura")?.gameObject; // Replace "vfxGraph_ItemAura" with the actual name of the VFX object
    }

    private System.Collections.IEnumerator MonitorYPosition(GameObject particle, ItemSpawning itemSpawning, RarityEffect rarityEffect)
    {
        while (particle.transform.position.y > 0.5f)
        {
            yield return null; // Wait for the next frame
        }

        // Disable gravity and stop the particle's movement
        _particleRb.useGravity = false;
        _particleRb.velocity = Vector3.zero;
        _particleRb.angularVelocity = Vector3.zero;

        // Destroy the old particle
        Destroy(particle);
        GameObject newItem;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (!PhotonNetwork.IsMasterClient) yield break;
            newItem = Server.Instance.InstantiateGameObject("WeaponItems/" + itemSpawning.Item.name, particle.transform.position, Quaternion.identity);
        }
        // Replace the particle with the actual item
        else newItem = Instantiate(itemSpawning.Item, particle.transform.position, Quaternion.identity);
        newItem.transform.SetParent(particle.transform.parent);

        // Apply the new item effect to the new item
        ApplyItemEffect(newItem, rarityEffect);

        // Configure the existing ItemPickup script on the new item
        ItemPickup itemPickup = newItem.GetComponent<ItemPickup>();
        if (itemPickup != null)
        {
            itemPickup._item = itemSpawning.Item.GetComponent<ItemPickup>()._item;
            itemPickup._itemType = itemSpawning.Item.GetComponent<ItemPickup>()._itemType;
        }

        // Disable gravity and stop the item's movement
        Rigidbody newItemRb = newItem.GetComponent<Rigidbody>();
        if (newItemRb != null)
        {
            newItemRb.useGravity = false;
            newItemRb.velocity = Vector3.zero;
            newItemRb.angularVelocity = Vector3.zero;
        }
    }

    private void ApplyItemEffect(GameObject item, RarityEffect rarityEffect)
    {
        if (rarityEffect.ItemEffectPrefab != null)
        {
            GameObject effectInstance = Instantiate(rarityEffect.ItemEffectPrefab, item.transform);
            effectInstance.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogError("ItemEffectPrefab is not set for rarity: " + rarityEffect.RarityName);
        }
    }

    // A Function to Choose an array of items to spawn as children of the chest
    public void SpawnItems(ItemSpawning[] items)
    {
        foreach (ItemSpawning item in items)
        {
            if (UnityEngine.Random.Range(0f, 100f) <= item.spawnChance)
            {
                GameObject spawnedItem = Instantiate(item.Item, transform.position, Quaternion.identity);
                spawnedItem.transform.SetParent(transform);
            }
        }
    }

    public bool IsButtonActive()
    {
        return _buttonIsPressed;
    }
}