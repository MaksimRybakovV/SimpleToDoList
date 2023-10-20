using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"DataSource={Environment.CurrentDirectory}/Data/DataBase/SimpleTodoListDB.db");
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
    }
}
