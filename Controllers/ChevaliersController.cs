using API_LesChevaliersEmeraude.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace API_LesChevaliersEmeraude.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChevaliersController : ControllerBase
    {
        private readonly ILogger<ChevaliersController> _logger;
        private readonly LesChevaliersEmeraudeContext _context;
        public ChevaliersController(ILogger<ChevaliersController> logger, LesChevaliersEmeraudeContext context)
        {
            _logger = logger;
            _context = context;
        }
    }
}