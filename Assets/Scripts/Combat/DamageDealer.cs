using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] Weapon weapon = null;

    private void OnTriggerEnter(Collider other)
    {
        // check if the object hit is not self

        Health target = other.gameObject.GetComponent<Health>();
        if (target == null) return;
        if (target.IsDead()) return;

        target.TakeDamage(weapon.GetDamage());
    }
}
