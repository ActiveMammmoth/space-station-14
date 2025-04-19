using Content.Shared.ActionBlocker;
using Content.Shared.Emoting;
using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory.Events;
using Content.Shared.Item;
using Content.Shared.Magic.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Pointing;
using Content.Shared.Speech;
using Content.Shared.Throwing;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using Content.Shared.Physics;
using Robust.Shared.Physics.Dynamics;
using System.Linq;

namespace Content.Shared.Magic.Systems;

public abstract class PetrifySpellSystem : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _blocker = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PetrifyComponent, ComponentStartup>(OnPetrify);
        SubscribeLocalEvent<PetrifyComponent, ComponentShutdown>(OnUnpetrify);
        SubscribeLocalEvent<StasisComponent, ComponentStartup>(OnStasis);
        SubscribeLocalEvent<StasisComponent, ComponentShutdown>(OnUnstasis);

        SubscribeLocalEvent<StasisComponent, ChangeDirectionAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, UpdateCanMoveEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, UseAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, ThrowAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, DropAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, AttackAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, PickupAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, SpeakAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, EmoteAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, PointAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<StasisComponent, IsEquippingAttemptEvent>(OnEquipAttempt);
        SubscribeLocalEvent<StasisComponent, IsUnequippingAttemptEvent>(OnUnequipAttempt);
        SubscribeLocalEvent<StasisComponent, InteractionAttemptEvent>(OnAttemptInteract);
    }

    protected virtual void OnPetrify(Entity<PetrifyComponent> ent, ref ComponentStartup args)
    {
        var ev = new PetrifySpellEvent();
        RaiseLocalEvent(ent, ref ev);
    }

    protected virtual void OnUnpetrify(Entity<PetrifyComponent> ent, ref ComponentShutdown args)
    {

    }

    protected virtual void OnStasis(Entity<StasisComponent> ent, ref ComponentStartup args)
    {
        _blocker.UpdateCanMove(ent);

        if (!TryComp<FixturesComponent>(ent, out var fixtures) || !TryComp<PhysicsComponent>(ent, out var physics))
            return;

        var fixture = fixtures.Fixtures.First();

        ent.Comp.FormerCollisionLayer = fixture.Value.CollisionLayer;
        ent.Comp.FormerCollisionMask = fixture.Value.CollisionMask;

        _physics.SetCollisionMask(ent, fixture.Key, fixture.Value, (int)CollisionGroup.MachineMask, fixtures, physics);
        _physics.SetCollisionLayer(ent, fixture.Key, fixture.Value, (int)CollisionGroup.MachineLayer, fixtures, physics);
    }

    protected virtual void OnUnstasis(Entity<StasisComponent> ent, ref ComponentShutdown args)
    {
        _blocker.UpdateCanMove(ent);

        if (!TryComp<FixturesComponent>(ent, out var fixtures) || !TryComp<PhysicsComponent>(ent, out var physics))
            return;

        var fixture = fixtures.Fixtures.First();

        _physics.SetCollisionMask(ent, fixture.Key, fixture.Value, ent.Comp.FormerCollisionMask, fixtures, physics);
        _physics.SetCollisionLayer(ent, fixture.Key, fixture.Value, ent.Comp.FormerCollisionLayer, fixtures, physics);
    }

    private void OnAttempt(EntityUid uid, StasisComponent stasis, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void OnEquipAttempt(EntityUid uid, StasisComponent stasis, IsEquippingAttemptEvent args)
    {
        if (args.Equipee == uid)
            args.Cancel();
    }

    private void OnUnequipAttempt(EntityUid uid, StasisComponent stasis, IsUnequippingAttemptEvent args)
    {
        if (args.Unequipee == uid)
            args.Cancel();
    }

    private void OnAttemptInteract(Entity<StasisComponent> ent, ref InteractionAttemptEvent args)
    {
        args.Cancelled = true;
    }
}

[ByRefEvent]
public readonly record struct PetrifySpellEvent;
