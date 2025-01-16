using System.Collections;
using System.Linq;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine;

    public PlayerInputHandler Input { get; private set; }

    [field: SerializeField] public PlayerSO Data { get; private set; }
    [SerializeField] private AudioClip _stepSound;
    [SerializeField] private AudioClip _honkSound;
    public AudioClip DeadSound;

    public Rigidbody RigidBody { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    [HideInInspector] public Weapon[] Weapons = new Weapon[2];
    [HideInInspector] public Consumable CurrentConsumable; // Store the current consumable
    [HideInInspector] public int EquipedWeapon;
    [HideInInspector] public float AttackSpeed = 1f;

    public Animator Animator { get; private set; }

    [HideInInspector] public SpriteRenderer SpriteRenderer;

    private CinemachineImpulseSource _impulseSource;
    [HideInInspector] public AnimationEventHandler EventHandler;

    public GameObject deadImage;
    private InventoryUI inventoryUI; // Reference to the InventoryUI script for weapons
    private ConsumableInventoryUI consumableInventoryUI; // Reference to the InventoryUI script for consumables

    [HideInInspector] public PhotonView photonView; // for multiplayer

    private CinemachineVirtualCamera vcam;

    public bool IsDashing;
    public bool IsDead;

    private void Awake()
    {
        GameObject hud = GameObject.FindGameObjectWithTag("HUD");

        photonView = GetComponent<PhotonView>();

        vcam = GameObject.FindGameObjectWithTag("cameraman").GetComponent<CinemachineVirtualCamera>();

        inventoryUI = hud.transform.Find("InventoryWeapons").GetComponent<InventoryUI>();
        consumableInventoryUI = hud.transform.Find("InventoryConsumable").GetComponent<ConsumableInventoryUI>();
        deadImage = hud.transform.Find("dead").gameObject;

        Input = GetComponent<PlayerInputHandler>();

        RigidBody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();

        SpriteRenderer = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
        EventHandler = SpriteRenderer.GetComponent<AnimationEventHandler>();
        Animator = SpriteRenderer.GetComponent<Animator>();

        Weapons[0] = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        Weapons[1] = transform.Find("SecondaryWeapon").GetComponent<Weapon>();

        StateMachine = new PlayerStateMachine(this);

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        StartCoroutine(Funny());
        // Estado inicial
        StateMachine.Initialize(StateMachine.IdlingState);

        if (!PhotonNetwork.IsConnectedAndReady) LoadWeapons();

        if (!MainManager.Instance.players.Contains(this))
        {
            MainManager.Instance.players.Add(this);
        }

        if (photonView.IsMine)
        {
            MainManager.Instance.myPlayer = this;
            vcam.Follow = transform;
            vcam.LookAt = transform;
            LoadWeapons();
        }

        if (PlayerHealthBar.Instance != null) PlayerHealthBar.Instance.SetPlayer();

        EventHandler.OnStep += Step;

        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            GetComponent<HealthComponent>().OnDamageReceived += HandleDamage;
            GetComponent<HealthComponent>().OnDeath += HandleDeath;
            Input.PlayerActions.UseConsumable.started += UseConsumable; // Add input handling for using consumable
            Input.PlayerActions.Interact.started += Interact; // Add input handling for interacting
            Input.PlayerActions.ShowStats.started += ShowStatus;
            Input.PlayerActions.Honk.started += Honk;
        }

        Weapons[EquipedWeapon].Equip();
        UpdateWeaponInventoryUI(); // Update the weapon inventory UI at the start
        UpdateConsumableInventoryUI(); // Update the consumable inventory UI at the start

        Input.DisableAction(Input.PlayerActions.Move, 0.5f);
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
        StateMachine.HandleInput();
        StateMachine.Update();
        UpdateWeaponInventoryUI(); // Update the weapon inventory UI every frame
        UpdateConsumableInventoryUI(); // Update the consumable inventory UI every frame
    }

    IEnumerator Funny()
    {
        while (1 + 1 == 2)
        {
            yield return new WaitForSeconds(22);
            //Debug.Log("2");
        }
    }

    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
    }

    private void HandleDamage(float amount)
    {
        DamageFeedback();
    }

    public void ShowStatus(InputAction.CallbackContext context)
    {
        WeaponDescPopup.Instance.Toggle();
    }

    private void DamageFeedback()
    {
        if (!PhotonNetwork.IsConnectedAndReady || photonView.IsMine) CameraShakeManager.instance.CameraShake(_impulseSource);
        // DamageOverlay.instance.CallDamageFlash();
    }

    private void HandleDeath()
    {
        if (PhotonNetwork.IsConnectedAndReady) photonView.RPC(nameof(NotifyDeath), RpcTarget.All);
        StateMachine.ChangeState(StateMachine.DeadState);
    }

    [PunRPC]
    public void NotifyDeath()
    {
        MainManager.Instance.deadPlayerCount++;
        
        if (MainManager.Instance.AllPlayersDead())
        {
            MainManager.Instance.OnAllPlayersDead();
        }
    }

    private void Honk(InputAction.CallbackContext context)
    {
        Animator.SetTrigger("honk");

        SoundManager.instance.PlayClip(_honkSound, transform, 0.5f);
    }

    private void Step()
    {
        SoundManager.instance.PlayClipWithRandomPitch(_stepSound, transform, 0.4f, 0.9f, 1.1f);
    }

    public void LoadWeapons()
    {
        if (PlayerData.weapon1 != null)
        {
            EquipedWeapon = 0;
            PickUpWeapon(PlayerData.weapon1);
        }
        if (PlayerData.weapon2 != null)
        {
            EquipedWeapon = 1;
            PickUpWeapon(PlayerData.weapon2);
        }
        if (PlayerData.hat != null)
        {
            PickUpHat(PlayerData.hat);
        }

        EquipedWeapon = 0;
        Weapons[1].Unequip();
        Weapons[0].Equip();

        StateMachine.ChangeState(StateMachine.IdlingState);
        UpdateWeaponInventoryUI(); // Update the weapon inventory UI when loading weapons
    }

    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        SwitchWeaponFR();
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
        {
            photonView.RPC(nameof(SwitchWeaponFR), RpcTarget.Others);
        }
    }

    [PunRPC]
    public void SwitchWeaponFR()
    {
        Weapons[EquipedWeapon].Unequip();

        EquipedWeapon = EquipedWeapon == 0 ? 1 : 0;

        Weapons[EquipedWeapon].Equip();
        UpdateWeaponInventoryUI(); // Update the weapon inventory UI when switching weapons
        inventoryUI.UpdateBackgroundColors(EquipedWeapon); // Update the background colors when switching weapons
        
        if (photonView.IsMine && Weapons[EquipedWeapon].Data != null) WeaponDescPopup.Instance.SetWeapon(Weapons[EquipedWeapon].Data.Name, Weapons[EquipedWeapon].Data.Description);
    }

    public void PickUpWeapon(WeaponDataSO newWeapon)
    {
        if (Weapons[EquipedWeapon] == null) return;

        Weapons[EquipedWeapon].Data = newWeapon;
        Weapons[EquipedWeapon].Generator.Data = newWeapon;
        Weapons[EquipedWeapon].GenerateWeapon();
        Weapons[EquipedWeapon].Equip();
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
        {
            int weaponId = MainManager.Instance.AllWeaponsList.GetWeaponIndex(newWeapon);
            photonView.RPC(nameof(PickUpWeapon), RpcTarget.Others, weaponId, EquipedWeapon);
        }
        if (!PhotonNetwork.IsConnectedAndReady || photonView.IsMine)
            StateMachine.ChangeState(StateMachine.IdlingState);
        
        if (this == MainManager.Instance.myPlayer) WeaponDescPopup.Instance.SetWeapon(newWeapon.Name, newWeapon.Description);

        // Reset the hammo to the maximum value when picking up a new weapon
        Hammo hammo = Weapons[EquipedWeapon].GetComponent<Hammo>();
        hammo?.ResetHammoToMax();

        UpdateWeaponInventoryUI(); // Update the weapon inventory UI when picking up a new weapon
    }

    [PunRPC]
    public void PickUpWeapon(int newWeaponIndex, int slot)
    {
        EquipedWeapon = slot;
        PickUpWeapon(MainManager.Instance.AllWeaponsList.GetWeaponByIndex(newWeaponIndex));
    }

    public void FlipSprite(bool isFlipped)
    {
        transform.localScale = new Vector3(isFlipped ? -1f : 1f, 1f, 1f);

        Weapons[0]?.FlipWeaponSprite(isFlipped ? Facing.left : Facing.right);
        Weapons[1]?.FlipWeaponSprite(isFlipped ? Facing.left : Facing.right);
    }

    public void PickUpHat(HatSO hat)
    {
        foreach (Transform child in SpriteRenderer.transform)
        {
            if (child.gameObject.name == "Base") continue;
            Destroy(child.gameObject);
        }

        GameObject thishat = Instantiate(hat.Prefab, SpriteRenderer.transform);
        
        thishat.GetComponent<HatBehaviour>().Equip();
        
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
        {
            int hatId = MainManager.Instance.AllWeaponsList.GetHatIndex(hat);
            photonView.RPC(nameof(PickUpHat), RpcTarget.Others, hatId);
        }
    }

    [PunRPC]
    public void PickUpHat(int newHatIndex)
    {
        PickUpHat(MainManager.Instance.AllWeaponsList.GetHatByIndex(newHatIndex));
    }

    public HatSO GetHat()
    {
        foreach (Transform child in SpriteRenderer.transform)
        {
            if(child.TryGetComponent(out HatBehaviour hat))
            {
                return hat.data;
            }
        }

        return null;
    }

    public void PickUpConsumable(ConsumableDataSO consumable)
    {
        // Create a new GameObject for the consumable
        GameObject consumableObject = new GameObject("Consumable");
        consumableObject.transform.SetParent(transform);

        // Add the Consumable component to the new GameObject
        CurrentConsumable = consumableObject.AddComponent<Consumable>();
        CurrentConsumable.SetData(consumable);

        // Update the consumable UI
        UpdateConsumableInventoryUI();
    }

    // This method is made to run on a non master client from the photon room so the others clients can go to the same room
    [PunRPC]
    public void DoorTransition(Vector3 doorPos)
    {
        if (!photonView.IsMine) return;

        Door[] doors = Room.activeRoom.transform.GetComponentsInChildren<Door>();

        Door closestDoor = doors[0];

        foreach (Door door in doors)
        {
            if (Vector3.Distance(door.transform.position, doorPos) <
                Vector3.Distance(closestDoor.transform.position, doorPos))
            {
                closestDoor = door;
            }
        }

        closestDoor.ExitRoom();
    }

    [PunRPC]
    public void Revive()
    {
        if (!IsDead) return;

        MainManager.Instance.deadPlayerCount--;
        GetComponent<HealthComponent>().Revive();
        StateMachine.ChangeState(StateMachine.IdlingState);
    }

    private void UseConsumable(InputAction.CallbackContext context)
    {
        if (IsDead) return;

        Debug.Log("UseConsumable action triggered.");
        if (CurrentConsumable != null)
        {
            CurrentConsumable.Use(this);
            if (CurrentConsumable.Data.Counter <= 0)
            {
                Destroy(CurrentConsumable.gameObject); // Remove the consumable after use
                CurrentConsumable = null;
            }
            UpdateConsumableInventoryUI(); // Update the UI slot
        }
        else
        {
            Debug.LogWarning("No consumable to use.");
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 0.5f);

        foreach (Collider player in colliders)
        {
            if (player.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
                continue;
            }
            if (player.TryGetComponent(out PhotonView photonView))
            {
                if (photonView.IsMine || !photonView.gameObject.CompareTag("Player")) continue;
                StateMachine.RevivingState.SetPlayer(photonView);
                StateMachine.ChangeState(StateMachine.RevivingState);
                continue;
            }
        }

        // Handle interaction logic here, such as picking up items
    }

    private void OnDestroy()
    {
        MainManager.Instance.players.Remove(this);
        EventHandler.OnStep -= Step;
        Input.PlayerActions.Honk.started -= Honk;
        Input.PlayerActions.UseConsumable.started -= UseConsumable; // Remove input handling for using consumable
        Input.PlayerActions.SwitchWeapon.performed -= SwitchWeapon; // Remove input handling for switching weapon
        Input.PlayerActions.Interact.started -= Interact; // Remove input handling for interacting
    }

    private void UpdateWeaponInventoryUI() // Method to update the weapon inventory UI
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i].Data != null)
            {
                // Update the weapon slot with the weapon's sprite and pass the weapon instance
                inventoryUI.UpdateWeaponSlot(i, Weapons[i].Data.WeaponSprite, Weapons[i]);
            }
            else
            {
                inventoryUI.UpdateWeaponSlot(i, null, null);
            }
        }
    }

    private void UpdateConsumableInventoryUI() // Method to update the consumable inventory UI
    {
        // Update the consumable slot
        consumableInventoryUI.UpdateConsumableSlot(CurrentConsumable);
    }
}
