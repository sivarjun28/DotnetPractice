using Exercise02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exercise02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LifetimeTestController : ControllerBase
    {
        private readonly IRequestContext _context1;
        private readonly IRequestContext _context2;
        private readonly IEmailService _email1;
        private readonly IEmailService _email2;
        private readonly IApplicationCache _cache;

        public LifetimeTestController(IRequestContext context1,
        IRequestContext context2,
        IEmailService email1,
        IEmailService email2,
        IApplicationCache cache)
        {
            _context1 = context1;
            _context2 = context2;
            _email1 = email1;
            _email2 = email2;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Test()
        {
            var result = new
            {
                scoped = new
                {
                    context1RequestId = _context1.RequestId,
                    context2RequestId = _context1.RequestId,
                    areSame = ReferenceEquals(_context1,_context2)
                },
                transient = new
                {
                    email1HashCode = _email1.GetHashCode(),
                    email2HashCode = _email2.GetHashCode(),
                    areSame = ReferenceEquals(_email1, _email2)
                },
                singleton = new
                {
                    cacheHashCode = _cache.GetHashCode(),
                    isConsistent = true
                }
            };
            return Ok(result);
        }
    }
}