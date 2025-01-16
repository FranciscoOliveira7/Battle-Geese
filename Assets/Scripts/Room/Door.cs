using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Room _room;
    public Door NextDoor;
    private const float _doorTime = 1f;
    private float _timeElapsedIn;
    private GameObject _visualArea;
    private ParticleSystem _doorAura;

    private static Collider[] _playerIn = new Collider[3];

    public bool _allPlayersIn = false;

    private Animator _animator;

    // Hitbox
    private Vector3 _hitboxPos;
    private Vector3 _hitboxSize;

    private static CoolAnimationClass _animationClass;

    // New fields
    [SerializeField] private bool isLobbyRoom = false;
    [SerializeField] private string NextGameScene;

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _visualArea = transform.Find("Area").gameObject;
        
        _doorAura = transform.Find("Aura").GetComponent<ParticleSystem>();

        _animationClass = new CoolAnimationClass();
        // Hitbox
        _hitboxPos = transform.position + new Vector3(0f, 1f, 0f);
        _hitboxSize = Vector3.one;

        _room = GetComponentInParent<Room>();

        _room.OnLock += OnRoomLock;
        // if (!_room._firstRoom) _room.Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayersIn() == MainManager.Instance.players.Count)
        {
            _allPlayersIn = true;
            _timeElapsedIn = 0;
        }
    }

    private void OnTriggerExit(Collider other) => _allPlayersIn = false;

    private void Update()
    {
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;
        
        if (_allPlayersIn && !_room.isInBattle) _timeElapsedIn += Time.deltaTime;

        if (_timeElapsedIn < _doorTime) return;

        ExitRoom();
    }

    private int PlayersIn()
    {
        return Physics.OverlapBoxNonAlloc(_hitboxPos, _hitboxSize, _playerIn, quaternion.identity, LayerMask.GetMask("Player"));
    }

    private void EnterRoom()
    {
        _room.Activate();
        Room.activeRoom = _room;

        // TP all player to door
        Vector3 spawnpoint = transform.position + transform.forward * 1.5f;

        // keep player y coord
        spawnpoint.y = MainManager.Instance.myPlayer.transform.position.y;
        
        int playersCount = MainManager.Instance.players.Count;

        for (int i = 0; i < playersCount; i++)
        {
            MainManager.Instance.players[i].RigidBody.position = spawnpoint + Vector3.right * (-0.5f * (playersCount - 1) + 1f * i);
        }
        // foreach (var player in _playerIn)
        // {
        //     if (!player) continue;
        //     player.gameObject.GetComponent<Rigidbody>().position = spawnpoint - Vector3.right * (playersCount - 1) * 0.5f + ;
        // }

        _timeElapsedIn = 0f;

        _animator.SetBool("opened", false);
    }

    public void ExitRoom()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            MainManager.Instance.TransitionPlayerToNextRoom(transform.position);
        }
        
        if (isLobbyRoom) MainManager.Instance.SavePlayerWeapons();
        
        _timeElapsedIn = 0f;
        _allPlayersIn = false;

        _animator.SetBool("opened", true);

        if (isLobbyRoom)
        {
            MainManager.Instance.ChangeLevel(NextGameScene);
            return;
        }
        
        _animationClass.CoolRoomTransition(this);
        MainManager.Instance.myPlayer.StateMachine.EnteringDoorState.SetDoor(this);
        MainManager.Instance.myPlayer.StateMachine.ChangeState(MainManager.Instance.myPlayer.StateMachine.EnteringDoorState);
    }

    public void GoToNextDoor()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                if (isLobbyRoom)
                {
                    MainManager.Instance.ChangeLevel(NextGameScene);
                }
            }
        }
        else
        {
            if (isLobbyRoom)
            {
                MainManager.Instance.ChangeLevel(NextGameScene);
            }
        }
        _animationClass.CoolRoomTransitionExit();
        _room.Deactivate();
        if (!isLobbyRoom) NextDoor.EnterRoom();
    }

    private void OnDrawGizmos()
    {
        // Hitbox
        Vector3 pos = transform.position + new Vector3(0f, 1f, 0f);
        Vector3 size = 2f * Vector3.one;
        Gizmos.DrawWireCube(pos, size);
    }

    public void StartRoomBattle() => _room.StartBattle();

    void OnRoomLock(bool isLocked)
    {
        _animator.SetBool("locked", isLocked);
        _visualArea.SetActive(!isLocked);
        
        if (isLocked)
        {
            _doorAura.Stop();
            _doorAura.Clear();
        }
        else _doorAura.Play();
    }
}
