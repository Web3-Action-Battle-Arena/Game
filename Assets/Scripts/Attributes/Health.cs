using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SyncVar]
    public float healthPoints = 100f;

    bool isDead = false;

    public void TakeDamage(GameObject instigator, float damage)
    {
        print(gameObject.name + " took damage: " + damage);
        healthPoints = Mathf.Max(healthPoints - damage, 0);
        if (healthPoints == 0 && !isDead)
        {
            Die();

        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealthPoints()
    {
        return healthPoints;
    }


    private void Die()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("Die");
    }

}
