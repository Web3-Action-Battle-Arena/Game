using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fighter : NetworkBehaviour
{

    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Weapon defaultWeapon = null;

    private Animator _animator;

    public float baseDamge = 5f;

    float timeSinceLastAttack = Mathf.Infinity;

    Weapon currentWeapon;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        EquipWeapon(defaultWeapon);
    }

    // trigger attack on left mouse click
    private void Update()
    {
        if (!base.IsOwner) return;
        timeSinceLastAttack += Time.deltaTime;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            AttackBehavior();
        }
    }

    private void AttackBehavior()
    {
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            // this will trigger the TakeDamage method on the server
            TriggerAttack();
            timeSinceLastAttack = 0;
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        AttachWeapon(weapon);
    }

    private void TriggerAttack()
    {
        _animator.ResetTrigger("StopAttack");
        _animator.SetTrigger("Attack");

    }

    private void StopAttack()
    {
        _animator.SetTrigger("StopAttack");
        _animator.ResetTrigger("Attack");
    }

    // Animation Event
    void Hit()
    {
        print("hit triggered");

    }

    private void AttachWeapon(Weapon weapon)
    {
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }


}