using Content.Shared.Actions;

namespace Content.Shared.Magic.Events;

public sealed partial class LightningSpellEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string LightningPrototype = "Lightning";

    [DataField]
    public string? Speech { get; private set; }
}
