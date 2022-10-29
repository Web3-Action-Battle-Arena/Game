using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] Weapon weapon = null;

    private bool alreadyAttackedThisMove = false;

    private void OnTriggerEnter(Collider other)
    {
        Animator animator = transform.root.GetComponent<Animator>();
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack") || alreadyAttackedThisMove)
        {
            alreadyAttackedThisMove = false;
            return;
        };

        Health target = other.gameObject.GetComponent<Health>();
        if (target == null) return;
        if (target.IsDead()) return;

        target.TakeDamage(weapon.GetDamage());
        alreadyAttackedThisMove = true;
    }
}
