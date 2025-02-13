using Content.Shared.Emoting;
using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Magic.Components;
using Content.Shared.Pointing;
using Content.Shared.Speech;
using Content.Shared.Throwing;

namespace Content.Shared.Magic.Systems;

public sealed partial class PetrificationSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PetrifiedComponent, ChangeDirectionAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, UseAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, ThrowAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, SpeakAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, DropAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, AttackAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, PickupAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, PointAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, EmoteAttemptEvent>(OnAttempt);
    }

    private void OnAttempt(EntityUid uid, PetrifiedComponent comp, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }
}
