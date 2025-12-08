using System.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace MiniPhone.Calls
{
    internal class CallManager
    {
        private readonly MiniPhoneMod mod;
        public CallManager(MiniPhoneMod mod) => this.mod = mod;

        public void ShowCallMenu()
        {
            var npcs = Utility.getAllCharacters()
                .Where(n => n.isVillager() && !n.isInvisible && n.canTalk())
                .OrderBy(n => n.displayName)
                .ToList();

            if (!npcs.Any())
            {
                Game1.showRedMessage("No one to call!");
                return;
            }

            var responses = npcs.Select(n => new Response(n.Name, n.displayName)).ToArray();
            responses = responses.Concat(new[] { new Response("cancel", "Hang up") }).ToArray();

            Game1.currentLocation.createQuestionDialogue(
                question: "Who would you like to call?",
                answerChoices: responses,
                dialogKey: "MiniPhone_CallMenu"
            );

            Game1.afterDialogues = () =>
            {
                if (Game1.lastQuestionKey?.Contains("MiniPhone_CallMenu") == true)
                {
                    string? choice = Game1.player.getResponseForAnswer("MiniPhone_CallMenu");
                    if (choice != null && choice != "cancel")
                    {
                        var npc = npcs.FirstOrDefault(n => n.Name == choice);
                        if (npc != null) TriggerCall(npc, isManual: true);
                    }
                }
            };
        }

        public void TriggerCall(NPC npc, bool isManual)
        {
            string prefix = isManual ? "call.manual." : "call.random.";
            string key = prefix + npc.Name;

            var custom = mod.Helper.Translation.Get(key);
            string dialogue = !string.IsNullOrWhiteSpace(custom.ToString()) && !custom.ToString().Contains("{{")
                ? custom
                : mod.Helper.Translation.Get("call.npc")
                    .ToString()
                    .Replace("{{NPC}}", npc.displayName)
                    .Replace("{{Player}}", Game1.player.Name);

            if (!isManual && mod.Config.EnableScamCalls && Game1.random.Next(100) < mod.Config.ScamCallChance)
                dialogue = mod.Helper.Translation.Get("call.scam").ToString();

            if (mod.Config.PlaySound) Game1.playSound("phone");

            Game1.objectDialoguePortraitPerson = npc;
            Game1.drawDialogue(npc, dialogue);
        }

        public void TriggerRandomCall()
        {
            var npcs = Utility.getAllCharacters().Where(n => n.isVillager()).ToList();
            if (npcs.Any())
                TriggerCall(npcs[Game1.random.Next(npcs.Count)], isManual: false);
        }
    }
}