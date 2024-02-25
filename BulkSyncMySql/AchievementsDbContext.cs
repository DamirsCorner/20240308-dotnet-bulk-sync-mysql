using Microsoft.EntityFrameworkCore;

namespace BulkSyncMySql;

public class AchievementsDbContext(DbContextOptions<AchievementsDbContext> options)
    : DbContext(options)
{
    public DbSet<Achievement> Achievements => Set<Achievement>();
}
