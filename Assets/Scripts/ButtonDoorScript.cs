using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ButtonDoorScript : MonoBehaviour
{
    public GameObject obstacle; // The obstacle to animate
    public TextMesh timerTextMesh; // The TextMesh object to display the timer
    public AudioClip tickingSound; // The ticking sound to play every second
    [SerializeField] private float tickingSoundStartTime;
    [SerializeField] private float tickingSoundEndTime;
    private AudioSource audioSource;
    private bool isPressed = false;
    private ButtonTrigger buttonTrigger;
    private Animator obstacleAnimator;
    private static int coopButtonCount = 0; // Static counter for coop mode
    public AudioClip RockGrind; // The ticking sound to play every second
    public LeverScript lever; // Reference to the lever
    public GameObject secondButton; // Reference to the second button
    private int playerCount;

    private void Start()
    {
        playerCount = 1;

        if (PhotonNetwork.IsConnected)
        {
            playerCount = MainManager.Instance.players.Count;
        }


        // Destroy the second button if there are less than three players
        if (playerCount < 3 && secondButton != null)
        {
            Destroy(secondButton);
        }

        // Find the ButtonTrigger component on the same object
        buttonTrigger = GetComponent<ButtonTrigger>();
        audioSource = GetComponent<AudioSource>();
        obstacleAnimator = obstacle.GetComponent<Animator>();


        Debug.Log(playerCount);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed && (lever == null || !lever.IsActivated))
        {
            isPressed = true;
            Debug.Log("Button pressed by player.");

            if (playerCount == 1)
            {
                Debug.Log("Single player mode: Animating obstacle.");
                StartCoroutine(AnimateObstacle());
            }
            else if (playerCount == 2)
            {
                Debug.Log("Two player mode: Activating obstacle in coop mode.");
                ActivateObstacleCoop();
            }
            else if (playerCount >= 3)
            {
                coopButtonCount++;
                Debug.Log($"Three or more players mode: Coop button count is {coopButtonCount}.");
                if (coopButtonCount >= 2)
                {
                    Debug.Log("Coop button count reached 2: Activating obstacle in coop mode.");
                    ActivateObstacleCoop();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPressed)
        {
            isPressed = false;
            Debug.Log("Button released by player.");

            if (playerCount == 2)
            {
                Debug.Log("Two player mode: Deactivating obstacle in coop mode.");
                DeactivateObstacleCoop();
            }
            else if (playerCount >= 3)
            {
                coopButtonCount--;
                Debug.Log($"Three or more players mode: Coop button count is {coopButtonCount}.");
                if (coopButtonCount < 2)
                {
                    Debug.Log("Coop button count less than 2: Deactivating obstacle in coop mode.");
                    DeactivateObstacleCoop();
                }
            }
        }
    }

    private IEnumerator AnimateObstacle()
    {
        obstacleAnimator.SetBool("IsOpen", true);
        float timeLeft = 5f;
        PlayRockSlideSound();
        while (timeLeft > 0)
        {
            timerTextMesh.text = Mathf.CeilToInt(timeLeft).ToString();
            PlayTickingSound(tickingSoundStartTime, tickingSoundEndTime);
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }
        timerTextMesh.text = "";
        if (lever == null || !lever.IsActivated)
        {
            PlayRockSlideSound();
            obstacleAnimator.SetBool("IsOpen", false);
            isPressed = false;
        }
        // Stop ticking sound and reset the button sprite
        StopTickingSound();
        if (buttonTrigger != null)
        {
            buttonTrigger.ResetSprite();
        }
    }

    private void ActivateObstacleCoop()
    {
        obstacleAnimator.SetBool("IsOpen", true);
        PlayRockSlideSound();
        Debug.Log("Obstacle activated in coop mode.");

        // Reset the button sprite
        if (buttonTrigger != null)
        {
            buttonTrigger.ResetSprite();
        }
    }

    private void DeactivateObstacleCoop()
    {
        obstacleAnimator.SetBool("IsOpen", false);
        Debug.Log("Obstacle deactivated in coop mode.");

        // Reset the button sprite
        if (buttonTrigger != null)
        {
            buttonTrigger.ResetSprite();
        }
    }

    private void PlayTickingSound(float startTime, float endTime)
    {
        if (audioSource != null && tickingSound != null)
        {
            audioSource.clip = tickingSound;
            audioSource.time = startTime;
            audioSource.Play();
            StartCoroutine(StopTickingSoundAfterDuration(endTime - startTime));
        }
    }

    private void PlayRockSlideSound()
    {
        SoundManager.instance.PLayWithStartEnd(RockGrind, transform, 0f, 1f, 0.2f);
    }

    private void StopTickingSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator StopTickingSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }

    public void DeactivateScript()
    {
        this.enabled = false;
    }
}
