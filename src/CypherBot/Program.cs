using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using CypherBot.Core.DataAccess.Repos;
using CypherBot.Utilities;
using CypherBot.Commands;

namespace CypherBot
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

            var Configuration = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<CypherBot>()
                .AddSingleton<IConfiguration>(Configuration)
                .AddSingleton<RandomGeneratorService>()
                .AddSingleton<DatabaseService>()
                .AddDbContext<CypherContext>(options => options.UseNpgsql(""))
                .BuildServiceProvider();

            // Use this if you want App_Data off your project root folder
            string baseDir = Directory.GetCurrentDirectory();

            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(baseDir, "DataFiles"));

            var cb = serviceProvider.GetService<CypherBot>();

            try
            {
                await cb.InitBot();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }

    public class CypherBot
    {
        private IConfiguration _configuration = null;
        private CypherContext _cypherContxt = null;
        private DatabaseService _databaseService = null;

        private DiscordClient discord;
        private CommandsNextExtension commands;
        private InteractivityExtension interactivity;


        public CypherBot(IConfiguration configuration, CypherContext cypherContext, DatabaseService databaseService)
        {

            _configuration = configuration;
            _cypherContxt = cypherContext;
            _databaseService = databaseService;

            var serviceProvider = new ServiceCollection()
            .AddSingleton<RandomGeneratorService>()
            .AddTransient<ArtifactService>()
            .AddTransient<CharacterService>()
            .AddTransient<CypherService>()
            .AddTransient<DatabaseService>()
            .AddTransient<OddityService>()
            .AddDbContext<CypherContext>(options => options.UseNpgsql(""))
            .BuildServiceProvider();

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Environment.GetEnvironmentVariable("DiscordAPIKey") ?? _configuration["token"],
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            var prefixes = new List<string>
            {
                _configuration["commandPrefix"]
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = prefixes,
                CaseSensitive = false,
                Services = serviceProvider
            });

            commands.RegisterCommands<DiceCommands>();
            commands.RegisterCommands<CypherCommands>();

            interactivity = discord.UseInteractivity(new InteractivityConfiguration() { });
        }

        public async Task InitBot()
        {
            //Initialize the database and migrate on start.
            _cypherContxt.Database.Migrate();

            if (_configuration["appInitialize"].ToLower() == "true")
            {
                Console.WriteLine("Initializing the database.");

                await _databaseService.InitializeDatabaseAsync();

                Console.WriteLine("Database Initialized, please set the appInitialize flag in appsettings.json to false in order to stop the database from being overridden again.");

                System.Threading.Thread.Sleep(3000);
            }

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
