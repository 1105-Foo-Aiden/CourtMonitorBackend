using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CourtMonitorBackend.Services.Context
{
    public class DataContext : DbContext
    {
        public DbSet<UserModel> UserInfo { get; set; }
        public DbSet<EventModel> EventInfo { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}