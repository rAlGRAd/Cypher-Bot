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
    public class ArtifactService
    {
        private CypherContext _cypherContext = null;
        private RandomGeneratorService _randomGenerator = null;

        public ArtifactService(CypherContext cypherContext, RandomGeneratorService randomGenerator)
        {
            _cypherContext = cypherContext;
            _randomGenerator = randomGenerator;
        }

        public async Task<List<Artifact>> GetAllArtifactsAsync()
        {

            var artList = await _cypherContext.Artifacts.ToListAsync();

            return artList;
        }

        public async Task<Artifact> GetRandomArtifactAsync(string genre = "")
        {
            var artList = await GetAllArtifactsAsync();

            if (genre != "")
            {
                artList = artList.Where(x => x.Genre == genre).ToList();
            }

            var i = _randomGenerator.GetRandom().Next(0, artList.Count() - 1);

            return artList[i];
        }

        public async Task<List<Artifact>> GetRandomArtifactAsync(int numberOfCyphers, string genre = "")
        {
            var ls = new List<Artifact>();
            var rnd = _randomGenerator.GetRandom();

            for (int i = 0; i < numberOfCyphers; i++)
            {
                var artifactList = await GetAllArtifactsAsync();

                if (genre != "")
                {
                    artifactList = artifactList.Where(x => x.Genre == genre).ToList();
                }

                ls.Add(artifactList[rnd.Next(1, artifactList.Count)]);
            }

            return ls;
        }

        public async Task<List<ArtifactQuirk>> GetAllArtifactQuirksAsync()
        {
            var artList = await _cypherContext.ArtifactQuirks.ToListAsync();

            return artList;
        }

        public async Task<ArtifactQuirk> GetRandomArtifactQuirkAsync()
        {
            var artQuiList = await GetAllArtifactQuirksAsync();

            var i = _randomGenerator.GetRandom().Next(0, artQuiList.Count() - 1);

            return artQuiList[i];
        }
    }
}
