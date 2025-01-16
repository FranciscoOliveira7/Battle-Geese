using UnityEngine;

public class ItemParticle : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _hasBounced = false;
    public GameObject newItemPrefab; // Assign this in the Unity Editor


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the BALL hits theground
        if (!_hasBounced)
        {
            _hasBounced = true;
            // it goes up
            _rb.velocity = Vector3.up * 4;
            Debug.Log("Current Velocity: " + _rb.velocity);

             
        }
        if (_rb.velocity == Vector3.zero)
        {
            
            SpawnItem();
        }
    }

   private void SpawnItem()
    {
        // Remove the current mesh or GameObject
        Destroy(gameObject);

        // Instantiate the new prefab at the same position and rotation
        Instantiate(newItemPrefab, transform.position, transform.rotation);
    }
}