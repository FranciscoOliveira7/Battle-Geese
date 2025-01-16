
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRevivingState : PlayerMovementState
{
    private const float reviveTime = 1f;
    private float elapsedTime = 0f;
    PhotonView playerPhotonView;
    private Image image;
    private Image backgroundimage;
    private Image FInteract;

    public PlayerRevivingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        reusableData.SpeedMultiplier = player.Data.RevivingSpeedMultiplier;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetKeyUp(KeyCode.F))
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }

    public override void Update()
    {
        base.Update();

        elapsedTime += Time.deltaTime;

        image.fillAmount = elapsedTime / reviveTime;

        if (elapsedTime < reviveTime) return;

        RevivePlayer();
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    private void RevivePlayer()
    {
        playerPhotonView.RPC("Revive", RpcTarget.All);
        FInteract.gameObject.SetActive(false);

    }

    public void SetPlayer(PhotonView other)
    {
        playerPhotonView = other;
        image = other.transform.Find("ProgressBarRevive/ProgressBar").GetComponent<Image>();
        backgroundimage = other.transform.Find("ProgressBarRevive/Background").GetComponent<Image>();
        FInteract = other.transform.Find("ProgressBarRevive/ReviveF").GetComponent<Image>();
        image.gameObject.SetActive(true);
        backgroundimage.gameObject.SetActive(true);
        image.fillAmount = 0;
    }

    public override void Exit()
    {
        base.Exit();
        image.gameObject.SetActive(false);
        backgroundimage.gameObject.SetActive(false);
        elapsedTime = 0f;
    }
}