using System;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using MEC;
using DesfribilatorPlugin;
using UnityEngine;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Enums;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using Exiled.Events.Handlers;
using Exiled.CustomItems;
using static UnityEngine.GraphicsBuffer;

public class Extensions
{
    public static void DesSpawnProtection(Exiled.API.Features.Player ply, int Time)
    {
        if (ply == null) 
         { return; }


        if (ply.IsGodModeEnabled == true)
        {
            ply.IsGodModeEnabled = false;
        }

        ply.SessionVariables.Add("DesInv", null);
        Timing.CallDelayed(Time, () => { ply.SessionVariables.Remove("DesInv"); });
    }

    public static void RespawnHumanPlayer(Exiled.API.Features.Player player, Ragdoll target)
    {
        Exiled.API.Features.Player ply = target.Owner;
        if (Exiled.API.Features.Warhead.IsDetonated)
        {
            Lift liftA = Lift.Get(ElevatorType.GateA);
            Lift liftB = Lift.Get(ElevatorType.GateB);
            if (liftA.IsInElevator(target.Transform.position) || liftB.IsInElevator(target.Transform.position))
            {
                CustomItem.Get(Plugin.Instance.Config.Desfri.Id).Give(player, false);
                player.ShowHint($"{Plugin.Instance.Translation.hintwhenragdollisinliftwhenwarhead}", 2);
                return;
            }
        }
        ply.Role.Set(target.Role, RoleSpawnFlags.None);
        ply.Health = Mathf.RoundToInt(Plugin.Instance.Config.PercentageOfHPWhenRevived / 100 * ply.MaxHealth);
        ply.EnableEffect(EffectType.Burned, 15, false);
        ply.EnableEffect(EffectType.AmnesiaVision, 25, false);
        var revivingEffects = Plugin.Instance.Config.RevivingEffects;
        foreach (var Effects in revivingEffects)
        {
            var EffectType = Effects.Key;
            var TimeEffect = Effects.Value;
            ply.EnableEffect(EffectType, TimeEffect);
        }
        ply.Position = new Vector3(target.Position.x, target.Position.y + 2, target.Position.z);
        ply.ShowHint($"{Plugin.Instance.Translation.MessageWhenYouRevive}".Replace("{PlayerName}", player.DisplayNickname), 4);
        if (ply.CurrentRoom == Room.Get(RoomType.Pocket))
            ply.EnableEffect(EffectType.PocketCorroding, 9999, false);
        if (Plugin.Instance.Config.ProctetionDamageTime > 0)
        {
            Extensions.DesSpawnProtection(ply, Plugin.Instance.Config.ProctetionDamageTime);
        }
        Plugin.Instance.EventHandlers.Cooldown = Plugin.Instance.Config.CooldownTime;
        Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(Plugin.Instance.EventHandlers.TimerCooldown()));
        target.Destroy();
        Log.Info($"Player {ply.Nickname} revived successfully.");
    }

    public static void RespawnSCP(Exiled.API.Features.Player player, Ragdoll SCPragdoll)
    {
        Exiled.API.Features.Player ply = SCPragdoll.Owner;
        if (Exiled.API.Features.Warhead.IsDetonated)
        {
            Lift liftA = Lift.Get(ElevatorType.GateA);
            Lift liftB = Lift.Get(ElevatorType.GateB);
            if (liftA.IsInElevator(SCPragdoll.Transform.position) || liftB.IsInElevator(SCPragdoll.Transform.position))
            {
                CustomItem.Get(Plugin.Instance.Config.Desfri.Id).Give(player, false);
                player.ShowHint($"{Plugin.Instance.Translation.hintwhenragdollisinliftwhenwarhead}", 2);
                return;
            }
        }
        ply.Role.Set(SCPragdoll.Role, RoleSpawnFlags.None);
        ply.Health = Mathf.RoundToInt(Plugin.Instance.Config.PercentageOfHPWhenSCPRevived / 100 * ply.MaxHealth);
        var revivingEffectsSCPs = Plugin.Instance.Config.RevivingEffectsSCPs;
        foreach (var Effects in revivingEffectsSCPs)
        {
            var EffectType = Effects.Key;
            var TimeEffect = Effects.Value;
            ply.EnableEffect(EffectType, TimeEffect);
        }
        ply.Position = new Vector3(SCPragdoll.Position.x, SCPragdoll.Position.y + 2, SCPragdoll.Position.z);
        ply.ShowHint($"{Plugin.Instance.Translation.MessageWhenYouReviveSCP}".Replace("{PlayerName}", player.DisplayNickname), 4);
        if (Plugin.Instance.Config.ProctetionDamageTime > 0)
        {
            Extensions.DesSpawnProtection(ply, Plugin.Instance.Config.ProctetionDamageTime);
        }
        Plugin.Instance.EventHandlers.Cooldown = Plugin.Instance.Config.CooldownTimeSCP;
        Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(Plugin.Instance.EventHandlers.TimerCooldown()));
        SCPragdoll.Destroy();
        Log.Info($"The SCP {ply.Nickname} has revived.");
    }
}