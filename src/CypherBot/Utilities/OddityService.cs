using CypherBot.Core.DataAccess.Repos;
using CypherBot.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherBot.Utilities
{
    public class OddityService
    {
        private CypherContext _cypherContext = null;
        private RandomGeneratorService _randomGenerator = null;

        public OddityService(CypherContext cypherContext, RandomGeneratorService randomGenerator)
        {
            _cypherContext = cypherContext;
            _randomGenerator = randomGenerator;
        }

        public async Task<List<Oddity>> GetAllOdditysAsync()
        {
            var oddList = await _cypherContext.Oddities.ToListAsync();

            return oddList;
        }

        public async Task<Oddity> GetRandomOddityAsync()
        {
            var oddList = await GetAllOdditysAsync();

            var i = _randomGenerator.GetRandom().Next(0, oddList.Count() - 1);

            return oddList[i];
        }

        public async Task<List<Oddity>> GetRandomOddityAsync(int numberOfCyphers)
        {
            var ls = new List<Oddity>();
            var rnd = _randomGenerator.GetRandom();

            for (int i = 0; i < numberOfCyphers; i++)
            {
                var OddityList = await GetAllOdditysAsync();

                ls.Add(OddityList[rnd.Next(1, OddityList.Count)]);
            }

            return ls;
        }
    }
}
