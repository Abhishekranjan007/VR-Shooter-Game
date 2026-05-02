using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class XRPauseHandler : MonoBehaviour
{
    private TeleportationProvider teleportProvider;
    private ContinuousMoveProviderBase moveProvider;
    private ContinuousTurnProviderBase turnProvider;

    void Awake()
    {
        teleportProvider = GetComponentInChildren<TeleportationProvider>();
        moveProvider = GetComponentInChildren<ContinuousMoveProviderBase>();
        turnProvider = GetComponentInChildren<ContinuousTurnProviderBase>();
    }

    void Update()
    {
        bool paused = GameManager.Instance.IsPaused();

        if (teleportProvider) teleportProvider.enabled = !paused;
        if (moveProvider) moveProvider.enabled = !paused;
        if (turnProvider) turnProvider.enabled = !paused;
    }
}