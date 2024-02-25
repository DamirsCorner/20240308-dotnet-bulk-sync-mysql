using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BulkSyncMySql;

public static class BulkSyncAchievementsDbContextExtensions
{
    public static async Task BulkSyncAchievementsDefaultAsync(
        this AchievementsDbContext dbContext,
        IEnumerable<Achievement> achievements
    )
    {
        await dbContext.BulkInsertOrUpdateOrDeleteAsync(achievements);
    }

    public static async Task BulkSyncAchievementsSelectBeforeAsync(
        this AchievementsDbContext dbContext,
        IEnumerable<Achievement> achievements
    )
    {
        var existingAchievements = await dbContext
            .Achievements.Select(
                achievement => new Achievement(achievement.Name, string.Empty, 0, DateTime.Now)
            )
            .AsNoTracking()
            .ToListAsync();

        await dbContext.BulkInsertOrUpdateAsync(achievements);

        var achievementsToDelete = existingAchievements
            .ExceptBy(
                achievements.Select(achievement => achievement.Name),
                achievement => achievement.Name
            )
            .ToList();
        await dbContext.BulkDeleteAsync(achievementsToDelete);
    }

    public static async Task BulkSyncAchievementsSelectAfterAsync(
        this AchievementsDbContext dbContext,
        IEnumerable<Achievement> achievements
    )
    {
        await dbContext.BulkInsertOrUpdateAsync(achievements);

        var achievementsToDelete = await dbContext
            .Achievements.Where(achievement => !achievements.Contains(achievement))
            .Select(achievement => new Achievement(achievement.Name, string.Empty, 0, DateTime.Now))
            .AsNoTracking()
            .ToListAsync();
        await dbContext.BulkDeleteAsync(achievementsToDelete);
    }
}
