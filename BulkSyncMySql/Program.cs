using System.Globalization;
using BulkSyncMySql;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var achievementList = await LoadFromCsvAsync("achievements.csv");
var unlockedAchievements = PrepareAchievements(achievementList);

using var dbContext = CreateDbContext();

//await dbContext.BulkSyncAchievementsDefaultAsync(unlockedAchievements);

await dbContext.BulkSyncAchievementsSelectBeforeAsync(unlockedAchievements);

//await dbContext.BulkSyncAchievementsSelectAfterAsync(unlockedAchievements);

async Task<List<ImportedAchievement>> LoadFromCsvAsync(string filename)
{
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        Delimiter = ";"
    };
    using var reader = new StreamReader(filename);
    using var csv = new CsvReader(reader, config);
    return await csv.GetRecordsAsync<ImportedAchievement>().ToListAsync();
}

List<Achievement> PrepareAchievements(List<ImportedAchievement> importedAchievements)
{
    var random = new Random();

    var achievements = new List<Achievement>();
    foreach (var importedAchievement in importedAchievements)
    {
        if (random.NextDouble() < 0.01)
        {
            continue;
        }

        var achievement = new Achievement(
            importedAchievement.Name,
            importedAchievement.Description,
            importedAchievement.Gamerscore,
            DateTime.Now - TimeSpan.FromDays(random.NextDouble() * 365)
        );
        achievements.Add(achievement);
    }
    return achievements;
}

AchievementsDbContext CreateDbContext()
{
    string connectionString =
        "server=localhost;user=root;password=root;database=blogsample;AllowLoadLocalInfile=true";

    var serverVersion = ServerVersion.Create(8, 0, 35, ServerType.MySql);
    var optionsBuilder = new DbContextOptionsBuilder<AchievementsDbContext>()
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();

    return new AchievementsDbContext(optionsBuilder.Options);
}
