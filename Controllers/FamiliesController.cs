using API_LesChevaliersEmeraude.Models;
using API_LesChevaliersEmeraude.Models.Custom;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_LesChevaliersEmeraude.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamiliesController : ControllerBase
    {
        private readonly ILogger<FamiliesController> _logger;
        private readonly LesChevaliersEmeraudeContext _context;
        public FamiliesController(ILogger<FamiliesController> logger, LesChevaliersEmeraudeContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet()]
        public IActionResult GetFamilies()
        {
            try
            {
                var loaded = _context.RelationFamiliales
                .Include(relation => relation.IdEnfantNavigation)
                .Include(relation => relation.IdParent1Navigation)
                .Include(relation => relation.IdParent2Navigation)
                .ToList();

                IEnumerable<RelationFamiliale> relationsFamiliales = from family in _context.RelationFamiliales
                                                                     where family.IdParent1 != null || family.IdParent2 != null
                                                                     select family;

                if (!relationsFamiliales.Any())
                    return NoContent();

                IEnumerable<Family> families = relationsFamiliales
                    .Select(family => new Family
                    {
                        Parent1 = new SimpleCharacter
                        {
                            Id = family.IdParent1 ?? -1,
                            Name = family.IdParent1Navigation!.Nom
                        },

                        Parent2 = family.IdParent2 == null ? null : new SimpleCharacter
                        {
                            Id = family.IdParent2 ?? -1,
                            Name = family.IdParent2Navigation!.Nom
                        },

                        Children = new List<SimpleCharacter> {
                            new SimpleCharacter
                            {
                                Id = family.IdEnfant,
                                Name = family.IdEnfantNavigation!.Nom
                            }
                        }
                    })
                    .GroupBy(family => $"{family.Parent1.Id} & {family.Parent2?.Id}")
                    .Select(familyGroup => {
                        SimpleCharacter parent1 = familyGroup.First().Parent1;
                        SimpleCharacter? parent2 = familyGroup.First().Parent2;

                        SimpleCharacter[] children = familyGroup
                        .Select(family => family.Children.ToArray())
                        .Aggregate(Array.Empty<SimpleCharacter>(), (total, current) => total.Concat(current).ToArray())
                        .ToArray();

                        return new Family
                        {
                            Parent1 = parent1,
                            Parent2 = parent2,
                            Children = children
                        };
                    });

                return Ok(families);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data..." + ex.Message);
            }
        }
    }
}
