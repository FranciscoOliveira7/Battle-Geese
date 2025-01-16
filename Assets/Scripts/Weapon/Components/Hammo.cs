using UnityEngine;

public class Hammo : WeaponComponent<HammoData, AttackHammo>
{
    private int currentHammo;
    private float regenTimer;

    protected override void Start()
    {
        base.Start();
        currentHammo = data.AttackData.MaxHammo;
    }

    private void Update()
    {
        if (currentHammo < data.AttackData.MaxHammo)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= data.AttackData.TimeToRegen1)
            {
                currentHammo++;
                regenTimer = 0f;
            }
        }
    }

    public bool CanUseHammo(int amount) => currentHammo >= amount;

    public void UseHammo(int amount)
    {
        if (currentHammo >= amount)
        {
            currentHammo -= amount;
        }
        else
        {
            // Handle case when there is not enough hammo
            Debug.Log("Not enough hammo.");
        }
    }

    public int GetCurrentHammo() => currentHammo;

    public void ResetHammoToMax()
    {
        currentHammo = data.AttackData.MaxHammo;
    }
}
