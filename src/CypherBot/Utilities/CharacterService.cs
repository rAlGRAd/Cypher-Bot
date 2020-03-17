using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.EntityFrameworkCore;
using CypherBot.Core.Models;
using CypherBot.Core.DataAccess.Repos;


namespace CypherBot.Utilities
{
    public class CharacterService
    {
        private CypherContext _cypherContext = null;

        public CharacterService(CypherContext cypherContext, RandomGeneratorService randomGenerator)
        {
            _cypherContext = cypherContext;
        }

        public async Task<Character> GetCurrentPlayersCharacterAsync(CommandContext ctx)
        {
            var chr = Data.CharacterList.Characters.FirstOrDefault(x => x.Player == ctx.Member.Username + ctx.Member.Discriminator);

            if (chr == null)
            {
                await ctx.RespondAsync("Hey!  you don't have any characters!");
            }

            return chr;
        }

        public async Task<List<Character>> GetCurrentPlayersCharactersAsync(CommandContext ctx)
        {
            var chars = await _cypherContext.Characters
                .Include(x => x.Cyphers)
                .Include(x => x.Inventory)
                .Include(x => x.RecoveryRolls).Where(x => x.Player == ctx.Member.Username + ctx.Member.Discriminator).ToListAsync();

            return chars;
        }

        public async Task<string> GetCurrentCharacterCyphersAsync(CommandContext ctx)
        {
            var chr = await GetCurrentPlayersCharacterAsync(ctx);

            var response = "Here are your current cyphers:" + Environment.NewLine;

            foreach (var cypher in chr.Cyphers)
            {
                response += "**Name:** " + cypher.Name + Environment.NewLine;
                response += "**Level:** " + cypher.Level + Environment.NewLine;
                response += "**Effect:** " + cypher.Effect + Environment.NewLine + Environment.NewLine;
            }

            return response;
        }

        public async Task<string> GetCurrentCharacterArtifactsAsync(CommandContext ctx)
        {
            var chr = await GetCurrentPlayersCharacterAsync(ctx);

            var response = "Here are your current artifacts:" + Environment.NewLine;

            foreach (var artifact in chr.Artifacts)
            {
                response += "**Name:** " + artifact.Name + Environment.NewLine;
                response += "**Level:** " + artifact.Level + Environment.NewLine;
                response += "**Effect:** " + artifact.Effect + Environment.NewLine + Environment.NewLine;
            }

            return response;
        }


        public async Task<string> GetCurrentCharacterInventoryAsync(CommandContext ctx)
        {
            var chr = await GetCurrentPlayersCharacterAsync(ctx);

            var responses = new List<string>();

            responses.Add("Here is your inventory:");
            var i = 1;
            foreach (var inv in chr.Inventory)
            {
                responses.Add($"{i++}. {inv.ItemName} : {inv.Qty}x");
            }

            return string.Join(Environment.NewLine, responses);
        }

        public async Task SaveCurrentCharacterAsync(string playerId, Character charToSave)
        {

            var chr = _cypherContext.Characters
                .Include(x => x.Cyphers)
                .Include(x => x.Inventory)
                .Include(x => x.RecoveryRolls)
                .FirstOrDefault(x => x.CharacterId == charToSave.CharacterId);

            if (chr == null)
            {
                _cypherContext.Characters.Add(charToSave);
            }
            else
            {
                _cypherContext.Entry(chr).CurrentValues.SetValues(charToSave);

                foreach (var cy in chr.Cyphers)
                {
                    if (!charToSave.Cyphers.Any(x => x.CypherId == cy.CypherId))
                    {
                        _cypherContext.Remove(cy);
                    }
                }

                foreach (var inv in chr.Inventory)
                {
                    if (!charToSave.Inventory.Any(x => x.InventoryId == inv.InventoryId))
                    {
                        _cypherContext.Remove(inv);
                    }
                }

                foreach (var roll in chr.RecoveryRolls)
                {
                    if (!charToSave.RecoveryRolls.Any(x => x.RecoveryRollId == roll.RecoveryRollId))
                    {
                        _cypherContext.Remove(roll);
                    }
                }
            }

            try
            {
                await _cypherContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

    }
}
