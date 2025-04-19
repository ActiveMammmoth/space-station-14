using Content.Shared.Magic.Components;
using Content.Shared.Magic.Systems;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client.Magic.Systems;

public sealed class ClientPetrifySpellSystem : PetrifySpellSystem
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;

    private ShaderInstance _shader = default!;

    public override void Initialize()
    {
        base.Initialize();

        _shader = _protoMan.Index<ShaderPrototype>("Greyscale").InstanceUnique();
    }

    protected override void OnPetrify(Entity<PetrifyComponent> ent, ref ComponentStartup args)
    {
        base.OnPetrify(ent, ref args);

        SetShader(ent, true);
    }

    protected override void OnUnpetrify(Entity<PetrifyComponent> ent, ref ComponentShutdown args)
    {
        base.OnUnpetrify(ent, ref args);

        SetShader(ent, false);
    }

    private void SetShader(EntityUid ent, bool enabled, SpriteComponent? sprite = null)
    {
        if (!Resolve(ent, ref sprite, false))
            return;

        sprite.PostShader = enabled ? _shader : null;
        sprite.GetScreenTexture = enabled;
        sprite.RaiseShaderEvent = enabled;
    }
}
