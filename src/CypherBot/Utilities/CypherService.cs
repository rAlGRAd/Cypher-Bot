using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CypherBot.Core.Models;
using CypherBot.Core.DataAccess.Repos;

namespace CypherBot.Utilities
{
    public class CypherService
    {
        private CypherContext _cypherContext = null;
        private RandomGeneratorService _randomGenerator = null;

        public CypherService(CypherContext cypherContext, RandomGeneratorService randomGenerator)
        {
            _cypherContext = cypherContext;
            _randomGenerator = randomGenerator;
        }

        public async Task<List<Cypher>> GetAllCyphersAsync()
        {
            var cyList = await _cypherContext.Cyphers
                .Include(x => x.EffectOptions)
                .Include(x => x.Forms).ToListAsync();

            return cyList;
        }

        public async Task<Cypher> GetRandomCypherAsync()
        {
            var cyList = await GetAllCyphersAsync();

            var i = _randomGenerator.GetRandom().Next(0, cyList.Count() - 1);

            return cyList[i];
        }

        public async Task<List<Cypher>> GetRandomCypherAsync(int numberOfCyphers)
        {
            var ls = new List<Cypher>();
            var rnd = _randomGenerator.GetRandom();

            for (int i = 0; i < numberOfCyphers; i++)
            {
                var cypherList = await GetAllCyphersAsync();
                ls.Add(cypherList[rnd.Next(1, cypherList.Count)]);
            }

            return ls;
        }

        public async Task SaveUnidentifiedCypherAsync(UnidentifiedCypher unidentifiedCypher)
        {
            _cypherContext.UnidentifiedCyphers.Add(unidentifiedCypher);

            await _cypherContext.SaveChangesAsync();
        }

        public async Task<List<UnidentifiedCypher>> GetAllUnidentifiedCyphersAsync()
        {
            var cyList = await _cypherContext.UnidentifiedCyphers.ToListAsync();

            return cyList;
        }
    }
}
