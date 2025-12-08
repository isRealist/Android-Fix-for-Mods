using MiniPhone.Calls;
using MiniPhone.Config;
using MiniPhone.UI;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace MiniPhone
{
    public class MiniPhoneMod : Mod
    {
        internal static MiniPhoneMod Instance { get; private set; } = null!;
        internal ModConfig Config { get; private set; } = null!;
        internal CallManager Calls { get; private set; } = null!;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Config = ModConfig.Load(helper);

            Calls = new CallManager(this);
            new PhoneHudElement();
            new RandomCallHandler(this);

            Monitor.Log("[MiniPhone] Always-on phone with NPC call menu loaded!", LogLevel.Info);
        }

        internal void LogOnce(string msg, LogLevel level = LogLevel.Trace) => Monitor.LogOnce(msg, level);
    }
}