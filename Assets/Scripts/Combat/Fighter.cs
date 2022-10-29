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

    public float baseDamge = 5f;

    Health target;
    float timeSinceLastAttack = Mathf.Infinity;

    Weapon currentWeapon;

    private void Start()
    {
        EquipWeapon(defaultWeapon);

    }

    // trigger attack on left mouse click
    private void Update()
    {
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
        GetComponent<Animator>().ResetTrigger("StopAttack");
        GetComponent<Animator>().SetTrigger("Attack");
    }

    private void StopAttack()
    {
        GetComponent<Animator>().SetTrigger("StopAttack");
        GetComponent<Animator>().ResetTrigger("Attack");
    }


    // Animation Event
    void Hit()
    {
        if (target == null) return;
        target.TakeDamage(gameObject, baseDamge);

    }


    private void AttachWeapon(Weapon weapon)
    {
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }


}