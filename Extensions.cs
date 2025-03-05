using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Defibrillator
{
    public static class Extensions
    {
        public static void RespawnHumanPlayer(Player player, Ragdoll target)
        {
            Player ply = target.Owner;
            if (Warhead.IsDetonated)
            {
                Lift liftA = Lift.Get(ElevatorType.GateA);
                Lift liftB = Lift.Get(ElevatorType.GateB);
                if (liftA.IsInElevator(target.Transform.position) || liftB.IsInElevator(target.Transform.position))
                {
                    CustomItem.Get(Plugin.Instance.Config.Defibrillator.Id)?.Give(player, false);
                    player.ShowHint($"{Plugin.Instance.Translation.hintwhenragdollisinliftwhenwarhead}", 2);
                    return;
                }
            }

            ply.Role.Set(target.Role, RoleSpawnFlags.None);
            ply.Health = Mathf.RoundToInt(Plugin.Instance.Config.PercentageOfHPWhenRevived / 100 * ply.MaxHealth);
            ply.EnableEffect(EffectType.Burned, 15, false);
            ply.EnableEffect(EffectType.AmnesiaVision, 25, false);
            var revivingEffects = Plugin.Instance.Config.RevivingEffects;
            foreach (var effects in revivingEffects)
            {
                var effectType = effects.Key;
                var timeEffect = effects.Value;
                ply.EnableEffect(effectType, timeEffect);
            }

            ply.Position = new Vector3(target.Position.x, target.Position.y + 2, target.Position.z);
            ply.ShowHint(
                $"{Plugin.Instance.Translation.MessageWhenYouRevive}".Replace("{PlayerName}", player.DisplayNickname),
                4);
            if (ply.CurrentRoom == Room.Get(RoomType.Pocket))
                ply.EnableEffect(EffectType.PocketCorroding, 9999, false);
            if (Plugin.Instance.Config.ProctetionDamageTime > 0)
            {
                ply.EnableEffect(EffectType.SpawnProtected, Plugin.Instance.Config.ProctetionDamageTime);
            }

            Plugin.Instance.EventHandlers.Cooldown = Plugin.Instance.Config.CooldownTime;
            Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(Plugin.Instance.EventHandlers.TimerCooldown()));
            target.Destroy();
            Log.Debug($"Player {ply.Nickname} revived successfully.");
        }

        public static void RespawnScp(Player player, Ragdoll scpRagdoll)
        {
            Player ply = scpRagdoll.Owner;
            if (Warhead.IsDetonated)
            {
                Lift liftA = Lift.Get(ElevatorType.GateA);
                Lift liftB = Lift.Get(ElevatorType.GateB);
                if (liftA.IsInElevator(scpRagdoll.Transform.position) ||
                    liftB.IsInElevator(scpRagdoll.Transform.position))
                {
                    CustomItem.Get(Plugin.Instance.Config.Defibrillator.Id)?.Give(player, false);
                    player.ShowHint($"{Plugin.Instance.Translation.hintwhenragdollisinliftwhenwarhead}", 2);
                    return;
                }
            }

            ply.Role.Set(scpRagdoll.Role, RoleSpawnFlags.None);
            ply.Health = Mathf.RoundToInt(Plugin.Instance.Config.PercentageOfHPWhenSCPRevived / 100 * ply.MaxHealth);
            var revivingEffectsScPs = Plugin.Instance.Config.RevivingEffectsSCPs;
            foreach (var effects in revivingEffectsScPs)
            {
                var effectType = effects.Key;
                var timeEffect = effects.Value;
                ply.EnableEffect(effectType, timeEffect);
            }

            ply.Position = new Vector3(scpRagdoll.Position.x, scpRagdoll.Position.y + 2, scpRagdoll.Position.z);
            ply.ShowHint(
                $"{Plugin.Instance.Translation.MessageWhenYouReviveSCP}".Replace("{PlayerName}",
                    player.DisplayNickname), 4);
            if (Plugin.Instance.Config.ProctetionDamageTime > 0)
            {
                ply.EnableEffect(EffectType.SpawnProtected, Plugin.Instance.Config.ProctetionDamageTime);
            }

            Plugin.Instance.EventHandlers.Cooldown = Plugin.Instance.Config.CooldownTimeSCP;
            Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(Plugin.Instance.EventHandlers.TimerCooldown()));
            scpRagdoll.Destroy();
            Log.Debug($"The SCP {ply.Nickname} has revived.");
        }
    }
}