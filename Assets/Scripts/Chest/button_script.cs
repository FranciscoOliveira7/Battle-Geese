using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    private ItemSpawn itemSpawn;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private AudioClip pressSound;
    [SerializeField] private float activationSoundStartTime;
    [SerializeField] private float activationSoundDuration;
    [SerializeField] private AudioClip deactivateSound;
    [SerializeField] private float deactivationSoundStartTime;
    [SerializeField] private float deactivationSoundDuration;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isPressed = false;

    private void Start()
    {
        // Find the ItemSpawn script in the parent object
        itemSpawn = GetComponentInParent<ItemSpawn>();
        // Find the SpriteRenderer component on the same object
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.4f;

        // Debug logs to check component assignment


        // Set the initial sprite to the active sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = activeSprite;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            ChangeSprite(inactiveSprite);
            PlaySound(pressSound, activationSoundStartTime, activationSoundDuration);
            if (itemSpawn != null)
            {
                itemSpawn.OnButtonPressed();
            }
        }
    }

    public void ChangeSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            Debug.Log("Changing sprite.");
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer is null.");
        }
    }

    public void ResetSprite()
    {
        isPressed = false;
        ChangeSprite(activeSprite);
        PlaySound(deactivateSound, deactivationSoundStartTime, deactivationSoundDuration);
    }

    private void PlaySound(AudioClip clip, float startTime, float duration)
    {
        if (audioSource != null && clip != null)
        {
            Debug.Log("Playing sound.");
            audioSource.clip = clip;
            audioSource.time = startTime;
            audioSource.Play();
            StartCoroutine(StopSoundAfterDuration(duration));
        }
        else
        {
            Debug.LogError("AudioSource or clip is null.");
        }
    }

    private System.Collections.IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}


