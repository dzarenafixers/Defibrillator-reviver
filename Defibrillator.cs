using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

namespace Defibrillator
{
    /// <inheritdoc />
    [CustomItem(ItemType.Medkit)]
    public class Defibrillator : CustomItem
    {
        /// <inheritdoc/>
        public override uint Id { get; set; } = 222;

        /// <inheritdoc/>
        public override string Name { get; set; } = "DTR-DFRB-001";

        public override ItemType Type { get; set; } = ItemType.Medkit;

        public override Vector3 Scale { get; set; } = new Vector3(0.6f, 0.6f, 0.6f);

        /// <inheritdoc/>
        public override string Description { get; set; } =
            "Defibrillator, it's just a normal defibrillator, it doesn't have anything different, what you do think is?";

        /// <inheritdoc/>
        public override float Weight { get; set; } = 1f;

        public static readonly Vector3 DefibrillatorRange = new Vector3(3f, 1.5f, 3f);

        /// <inheritdoc/>
        public override SpawnProperties SpawnProperties { get; set; }


        /// <inheritdoc/>
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingItem += OnUsing;
            Exiled.Events.Handlers.Player.ThrowingRequest += OnThrown;
            base.SubscribeEvents();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingItem -= OnUsing;
            Exiled.Events.Handlers.Player.ThrowingRequest -= OnThrown;
            base.UnsubscribeEvents();
        }

        private void OnThrown(ThrowingRequestEventArgs ev)
        {
            if (Check(ev.Item) || ev.Item != null)
                return;
            ev.RequestType = ThrowRequest.CancelThrow;
        }

        private void OnUsing(UsingItemEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem) || ev.Item == null)
                return;
            Ragdoll closestRagdoll = null;
            float closestDistance = float.MaxValue;
            foreach (Ragdoll ragdoll in Ragdoll.List)
            {
                float distance = Vector3.Distance(ev.Player.Position, ragdoll.Position);

                if (distance <= DefibrillatorRange.magnitude && distance < closestDistance)
                {
                    closestRagdoll = ragdoll;
                    closestDistance = distance;
                }
            }

            if (Plugin.Instance.EventHandlers.TimeGrace == 0)
            {
                if (Plugin.Instance.EventHandlers.Cooldown == 0)
                {
                    if (closestRagdoll != null)
                    {
                        if (closestRagdoll.Owner != null && closestRagdoll.Owner.IsDead &&
                            !closestRagdoll.Owner.IsHost && Player.List.Contains(ev.Player))
                        {
                            if (closestRagdoll.Role.GetTeam() != Team.SCPs)
                            {
                                Extensions.RespawnHumanPlayer(ev.Player, closestRagdoll);
                                ev.Item.Destroy();
                            }
                            else
                            {
                                if (!Plugin.Instance.Config.SCPBlacklisted.Contains(closestRagdoll.Role) &&
                                    Plugin.Instance.Config.SCPRevive)
                                {
                                    Extensions.RespawnScp(ev.Player, closestRagdoll);
                                    ev.Item.Destroy();
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.ShowHint($"{Plugin.Instance.Translation.BlacklistedSCPMessage}", 3);
                                }
                            }
                        }
                        else
                        {
                            ev.IsAllowed = false;
                            ev.Player.ShowHint($"{Plugin.Instance.Translation.MessageWhenARagdollnotavailable}", 3);
                        }
                    }
                    else
                    {
                        ev.IsAllowed = false;
                        ev.Player.ShowHint($"{Plugin.Instance.Translation.hintwhenthereisnoragdollnearby}");
                    }
                }
                else
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHint(
                        $"{Plugin.Instance.Translation.CooldownHint.Replace("{Cooldown}", Plugin.Instance.EventHandlers.Cooldown.ToString())}",
                        3);
                }
            }
            else
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(
                    $"{Plugin.Instance.Translation.TimeOfGrace.Replace("{Time}", Plugin.Instance.EventHandlers.TimeGrace.ToString())}",
                    3);
            }
        }
    }
}