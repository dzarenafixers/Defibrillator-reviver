using System;
using System.Collections.Generic;
using Desfribalator;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MEC;

namespace Defibrillator
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Name => "Defibrillator";
        public override string Prefix => "defibrillator";
        public override string Author => "@dzarenafixer";
        public override Version Version { get; } = new Version(1, 4, 1);
        public override PluginPriority Priority => PluginPriority.Default;
        public EventHandler EventHandlers;
        public readonly List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        public static Plugin Instance;


        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandler();
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;

            CustomItem.RegisterItems();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            EventHandlers = null;
            Instance = null;

            CustomItem.UnregisterItems();
        }
    }
}