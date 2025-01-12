namespace DesfribilatorPlugin
{
    using Exiled.API.Interfaces;

    public class Translation : ITranslation
    {
        public string MessageWhenYouRevive { get; private set; } = "You feel like you recover your life\n<color=#028EDC>{PlayerName} has revived you with a defibrillator!</color>";
        public string CooldownHint { get; private set; } = "There is a Cooldown of {Cooldown}";
        public string TimeOfGrace { get; private set; } = "You cannot use the defibrillator until after {Time}";
        public string MessageWhenYouReviveSCP { get; private set; } = "<color=red>Someone make the error of reviving You...</color>\n<color=#028EDC>{PlayerName} has revived you with a defibrillator!</color>";
        public string BlacklistedSCPMessage { get; private set; } = "You can't revive something like that\n<color=red>Maybe you can revive other SCPs...</color>";
        public string MessageWhenARagdollnotavailable { get; private set; } = "This body has not a soul anymore...\n<color=red>You shouldn't try again</color>";
        public string hintwhenthereisnoragdollnearby { get; private set; } = "¡Your need a corspe!";
        public string hintwhenragdollisinliftwhenwarhead { get; private set; } = "¡You can't!";
    }
}
