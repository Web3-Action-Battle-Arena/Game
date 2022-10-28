using Cinemachine;
using FishNet.Object;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    private CinemachineVirtualCamera _camera;

    void Update()
    {
        if (!base.IsOwner) return;
        GameObject player = GameObject.FindWithTag("Player");
        _camera.Follow = player.transform;
    }
}
