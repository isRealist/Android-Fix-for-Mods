using StardewModdingAPI.Events;

namespace MiniPhone
{
    internal class RandomCallHandler
    {
        private readonly MiniPhoneMod mod;
        private int counter = 0;

        public RandomCallHandler(MiniPhoneMod mod)
        {
            this.mod = mod;
            mod.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (Game1.activeClickableMenu != null || !Context.IsPlayerFree) return;

            int ticks = mod.Config.CheckIntervalSeconds * 60;
            if (++counter >= ticks)
            {
                counter = 0;
                if (Game1.random.Next(100) < mod.Config.RandomCallChance)
                    mod.Calls.TriggerRandomCall();
            }
        }
    }
}