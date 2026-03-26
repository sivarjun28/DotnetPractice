using Microsoft.AspNetCore.Mvc;
namespace Exercise05.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCodesController : ControllerBase
    {
        [HttpGet("ok")]
        public ActionResult GetOk()
        {
            return Ok(new { message = "200 Ok" });
        }

        [HttpPost("created")]
        public ActionResult PostCreated()
        {
            return Created("/api/resource/1", new { id = 1, name = "New Resource" });
        }
        [HttpDelete("no-content")]
        public ActionResult DeleteNoContent()
        {
            return NoContent();
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new { error = "Invalid Input" });
        }
        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized(new { error = "You are Unauthorized" });
        }

        [HttpGet("forbidden")]
        public ActionResult GetForbidden()
        {
            return Forbid();
        }
        [HttpGet("not-found/{id}")]
        public ActionResult GetNotFound(int id)
        {
            return NotFound(new { error = $"Resource {id} not found" });
        }

        // 409 Conflict
        [HttpPost("conflict")]
        public ActionResult PostConflict()
        {
            return Conflict(new { error = "Resource already exists" });
        }

        // 500 Internal Server Error
        [HttpGet("internal-server-error")]
        public ActionResult GetInternalServerError()
        {
            return StatusCode(500, new { error = "Internal server error" });
        }

    }
}