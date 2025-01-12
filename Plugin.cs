namespace DesfribilatorPlugin
{
    using Exiled.API.Features;
    using Exiled.API.Enums;
    using System.Collections.Generic;
    using PlayerRoles;
    using System.Runtime.InteropServices;
    using System;
    using MEC;
    using Exiled.Events.Handlers;
    using Exiled.CustomItems.API.Features;

    public class Plugin : Plugin<Config, Translation>
    {
        public override string Name => "Defibrillator";
        public override string Prefix => "MONCEF50G";
        public override string Author => "@dzarenafixer";
        public override Version Version { get; } = new Version(1, 4, 0);
        public override PluginPriority Priority => PluginPriority.Default;
        public EventHandler EventHandlers;
        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

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
            EventHandlers = null;
            Instance = null;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;

            CustomItem.UnregisterItems();
        }
    }
}