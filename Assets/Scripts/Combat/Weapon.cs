
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "Weapon",
        menuName = "Weapons/Make New Weapon",
        order = 0)
]
public class Weapon : ScriptableObject
{
    [SerializeField]
    float weaponDamage = 5f;

    [SerializeField]
    float percentageBonus = 0;

    [SerializeField]
    float weaponRange = 1f;

    [SerializeField]
    bool isRightHanded = true;

    [SerializeField]
    GameObject equippedPrefab = null;

    [SerializeField]
    AnimatorOverrideController animatorOverride = null;

    const string weaponName = "Weapon";

    public void Spawn(
        Transform rightHand,
        Transform leftHand,
        Animator animator
    )
    {
        if (equippedPrefab != null)
        {
            GameObject weapon =
                Instantiate(equippedPrefab,
                GetHandTransform(rightHand, leftHand));
            weapon.name = weaponName;
        }
        var overrideController =
            animator.runtimeAnimatorController as
            AnimatorOverrideController;
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
        }
        else if (overrideController != null)
        {
            animator.runtimeAnimatorController =
                overrideController.runtimeAnimatorController;
        }
    }


    private Transform
    GetHandTransform(Transform rightHand, Transform leftHand)
    {
        return isRightHanded ? rightHand : leftHand;
    }


    public float GetDamage()
    {
        return weaponDamage;
    }

    public float GetRange()
    {
        return weaponRange;
    }
}

