using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CypherBot.Utilities;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace CypherBot.Commands
{
    [Group("dice")]
    public class DiceCommands : BaseCommandModule
    {
        private RandomGeneratorService _randomGeneratorService = null;

        public DiceCommands(RandomGeneratorService randomGenerator)
        {
            _randomGeneratorService = randomGenerator;
        }


        [Command("roll")]
        public async Task Random(CommandContext ctx, int max)
        {
            var rnd = _randomGeneratorService.GetRandom();
            var dieroll = rnd.Next(1, max);
            await ctx.RespondAsync($"🎲 Your random number is: {dieroll}");
        }

        [Command("rollcypher")]
        public async Task CypherRoll(CommandContext ctx)
        {
            var rnd = _randomGeneratorService.GetRandom();
            var dieroll = rnd.Next(1, 20);
            var levelbeats = Math.Floor((decimal)dieroll / 3);
            await ctx.RespondAsync($"{ctx.Member.DisplayName} 🎲 Your random number is: {dieroll} and beats level: {levelbeats}");
        }
    }
}
