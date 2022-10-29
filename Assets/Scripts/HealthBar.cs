using FishNet.Object;
using UnityEngine;

public class HealthBar : NetworkBehaviour
{

    [SerializeField] Health health = null;
    [SerializeField] RectTransform foreground = null;

    void Update()
    {
        foreground.localScale = new Vector3(health.GetFraction(), 1, 1);
    }
}
