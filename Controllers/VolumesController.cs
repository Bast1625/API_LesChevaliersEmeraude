using API_LesChevaliersEmeraude.Models;
using API_LesChevaliersEmeraude.Models.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LesChevaliersEmeraude.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolumesController : ControllerBase
    {
        private readonly ILogger<VolumesController> _logger;
        private readonly LesChevaliersEmeraudeContext _context;
        public VolumesController(ILogger<VolumesController> logger, LesChevaliersEmeraudeContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet()]
        public IActionResult GetVolumes()
        {
            try
            {
                IEnumerable<Volume> volumes = (from volume in _context.Tomes
                                              select new Volume
                                              {
                                                  Id = volume.IdTome,
                                                  Series = volume.IdSerieNavigation.Titre,
                                                  Number = volume.Numero ?? 0,
                                                  Title = volume.Titre,
                                                  ReleaseDate = volume.DateParution ?? DateOnly.MinValue,
                                                  Page = volume.Page ?? 0,
                                                  Isbn = volume.Isbn
                                              }).OrderBy(volume => volume.Id);

                if (volumes.Any())
                    return Ok(volumes);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetVolumeByID(int id)
        {
            try
            {
                Volume? volume = (from _volume in _context.Tomes
                                  where _volume.IdTome == id
                                  select new Volume
                                  {
                                      Id = _volume.IdTome,
                                      Series = _volume.IdSerieNavigation.Titre,
                                      Number = _volume.Numero ?? 0,
                                      Title = _volume.Titre,
                                      ReleaseDate = _volume.DateParution ?? DateOnly.MinValue,
                                      Page = _volume.Page ?? 0,
                                      Isbn = _volume.Isbn
                                  }).SingleOrDefault();

                if (volume == null)
                    return NoContent();

                return Ok(volume);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }
    }
}
