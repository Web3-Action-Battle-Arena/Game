using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] public int baseDamage = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            print(other.transform.name);
            health.TakeDamage(baseDamage);


        }
    }
}
