using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fighter : NetworkBehaviour
{

    [SerializeField] public GameObject equippedWeapon = null;
    [SerializeField] public GameObject rightHandTransform = null;
    [SerializeField] public Animator animator;

    private GameObject spawnedWeapon = null;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            gameObject.GetComponent<Fighter>().enabled = false;
        }

        EquipWeapon(rightHandTransform, equippedWeapon);
    }

    [ServerRpc(RequireOwnership = false)]
    private void EquipWeapon(GameObject spawnPlace, GameObject weapon)
    {
        GameObject currentWeapon = Instantiate(weapon, spawnPlace.transform);
        ServerManager.Spawn(currentWeapon);

    }

    private void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");
            animator.ResetTrigger("StopAttack");
        }
    }




    // Unused animation event
    private void Hit() { }
}
