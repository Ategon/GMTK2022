using DataPipeline;
using UnityEngine;

public class PlayerVisualHandling : IHandler<PlayerInteractionState> {
    PlayerVisuals playerVisuals;

    public PlayerVisualHandling(PlayerVisuals playerVisuals)
    {
        this.playerVisuals = playerVisuals;
    }

    public void Handle(in PlayerInteractionState state)
    {
        playerVisuals.HandleAnimations(state);
    }
}