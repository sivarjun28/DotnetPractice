using Microsoft.AspNetCore.Mvc;
using Exercise03.Models;

namespace Exercise03.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        public static List<Category> categories = new List<Category>
        {
            new Category { Id = 1, Name = "Fiction", Description = "Fiction books" },
            new Category { Id = 2, Name = "Science", Description = "Science books" },
            new Category { Id = 3, Name = "Marvel", Description = "Marvel books" },
            new Category { Id = 4, Name = "History", Description = "History books" },
            new Category { Id = 5, Name = "Fiction", Description = "Fiction books" },
            new Category { Id = 6, Name = "Science", Description = "Science books" }
        };


        // GET api/categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            return Ok(categories);
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name))
                return BadRequest("Invalid data.");

            category.Id = categories.Max(c => c.Id) + 1;  // auto-increment
            categories.Add(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Category updatedCategory)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;
            return NoContent();
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            categories.Remove(category);
            return NoContent();
        }
    }
}