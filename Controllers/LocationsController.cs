using API_LesChevaliersEmeraude.Models;
using API_LesChevaliersEmeraude.Models.Custom;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_LesChevaliersEmeraude.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly LesChevaliersEmeraudeContext _context;
        public LocationsController(ILogger<LocationsController> logger, LesChevaliersEmeraudeContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet()]
        public IActionResult GetLocations()
        {
            try
            {
                IEnumerable<Location> locations = (from location in _context.Lieus
                                                  select new Location
                                                  {
                                                      Id = location.IdLieu,
                                                      Name = location.Nom,
                                                      Gentilic = location.Gentile
                                                  }).OrderBy(location => location.Id);

                if (locations.Any())
                    return Ok(locations);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetLocationByID(int id)
        {
            try
            {
                Location? location = (from _location in _context.Lieus
                                      where _location.IdLieu == id
                                      select new Location
                                      {
                                          Id = _location.IdLieu,
                                          Name = _location.Nom,
                                          Gentilic = _location.Gentile
                                      }).SingleOrDefault();

                if (location == null)
                    return NoContent();

                return Ok(location);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/royalty")]
        public IActionResult GetRoyalty(int id)
        {
            try
            {
                IEnumerable<Royalty> royalties = (from royalty in _context.Royautes
                                                  where royalty.IdLieu == id
                                                  select new Royalty
                                                  {
                                                      Location = new Location
                                                      {
                                                          Id = royalty.IdLieu,
                                                          Name = royalty.IdLieuNavigation.Nom,
                                                          Gentilic = royalty.IdLieuNavigation.Gentile
                                                      },

                                                      Sovereign1 = royalty.IdRoyaute1Navigation == null ? null : new SimpleCharacter
                                                      {
                                                          Id = royalty.IdRoyaute1Navigation.IdPersonnage,
                                                          Name = royalty.IdRoyaute1Navigation.Nom,
                                                          Gender = royalty.IdRoyaute1Navigation.Sexe.ToString(),
                                                          BirthPlace = royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation == null ? null : royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation.Nom,
                                                          HomePlace = royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation == null ? null : royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation.Nom
                                                      },

                                                      Sovereign2 = royalty.IdRoyaute2Navigation == null || royalty.IdRoyaute2 == 0 ? null : new SimpleCharacter
                                                      {
                                                          Id = royalty.IdRoyaute2Navigation.IdPersonnage,
                                                          Name = royalty.IdRoyaute2Navigation.Nom,
                                                          Gender = royalty.IdRoyaute2Navigation.Sexe.ToString(),
                                                          BirthPlace = royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation == null ? null : royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation.Nom,
                                                          HomePlace = royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation == null ? null : royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation.Nom
                                                      },

                                                      SuccessionOrder = royalty.SuccessionOrder
                                                  }).OrderBy(royalty => royalty.SuccessionOrder);

                if (royalties.Any())
                    return Ok(royalties.Select(royalty =>
                    {
                        string location = royalty.Location.Name;
                        string prefix = "aeiouyé".Contains(location[0].ToString().ToLower()) ? "d'" : "de ";
                        
                        string? status1 = royalty.Sovereign1 is null ? null : royalty.Sovereign1.Gender == "M" ? "Roi" : "Reine";
                        string? status2 = royalty.Sovereign2 is null ? null : royalty.Sovereign2.Gender == "M" ? "Roi" : "Reine";

                        string? name1 = royalty.Sovereign1?.Name;
                        string? location1 = royalty.Sovereign1?.BirthPlace is null ? location : royalty.Sovereign1.BirthPlace;
                        string? prefix1 = location1 is null ? null : "aeiouyé".Contains(location1[0].ToString().ToLower()) ? "d'" : "de ";

                        string? name2 = royalty.Sovereign2?.Name;
                        string? location2 = royalty.Sovereign2?.BirthPlace is null ? location : royalty.Sovereign2.BirthPlace;
                        string? prefix2 = location2 is null ? null : "aeiouyé".Contains(location2[0].ToString().ToLower()) ? "d'" : "de ";

                        string? sovereign1 = royalty.Sovereign1 is not null ? $"{status1} {name1} {prefix1}{location1}" : null;
                        string? sovereign2 = royalty.Sovereign2 is not null ? $"{status2} {name2} {prefix2}{location2}" : null;

                        return $"Souverains {prefix}{location}: {sovereign1}{(sovereign2 is not null ? $" & {sovereign2}" : null)}";
                    }));

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/royalty/{succession_order}")]
        public IActionResult GetRoyaltyBySucessionOrder(int id, int succession_order)
        {
            try
            {
                Royalty? royalty = (from _royalty in _context.Royautes
                                    where _royalty.IdLieu == id && _royalty.SuccessionOrder == succession_order
                                    select new Royalty
                                    {
                                        Location = new Location
                                        {
                                            Id = _royalty.IdLieu,
                                            Name = _royalty.IdLieuNavigation.Nom,
                                            Gentilic = _royalty.IdLieuNavigation.Gentile
                                        },

                                        Sovereign1 = _royalty.IdRoyaute1Navigation == null ? null : new SimpleCharacter
                                        {
                                            Id = _royalty.IdRoyaute1Navigation.IdPersonnage,
                                            Name = _royalty.IdRoyaute1Navigation.Nom,
                                            Gender = _royalty.IdRoyaute1Navigation.Sexe.ToString(),
                                            BirthPlace = _royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation == null ? null : _royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation.Nom,
                                            HomePlace = _royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation == null ? null : _royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation.Nom
                                        },

                                        Sovereign2 = _royalty.IdRoyaute2Navigation == null || _royalty.IdRoyaute2 == 0 ? null : new SimpleCharacter
                                        {
                                            Id = _royalty.IdRoyaute2Navigation.IdPersonnage,
                                            Name = _royalty.IdRoyaute2Navigation.Nom,
                                            Gender = _royalty.IdRoyaute2Navigation.Sexe.ToString(),
                                            BirthPlace = _royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation == null ? null : _royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation.Nom,
                                            HomePlace = _royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation == null ? null : _royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation.Nom
                                        },

                                        SuccessionOrder = _royalty.SuccessionOrder
                                    }).SingleOrDefault();

                if (royalty == null)
                    return NoContent();

                string location = royalty.Location.Name;
                string prefix = "aeiouyé".Contains(location[0].ToString().ToLower()) ? "d'" : "de ";

                string? status1 = royalty.Sovereign1 is null ? null : royalty.Sovereign1.Gender == "M" ? "Roi" : "Reine";
                string? status2 = royalty.Sovereign2 is null ? null : royalty.Sovereign2.Gender == "M" ? "Roi" : "Reine";

                string? name1 = royalty.Sovereign1?.Name;
                string? location1 = royalty.Sovereign1?.BirthPlace is null ? location : royalty.Sovereign1.BirthPlace;
                string? prefix1 = location1 is null ? null : "aeiouyé".Contains(location1[0].ToString().ToLower()) ? "d'" : "de ";

                string? name2 = royalty.Sovereign2?.Name;
                string? location2 = royalty.Sovereign2?.BirthPlace is null ? location : royalty.Sovereign2.BirthPlace;
                string? prefix2 = location2 is null ? null : "aeiouyé".Contains(location2[0].ToString().ToLower()) ? "d'" : "de ";

                string? sovereign1 = royalty.Sovereign1 is not null ? $"{status1} {name1} {prefix1}{location1}" : null;
                string? sovereign2 = royalty.Sovereign2 is not null ? $"{status2} {name2} {prefix2}{location2}" : null;

                return Ok($"Souverains {prefix}{location}: {sovereign1}{(sovereign2 is not null ? $" & {sovereign2}" : null)}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/royalty/earliest")]
        public IActionResult GetEarliestRoyalty(int id)
        {
            try
            {
                Royalty? royalty = (from _royalty in _context.Royautes
                                    where _royalty.IdLieu == id
                                    select new Royalty
                                    {
                                        Location = new Location
                                        {
                                            Id = _royalty.IdLieu,
                                            Name = _royalty.IdLieuNavigation.Nom,
                                            Gentilic = _royalty.IdLieuNavigation.Gentile
                                        },

                                        Sovereign1 = _royalty.IdRoyaute1Navigation == null ? null : new SimpleCharacter
                                        {
                                            Id = _royalty.IdRoyaute1Navigation.IdPersonnage,
                                            Name = _royalty.IdRoyaute1Navigation.Nom,
                                            Gender = _royalty.IdRoyaute1Navigation.Sexe.ToString(),
                                            BirthPlace = _royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation == null ? null : _royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation.Nom,
                                            HomePlace = _royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation == null ? null : _royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation.Nom
                                        },

                                        Sovereign2 = _royalty.IdRoyaute2Navigation == null || _royalty.IdRoyaute2 == 0 ? null : new SimpleCharacter
                                        {
                                            Id = _royalty.IdRoyaute2Navigation.IdPersonnage,
                                            Name = _royalty.IdRoyaute2Navigation.Nom,
                                            Gender = _royalty.IdRoyaute2Navigation.Sexe.ToString(),
                                            BirthPlace = _royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation == null ? null : _royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation.Nom,
                                            HomePlace = _royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation == null ? null : _royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation.Nom
                                        },

                                        SuccessionOrder = _royalty.SuccessionOrder
                                    }).OrderBy(_royalty => _royalty.SuccessionOrder).LastOrDefault();

                if (royalty == null)
                    return NoContent();

                string location = royalty.Location.Name;
                string prefix = "aeiouyé".Contains(location[0].ToString().ToLower()) ? "d'" : "de ";

                string? status1 = royalty.Sovereign1 is null ? null : royalty.Sovereign1.Gender == "M" ? "Roi" : "Reine";
                string? status2 = royalty.Sovereign2 is null ? null : royalty.Sovereign2.Gender == "M" ? "Roi" : "Reine";

                string? name1 = royalty.Sovereign1?.Name;
                string? location1 = royalty.Sovereign1?.BirthPlace is null ? location : royalty.Sovereign1.BirthPlace;
                string? prefix1 = location1 is null ? null : "aeiouyé".Contains(location1[0].ToString().ToLower()) ? "d'" : "de ";

                string? name2 = royalty.Sovereign2?.Name;
                string? location2 = royalty.Sovereign2?.BirthPlace is null ? location : royalty.Sovereign2.BirthPlace;
                string? prefix2 = location2 is null ? null : "aeiouyé".Contains(location2[0].ToString().ToLower()) ? "d'" : "de ";

                string? sovereign1 = royalty.Sovereign1 is not null ? $"{status1} {name1} {prefix1}{location1}" : null;
                string? sovereign2 = royalty.Sovereign2 is not null ? $"{status2} {name2} {prefix2}{location2}" : null;

                return Ok($"Souverains {prefix}{location}: {sovereign1}{(sovereign2 is not null ? $" & {sovereign2}" : null)}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/royalty/latest")]
        public IActionResult GetLatestRoyalty(int id)
        {
            try
            {
                Royalty? royalty = (from _royalty in _context.Royautes
                                    where _royalty.IdLieu == id
                                    select new Royalty
                                    {
                                        Location = new Location
                                        {
                                            Id = _royalty.IdLieu,
                                            Name = _royalty.IdLieuNavigation.Nom,
                                            Gentilic = _royalty.IdLieuNavigation.Gentile
                                        },

                                        Sovereign1 = _royalty.IdRoyaute1Navigation == null ? null : new SimpleCharacter
                                        {
                                            Id = _royalty.IdRoyaute1Navigation.IdPersonnage,
                                            Name = _royalty.IdRoyaute1Navigation.Nom,
                                            Gender = _royalty.IdRoyaute1Navigation.Sexe.ToString(),
                                            BirthPlace = _royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation == null ? null : _royalty.IdRoyaute1Navigation.IdLieuOrigineNavigation.Nom,
                                            HomePlace = _royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation == null ? null : _royalty.IdRoyaute1Navigation.IdLieuResidenceNavigation.Nom
                                        },

                                        Sovereign2 = _royalty.IdRoyaute2Navigation == null || _royalty.IdRoyaute2 == 0 ? null : new SimpleCharacter
                                        {
                                            Id = _royalty.IdRoyaute2Navigation.IdPersonnage,
                                            Name = _royalty.IdRoyaute2Navigation.Nom,
                                            Gender = _royalty.IdRoyaute2Navigation.Sexe.ToString(),
                                            BirthPlace = _royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation == null ? null : _royalty.IdRoyaute2Navigation.IdLieuOrigineNavigation.Nom,
                                            HomePlace = _royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation == null ? null : _royalty.IdRoyaute2Navigation.IdLieuResidenceNavigation.Nom
                                        },

                                        SuccessionOrder = _royalty.SuccessionOrder
                                    }).OrderBy(_royalty => _royalty.SuccessionOrder).FirstOrDefault();

                if (royalty == null)
                    return NoContent();

                string location = royalty.Location.Name;
                string prefix = "aeiouyé".Contains(location[0].ToString().ToLower()) ? "d'" : "de ";

                string? status1 = royalty.Sovereign1 is null ? null : royalty.Sovereign1.Gender == "M" ? "Roi" : "Reine";
                string? status2 = royalty.Sovereign2 is null ? null : royalty.Sovereign2.Gender == "M" ? "Roi" : "Reine";

                string? name1 = royalty.Sovereign1?.Name;
                string? location1 = royalty.Sovereign1?.BirthPlace is null ? location : royalty.Sovereign1.BirthPlace;
                string? prefix1 = location1 is null ? null : "aeiouyé".Contains(location1[0].ToString().ToLower()) ? "d'" : "de ";

                string? name2 = royalty.Sovereign2?.Name;
                string? location2 = royalty.Sovereign2?.BirthPlace is null ? location : royalty.Sovereign2.BirthPlace;
                string? prefix2 = location2 is null ? null : "aeiouyé".Contains(location2[0].ToString().ToLower()) ? "d'" : "de ";

                string? sovereign1 = royalty.Sovereign1 is not null ? $"{status1} {name1} {prefix1}{location1}" : null;
                string? sovereign2 = royalty.Sovereign2 is not null ? $"{status2} {name2} {prefix2}{location2}" : null;

                return Ok($"Souverains {prefix}{location}: {sovereign1}{(sovereign2 is not null ? $" & {sovereign2}" : null)}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }
    }
}
