using APITaskManagerCSharpDotNet.Enum;

namespace APITaskManagerCSharpDotNet.Models
{
    public class TaskListItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TaskItemStatus Status { get; set; } 

        public DateTime Data { get; set; }
    }
}