using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace MiniPhone.Inventory
{
    internal class InventoryPhone
    {
        private readonly MiniPhoneMod mod;
        private int counter = 0;

        public InventoryPhone(MiniPhoneMod mod)
        {
            this.mod = mod;
            mod.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        }

        public static bool HasPhoneInInventory()
        {
            return Context.IsWorldReady &&
                   Game1.player?.Items != null &&
                   Game1.player.Items.Any(i =>
                       i is StardewValley.Objects.Furniture f &&
                       f.ParentSheetIndex == 3490
                   );
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!HasPhoneInInventory() || !Context.IsPlayerFree)
                return;

            int ticks = MiniPhoneMod.Instance.Config.CheckIntervalSeconds * 60;
            counter++;

            if (counter >= ticks)
            {
                counter = 0;

                if (Game1.random.Next(100) < MiniPhoneMod.Instance.Config.RandomCallChance)
                    MiniPhoneMod.Instance.Calls.TriggerRandomCall();
            }
        }
    }
}
