using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public event Action<int, int, int> OnHealthChanged;
    public event Action OnDeath;

    [SyncVar]
    public int CurrentHealth;

    public int MaximumHealth { get { return _baseHealth; } }

    [Tooltip("Health to start with.")]
    [SerializeField]
    private int _baseHealth = 100;


    private void Awake()
    {
        CurrentHealth = MaximumHealth;
    }

    public override void OnStartClient()
    {
        if (!base.IsOwner)
        {
            GetComponent<Health>().enabled = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int damage)
    {
        RemoveHealth(damage);
    }

    [ObserversRpc]
    private void RemoveHealth(int value)
    {
        int oldHealth = CurrentHealth;

        CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, MaximumHealth);

        OnHealthChanged?.Invoke(oldHealth, CurrentHealth, MaximumHealth);

        if (CurrentHealth <= 0f)
            HealthDepleted();

    }

    public virtual void HealthDepleted()
    {
        OnDeath?.Invoke();
        Die();
    }

    private void Die()
    {
        if (TryGetComponent<Animator>(out Animator animator))
        {
            animator.SetTrigger("Die");
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
