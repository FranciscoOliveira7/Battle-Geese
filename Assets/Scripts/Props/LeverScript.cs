using UnityEngine;

public class LeverScript : MonoBehaviour, IInteractable
{
    public GameObject obstacle; // The obstacle to animate
    public Sprite activeSprite; // The sprite to display when the lever is active
    public Sprite inactiveSprite; // The sprite to display when the lever is inactive
    private SpriteRenderer spriteRenderer;
    private Animator obstacleAnimator;
    public bool IsActivated { get; set; } = false;
    public ButtonDoorScript[] buttonDoorScripts; // References to the ButtonDoorScript components

    public AudioClip RockGrind; // The ticking sound to play every second



    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        obstacleAnimator = obstacle.GetComponent<Animator>();
    }

    private void ActivateLever()
    {
        spriteRenderer.sprite = activeSprite;
        IsActivated = true;
        obstacleAnimator.SetBool("IsOpen", true);
        PlayRockSlideSound();
        foreach (var buttonDoorScript in buttonDoorScripts)
        {
            buttonDoorScript.DeactivateScript();
        }
    }


    public void Interact()
    {
        //when  this function is called , check if the lever is activated or not and call the appropriate function
        if (!IsActivated)
        {
            ActivateLever();
        }

    }
    private void PlayRockSlideSound()
    {
        SoundManager.instance.PLayWithStartEnd(RockGrind, transform, 0f, 1f, 0.2f);
    }
}
