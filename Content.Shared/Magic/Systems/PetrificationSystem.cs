using Content.Shared.ActionBlocker;
using Content.Shared.Emoting;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Magic.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Pointing;
using Content.Shared.Pulling.Events;
using Content.Shared.Speech;
using Content.Shared.Throwing;

namespace Content.Shared.Magic.Systems;

public sealed partial class PetrificationSystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _blocker = default!;
    [Dependency] private readonly BlindableSystem _blindableSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PetrifiedComponent, ComponentStartup>(UpdatePetrifiedStatus);
        SubscribeLocalEvent<PetrifiedComponent, ComponentShutdown>(UpdatePetrifiedStatus);

        SubscribeLocalEvent<PetrifiedComponent, SpeakAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, EmoteAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, PointAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, DropAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, ThrowAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, PickupAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, StartPullAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, ChangeDirectionAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, UpdateCanMoveEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, UseAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, AttackAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<PetrifiedComponent, CanSeeAttemptEvent>(OnAttempt);
    }

    private void OnAttempt(EntityUid uid, PetrifiedComponent comp, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void UpdatePetrifiedStatus(EntityUid uid, PetrifiedComponent comp, EntityEventArgs args)
    {
        _blocker.UpdateCanMove(uid);
        _blindableSystem.UpdateIsBlind(uid);
    }

    public bool TryPetrify(EntityUid uid)
    {
        EnsureComp<PetrifiedComponent>(uid);

        return true;
    }
}
