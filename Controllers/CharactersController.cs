using API_LesChevaliersEmeraude.Models;
using API_LesChevaliersEmeraude.Models.Custom;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_LesChevaliersEmeraude.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ILogger<CharactersController> _logger;
        private readonly LesChevaliersEmeraudeContext _context;
        public CharactersController(ILogger<CharactersController> logger, LesChevaliersEmeraudeContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("")]
        public ActionResult<SimpleCharacter> GetCharacters(
            [FromQuery] string? name = null, 
            [FromQuery] char? gender = null, 
            [FromQuery] string? birthPlace = null, 
            [FromQuery] string? homePlace = null) 
        {
            try
            {
                IEnumerable<SimpleCharacter> characters = (from character in _context.Personnages
                                                           where
                                                           (name == null || character.Nom.ToLower().Contains(name.ToLower())) &&
                                                           (gender == null || character.Sexe == gender) &&
                                                           (birthPlace == null || character.IdLieuOrigineNavigation!.Nom == birthPlace) &&
                                                           (homePlace == null || character.IdLieuResidenceNavigation!.Nom == homePlace)
                                                           select new SimpleCharacter
                                                           {
                                                               Id = character.IdPersonnage,
                                                               Name = character.Nom,
                                                               Gender = character.Sexe.ToString(),
                                                               BirthPlace = character.IdLieuOrigineNavigation == null ? null : character.IdLieuOrigineNavigation.Nom,
                                                               HomePlace = character.IdLieuResidenceNavigation == null ? null : character.IdLieuResidenceNavigation.Nom
                                                           }).OrderBy(character => character.Id).Where(character => character.Id > 0);

                if (characters.Any())
                    return Ok(characters);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data..." + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetCharacterById(int id)
        {
            Character? characterToFetch = (from character in _context.Personnages
                                           where character.IdPersonnage == id && id > 0
                                           select new Character
                                           {
                                               Id = character.IdPersonnage,
                                               Name = character.Nom ?? "",
                                               Gender = character.Sexe.ToString() ?? "",

                                               BirthPlace = new Location
                                               {
                                                   Id = character.IdLieuOrigine ?? -1,
                                                   Name = character.IdLieuOrigineNavigation!.Nom,
                                                   Gentilic = character.IdLieuOrigineNavigation.Gentile ?? ""
                                               },

                                               HomePlace = new Location
                                               {
                                                   Id = character.IdLieuResidence ?? -1,
                                                   Name = character.IdLieuResidenceNavigation!.Nom,
                                                   Gentilic = character.IdLieuResidenceNavigation.Gentile ?? ""
                                               },

                                               FirstAppearanceVolume = character.IdTomeApparitionNavigation == null ? null : new Volume
                                               {
                                                   Id = character.IdTomeApparitionNavigation!.IdTome,
                                                   Series = character.IdTomeApparitionNavigation.IdSerieNavigation.Titre,
                                                   Number = character.IdTomeApparitionNavigation.Numero ?? -1,
                                                   Title = character.IdTomeApparitionNavigation.Titre,
                                                   Page = character.IdTomeApparitionNavigation.Page ?? 0,
                                                   ReleaseDate = character.IdTomeApparitionNavigation.DateParution ?? DateOnly.MinValue,
                                                   ReleaseLocation = character.IdTomeApparitionNavigation.LieuParution,
                                                   Isbn = character.IdTomeApparitionNavigation.Isbn,
                                                   Author = character.IdTomeApparitionNavigation.IdAuteurNavigation!.Nom,
                                                   Editor = character.IdTomeApparitionNavigation.IdEditeurNavigation!.Nom
                                               },

                                               DeathVolume = character.IdTomeDecesNavigation == null ? null : new Volume
                                               {
                                                   Id = character.IdTomeDecesNavigation!.IdTome,
                                                   Series = character.IdTomeDecesNavigation.IdSerieNavigation.Titre,
                                                   Number = character.IdTomeDecesNavigation.Numero ?? -1,
                                                   Title = character.IdTomeDecesNavigation.Titre,
                                                   Page = character.IdTomeDecesNavigation.Page ?? 0,
                                                   ReleaseDate = character.IdTomeDecesNavigation.DateParution ?? DateOnly.MinValue,
                                                   ReleaseLocation = character.IdTomeDecesNavigation.LieuParution,
                                                   Isbn = character.IdTomeDecesNavigation.Isbn,
                                                   Author = character.IdTomeDecesNavigation.IdAuteurNavigation!.Nom,
                                                   Editor = character.IdTomeDecesNavigation.IdEditeurNavigation!.Nom
                                               },
                                           }).SingleOrDefault();

            if (characterToFetch == null)
                return NoContent();

            return Ok(characterToFetch);
        }
        
        [HttpGet("{location}")]
        public IActionResult GetCharacterByLocation(string location)
        {
            try
            {
                IEnumerable<SimpleCharacter> characters = from character in _context.Personnages
                                                          where character.IdLieuOrigineNavigation == null ? true : character.IdLieuOrigineNavigation.Nom == location
                                                          select new SimpleCharacter
                                                          {
                                                              Id = character.IdPersonnage,
                                                              Name = character.Nom,
                                                              Gender = character.Sexe.ToString(),
                                                              BirthPlace = character.IdLieuOrigineNavigation == null ? null : character.IdLieuOrigineNavigation.Nom,
                                                              HomePlace = character.IdLieuResidenceNavigation == null ? null : character.IdLieuResidenceNavigation.Nom
                                                          };

                if (characters.Any())
                    return Ok(characters);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data..." + ex.Message);
            }
        }
    }
}