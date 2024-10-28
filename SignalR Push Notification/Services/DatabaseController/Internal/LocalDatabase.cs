using Microsoft.EntityFrameworkCore;
using SignalR_Push_Notification.Models.Notifications;

namespace SignalR_Push_Notification.Services.DatabaseController.Internal;

public class LocalDatabase : DbContext
{
    public LocalDatabase()
    {
        DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "local_database.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
    }

    public string DatabasePath { get; } = string.Empty;

    public DbSet<NotificationDb> Notifications { get; set; }
}