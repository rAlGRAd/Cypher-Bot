using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CypherBot.Core.DataAccess.Repos;
using CypherBot.Core.Models;

namespace CypherBot.Utilities
{
    public class DatabaseService
    {
        private CypherContext _cypherContext = null;

        public DatabaseService(CypherContext cypherContext)
        {
            _cypherContext = cypherContext;
        }

        /// <summary>
        /// Loads reference data from datafiles
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            try
            {
                #region DatabaseClear
                Console.WriteLine("Clearing Database...");

                Console.WriteLine("Clearing Cyphers.");
                _cypherContext.Cyphers.RemoveRange(_cypherContext.Cyphers.ToList());
                await _cypherContext.SaveChangesAsync();
                Console.WriteLine("Cyphers Cleared!");

                Console.WriteLine("Clearing Artifacts.");
                _cypherContext.Artifacts.RemoveRange(_cypherContext.Artifacts.ToList());
                await _cypherContext.SaveChangesAsync();
                Console.WriteLine("Artifacts Cleared!");

                Console.WriteLine("Clearing Oddities.");
                _cypherContext.Oddities.RemoveRange(_cypherContext.Oddities.ToList());
                await _cypherContext.SaveChangesAsync();
                Console.WriteLine("Oddities Cleared!");

                Console.WriteLine("Clearing Artifact Quirks.");
                _cypherContext.ArtifactQuirks.RemoveRange(_cypherContext.ArtifactQuirks.ToList());
                await _cypherContext.SaveChangesAsync();
                Console.WriteLine("Artifact Quirks Cleared!");
                #endregion

                //Cyphers
                #region CypherLoad
                Console.WriteLine("Getting Cyphers from cyphers.json");
                var cypherStrings = await Data.FileIO.GetFileString("cyphers");

                Console.WriteLine("Parsing Cyphers.");
                var cyphers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cypher>>(cypherStrings);

                Console.WriteLine($"{cyphers.Count()} cyphers found! Adding.");
                _cypherContext.AddRange(cyphers);
                await _cypherContext.SaveChangesAsync();
                #endregion

                //Artifacts
                #region ArtifactLoad
                Console.WriteLine("Getting Artifacts from artifacts.json");
                var artifactStrings = await Data.FileIO.GetFileString("artifacts");

                Console.WriteLine("Parsing Artifacts.");
                var artifacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Artifact>>(artifactStrings);

                Console.WriteLine($"{artifacts.Count()} artifacts found! Adding.");
                _cypherContext.AddRange(artifacts);
                await _cypherContext.SaveChangesAsync();
                #endregion

                //Oddities
                #region OddityLoad
                Console.WriteLine("Getting Oddities from oddities.json");
                var odditieStrings = await Data.FileIO.GetFileString("oddities");

                Console.WriteLine("Parsing Artifacts.");
                var oddities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Oddity>>(odditieStrings);

                Console.WriteLine($"{oddities.Count()} oddities found! Adding.");
                _cypherContext.AddRange(oddities);
                await _cypherContext.SaveChangesAsync();
                #endregion

                //Artifact Quirks
                #region ArtifactQuirkLoad
                Console.WriteLine("Getting Artifact Quirks from artifactquirks.json");
                var artifactQuirkStrings = await Data.FileIO.GetFileString("artifactquirks");

                Console.WriteLine("Parsing Artifacts.");
                var artifactQuirks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ArtifactQuirk>>(artifactQuirkStrings);

                Console.WriteLine($"{artifactQuirks.Count()} artifact quirks found! Adding.");
                _cypherContext.AddRange(artifactQuirks);
                await _cypherContext.SaveChangesAsync();
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
