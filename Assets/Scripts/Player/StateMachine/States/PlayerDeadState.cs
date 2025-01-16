using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        player.Animator.SetBool("running", false);
        player.IsDead = true;
        player.Animator.SetBool("dead", true);
        player.RigidBody.velocity = Vector3.zero;
        player.Collider.enabled = false;
        Debug.Log("Player died lmao");
        player.deadImage.SetActive(true);

        player.EventHandler.OnHitSound += FuckingDiesSfx;
    }

    private void FuckingDiesSfx()
    {
        SoundManager.instance.PlayClip(player.DeadSound, player.transform, 0.1f);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (!PhotonNetwork.IsConnectedAndReady && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.EventHandler.OnHitSound -= FuckingDiesSfx;
        player.Animator.SetBool("dead", false);
        player.Collider.enabled = true;
        player.deadImage.SetActive(false);
        player.IsDead = false;
    }
}
