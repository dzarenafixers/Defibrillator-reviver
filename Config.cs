using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace Defibrillator
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Is allowed revive SCPs")]
        public bool SCPRevive { get; set; } = true;

        [Description("Grace time at the start of the game to be able to use the defibrillator.")]
        public int GraceTime { get; set; } = 150;

        [Description("Time when the revived is invulnerable (set it to 0 to disable it).")]
        public int ProctetionDamageTime { get; set; } = 3;

        [Description("Blacklisted SCPs")]
        public List<RoleTypeId> SCPBlacklisted { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.Scp079,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
        };
        [Description("Cooldown that all people will have to use the defibrillator after they have used one")]
        public int CooldownTime { get; set; } = 120;
        [Description("Percentage of life they will have when they revive a player for the maximum life (e.g. 80 and they have 120 they will have 96 HP)")]
        public float PercentageOfHPWhenRevived { get; set; } = 75;
        [Description("List Of Effects and time when someone revives him")]
        public Dictionary<EffectType, float> RevivingEffects { get; set; } = new Dictionary<EffectType, float>
        {
            { EffectType.AmnesiaVision, 25 },
            { EffectType.Burned, 10 },
        };
        [Description("Cooldown that all people will have to use the defibrillator after they have used one to revive an SCP")]
        public int CooldownTimeSCP { get; set; } = 220;
        [Description("Percentage of life they will have when they revive a SCP for the maximum life (e.g. 80 and they have 120 they will have 96 HP)")]
        public float PercentageOfHPWhenSCPRevived { get; set; } = 30;
        [Description("List Of Effects and time when someone revives him(For SCPs)")]
        public Dictionary<EffectType, float> RevivingEffectsSCPs { get; set; } = new Dictionary<EffectType, float>
        {
            { EffectType.Burned, 15 },
        };

        [Description("list of rooms to spawn the defibrillator inside")]
        public List<RoomType> RoomTypes { get; set; } = new List<RoomType> 
        {
            RoomType.LczArmory,
            RoomType.HczArmory,
            RoomType.HczHid,
        };

        [Description("Desfribilador/Desfribilator Config:")]
        public Defibrillator Defibrillator { get; private set; } = new Defibrillator();
    }
}
