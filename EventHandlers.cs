using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using UnityEngine;

namespace Defibrillator
{
    public class EventHandler
    {
        private Plugin _plugin = Plugin.Instance;
        public int Cooldown = 0;
        public int TimeGrace = Plugin.Instance.Config.GraceTime;

        public void OnStart()
        {
            TimeGrace = Plugin.Instance.Config.GraceTime;
            Plugin.Instance.Coroutines.Add(Timing.RunCoroutine(Plugin.Instance.EventHandlers.TimeOfGrace()));
            foreach (RoomType room in _plugin.Config.RoomTypes)
            {
                CustomItem.Get($"{_plugin.Config.Defibrillator.Name}")
                    ?.Spawn(Room.Get(room).Position + new Vector3(0, 1f, 0));
            }
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (CoroutineHandle coroutine in Plugin.Instance.Coroutines)
                Timing.KillCoroutines(coroutine);
            Plugin.Instance.Coroutines.Clear();
            Cooldown = 0;
            Cooldown = 0;
            TimeGrace = 120;
        }

        private IEnumerator<float> TimeOfGrace()
        {
            for (;;)
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
            for (;;)
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