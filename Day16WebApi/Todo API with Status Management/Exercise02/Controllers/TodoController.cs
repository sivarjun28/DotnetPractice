using Microsoft.AspNetCore.Mvc;
using Exercise02.Models;

namespace Exercise02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        public static List<Todo> todos = new()
        {
            new Todo { Id = 1, Title = "Finish project", Description = "Complete the API project", DueDate = DateTime.Now.AddDays(2), Status = TodoStatus.Pending, Priority = Priority.High },
            new Todo { Id = 2, Title = "Buy groceries", Description = "Milk, Bread, Eggs", DueDate = DateTime.Now.AddDays(-1), Status = TodoStatus.InProgress, Priority = Priority.Medium },
            new Todo { Id = 3, Title = "Call John", Description = "Discuss meeting agenda", DueDate = DateTime.Now.AddDays(1), Status = TodoStatus.Pending, Priority = Priority.Low }
        };
        private static int nextId = todos.Count + 1;

        // GET api/todos
        [HttpGet]
        public IActionResult GetAll([FromQuery] TodoStatus? status, [FromQuery] Priority? priority)
        {
            var query = todos.AsEnumerable();
            if (status.HasValue) query = query.Where(t => t.Status == status.Value);
            if (priority.HasValue) query = query.Where(t => t.Priority == priority.Value);
            return Ok(query);

        }
        // GET api/todos/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            return todo == null ? NotFound() : Ok(todo);
        }

        // GET api/todos/overdue
        [HttpGet("overdue")]
        public IActionResult GetOverDue()
        {
            var overdue = todos
                            .Where(t => t.DueDate.HasValue &&
                                        t.DueDate.Value < DateTime.Now &&
                                        t.Status != TodoStatus.Completed);
            return Ok(overdue);
        }

        // GET api/todos/status/{status}
        [HttpGet("status/{status}")]
        public IActionResult GetByStatus(TodoStatus status)
        {
            var filtered = todos.Where(t => t.Status == status);
            return Ok(filtered);
        }

        // POST api/todos
        [HttpPost]
        public IActionResult Create([FromBody] CreateTodoDto dto)
        {
            var todo = new Todo
            {
                Id = nextId++,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority
            };
            todos.Add(todo);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        // PUT api/todos/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateTodoDto dto)
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();

            todo.Title = dto.Title;
            todo.Description = dto.Description;
            todo.DueDate = dto.DueDate;
            todo.Priority = dto.Priority;

            return NoContent();
        }

        // PATCH api/todos/{id}/status
        [HttpPatch("{id}/status")]

        public IActionResult Updatestatus(int id, [FromQuery] TodoStatus status)
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();
            todo.Status = status;
            if (status == TodoStatus.Completed) todo.CompletedAt = DateTime.Now;

            return NoContent();
        }
        //// POST api/todos/{id}/complete
        [HttpPost("{id}/complete")]
        public IActionResult MarkAsComplete(int id)
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();

            todo.Status = TodoStatus.Completed;
            todo.CompletedAt = DateTime.Now;

            return NoContent();
        }

        // DELETE api/todos/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();

            todos.Remove(todo);
            return NoContent();
        }

    }
}
