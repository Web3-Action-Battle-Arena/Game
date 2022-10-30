using System;
using FishNet.Component.Spawning;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class StatsDispay : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI cubeValue;
    [SerializeField] Health cube;


    public override void OnStartClient()
    {
        base.OnStartClient();
        cube.OnHealthChanged += Cube_OnHealthChanged;
        cubeValue.text = cube.CurrentHealth.ToString();
    }

    private void Cube_OnHealthChanged(int oldHealth, int newHealth, int maxHealth)
    {
        cubeValue.text = newHealth.ToString();
    }

    private void UpdateCanvasElements(int healthValue, float healthPercent)
    {
        cubeValue.text = healthValue.ToString();
    }
}
