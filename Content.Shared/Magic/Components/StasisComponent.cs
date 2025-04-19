using Robust.Shared.GameStates;

namespace Content.Shared.Magic.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class StasisComponent : Component
{
    public int FormerCollisionLayer;

    public int FormerCollisionMask;
}
