using System.ComponentModel.DataAnnotations;

namespace BulkSyncMySql;

public class Achievement(string name, string description, int gamerscore, DateTime unlockTime)
{
    [Key]
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int Gamerscore { get; set; } = gamerscore;
    public DateTime UnlockTime { get; set; } = unlockTime;
}
