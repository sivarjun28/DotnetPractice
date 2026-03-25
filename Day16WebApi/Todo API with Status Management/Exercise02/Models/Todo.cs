namespace Exercise02.Models
{
    public enum TodoStatus
    {
        Pending,
        InProgress,
        Completed
    }
    public enum Priority
    {
        Low,
        Medium,
        High
    }
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public TodoStatus Status { get; set; } = TodoStatus.Pending;
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }

     public class CreateTodoDto
    {
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
    }
}