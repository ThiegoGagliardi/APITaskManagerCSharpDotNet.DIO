using Microsoft.EntityFrameworkCore;
using APITaskManagerCSharpDotNet.Models;

namespace APITaskManagerCSharpDotNet.Context
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options):base(options)
        {

        }

        public DbSet<TaskListItem> TaskList { get; set; }
        
    }
}