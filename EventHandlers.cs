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
using Exiled.Events.EventArgs.Player;

namespace DesfribilatorPlugin
{

    public class EventHandler
    {
        private Plugin plugin = Plugin.Instance;
        public int Cooldown = 0;
        public int TimeGrace = Plugin.Instance.Config.GraceTime;
        public void OnStart()
        {
            TimeGrace = Plugin.Instance.Config.GraceTime;
            Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(Plugin.Instance.EventHandlers.TimeOfGrace()));
            foreach (RoomType room in plugin.Config.RoomTypes)
            {
                CustomItem.Get($"{plugin.Config.Desfri.Name}").Spawn(Room.Get(room).Position + new Vector3(0, 1f, 0));
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Player.SessionVariables.ContainsKey("DesInv"))
            {
                if (ev.DamageHandler.Type != DamageType.Warhead || ev.DamageHandler.Type != DamageType.Decontamination)
                {
                    ev.IsAllowed = false;
                }
            }
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (CoroutineHandle coroutine in Plugin.Instance.Coroutines)
                Timing.KillCoroutines(coroutine);
            Plugin.Instance.Coroutines.Clear();
            Cooldown = 0;
            TimeGrace = 120;
        }

        public IEnumerator<float> TimeOfGrace()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1f);
                TimeGrace--;


                if (TimeGrace == 0)
                {
                    foreach (CoroutineHandle coroutine in Plugin.Instance.Coroutines)
                        Timing.KillCoroutines(coroutine);
                    Plugin.Instance.Coroutines.Clear();
                }


            }
        }
        public IEnumerator<float> TimerCooldown()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1f);
                Cooldown--;


                if (Cooldown == 0)
                {
                    foreach (CoroutineHandle coroutine in Plugin.Instance.Coroutines)
                        Timing.KillCoroutines(coroutine);
                    Plugin.Instance.Coroutines.Clear();
                }


            }
        }
    }
}
