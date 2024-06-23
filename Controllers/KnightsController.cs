using API_LesChevaliersEmeraude.Models;
using API_LesChevaliersEmeraude.Models.Custom;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using System.Diagnostics.Metrics;

namespace API_LesChevaliersEmeraude.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KnightsController : ControllerBase
    {
        private readonly ILogger<KnightsController> _logger;
        private readonly LesChevaliersEmeraudeContext _context;
        public KnightsController(ILogger<KnightsController> logger, LesChevaliersEmeraudeContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet()]
        public ActionResult<SimpleCharacter> GetKnights()
        {
            try
            {
                IEnumerable<SimpleKnight> knights = (from knight in _context.Chevaliers
                                                     join character in _context.Personnages
                                                     on knight.IdChevalier equals character.IdPersonnage
                                                     select new SimpleKnight
                                                     {
                                                        Id = character.IdPersonnage,
                                                        Name = character.Nom,
                                                        Gender = character.Sexe.ToString(),
                                                        BirthPlace = character.IdLieuOrigineNavigation == null ? null : character.IdLieuOrigineNavigation.Nom,
                                                        HomePlace = character.IdLieuResidenceNavigation == null ? null : character.IdLieuResidenceNavigation.Nom,
                                                        Generation = knight.Generation
                                                     }).OrderBy(knight => knight.Generation).OrderBy(knight => knight.Id);

                if (knights.Any())
                    return Ok(knights);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("tree")]
        public IActionResult GetTree()
        {
            try
            {
                int[] rootMastersIDs = GetRootMasters();

                Node<SimpleKnight>[] knightTrees = new Node<SimpleKnight>[rootMastersIDs.Length];

                for(int rootMasterIndex = 0; rootMasterIndex < knightTrees.Length; rootMasterIndex++)
                {
                    int rootMasterID = rootMastersIDs[rootMasterIndex];

                    Node<SimpleKnight>? newTree = GenerateKnightTree(rootMasterID);


                    if (newTree == null)
                        continue;

                    knightTrees[rootMasterIndex] = newTree;
                }

                if (knightTrees.All(knightTree => knightTree is null))
                    return NoContent();

                return Ok(knightTrees);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/branch/full")]
        public IActionResult GetBranch(int id)
        {
            try
            {
                Node<SimpleKnight>? tree = GenerateKnightTree(id);

                if (tree == null)
                    return NoContent();

                return Ok(tree);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/branch/fan")]
        public IActionResult GetFanBranch()
        {
            try
            {
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/branch/upper_half")]
        public IActionResult GetUpperHalfBranch(int id)
        {
            try
            {
                Node<SimpleKnight>? knightTree = GenerateKnightTree(id);
                
                if(knightTree == null)
                    return NoContent();

                int[] pathFromRootMaster = GetPathFromRootMaster(id).Concat(new int[] { -1 }).ToArray();
                
                for(int nodeToRootMasterIndex = 0; nodeToRootMasterIndex < pathFromRootMaster.Length - 1; nodeToRootMasterIndex++)
                {
                    int masterID = pathFromRootMaster[nodeToRootMasterIndex];
                    int squireToKeepID = pathFromRootMaster[nodeToRootMasterIndex + 1];

                    Node<SimpleKnight>? master = knightTree.Find(knight => knight.Id == masterID);

                    if (master == null)
                        continue;

                    master.Children.RemoveAll(child => child.Data.Id != squireToKeepID);
                }
                
                return Ok(knightTree);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/branch/lower_half")]
        public IActionResult GetLowerHalfBranch(int id)
        {
            try
            {
                Node<SimpleKnight>? knightTree = GenerateKnightTree(id);

                if (knightTree == null)
                    return NoContent();

                return Ok(knightTree.Find(knight => knight.Id == id));
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/master")]
        public IActionResult GetMasters(int id)
        {
            try
            {
                IEnumerable<SimpleKnight> masters = (from knight in _context.Chevaliers
                                                    where knight.IdChevalier == id
                                                    select knight.IdMaitres
                                                    .Select(master => new SimpleKnight
                                                    {
                                                        Id = master.IdChevalier,
                                                        Name = master.IdChevalierNavigation.Nom,
                                                        Gender = master.IdChevalierNavigation.Sexe.ToString(),
                                                        BirthPlace = master.IdChevalierNavigation.IdLieuOrigineNavigation == null ? null : master.IdChevalierNavigation.IdLieuOrigineNavigation.Nom,
                                                        HomePlace = master.IdChevalierNavigation.IdLieuResidenceNavigation == null ? null : master.IdChevalierNavigation.IdLieuResidenceNavigation.Nom,
                                                        Generation = master.Generation
                                                    })).Single();

                if (masters.Any())
                    return Ok(masters);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data... " + ex.Message);
            }
        }

        [HttpGet("{id}/squires")]
        public IActionResult GetSquires(int id)
        {
            try
            {
                IEnumerable<SimpleKnight> squires = (from knight in _context.Chevaliers
                                                     where knight.IdChevalier == id
                                                     select knight.IdEcuyers
                                                     .Select(squire => new SimpleKnight
                                                     {
                                                         Id = squire.IdChevalier,
                                                         Name = squire.IdChevalierNavigation.Nom,
                                                         Gender = squire.IdChevalierNavigation.Sexe.ToString(),
                                                         BirthPlace = squire.IdChevalierNavigation.IdLieuOrigineNavigation == null ? null : squire.IdChevalierNavigation.IdLieuOrigineNavigation.Nom,
                                                         HomePlace = squire.IdChevalierNavigation.IdLieuResidenceNavigation == null ? null : squire.IdChevalierNavigation.IdLieuResidenceNavigation.Nom,
                                                         Generation = squire.Generation
                                                     })).Single();

                if (squires.Any())
                    return Ok(squires);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while fetching data...  " + ex.Message);
            }
        }

        #region Helper Methods
        private int[] GetRootMasters()
        {
            IEnumerable<Chevalier> rootMasters = from knight in _context.Chevaliers
                                                  where !knight.IdMaitres.Any()
                                                  select knight;

            return rootMasters.Select(master => master.IdChevalier).ToArray();
        }
        private int GetRootMaster(int id)
        {
            Chevalier? knight = (from _knight in _context.Chevaliers
                                 where _knight.IdChevalier == id
                                 select _knight).SingleOrDefault()
                                 ?? throw new ArgumentOutOfRangeException($"A knight with ID {id} doesn't exist.");

            Chevalier master = knight;
            while (master.IdMaitres.Any())
                master = master.IdMaitres.First();

            return master!.IdChevalier;
        }
        private int[] GetPathFromRootMaster(int id)
        {
            var loaded = _context.Chevaliers.Include(knight => knight.IdMaitres).ToList();

            Chevalier? knight = (from _knight in _context.Chevaliers
                                 where _knight.IdChevalier == id
                                 select _knight).SingleOrDefault()
                                 ?? throw new ArgumentOutOfRangeException($"A knight with ID {id} doesn't exist.");

            List<int> pathFromRootMaster = new List<int>() { id };

            Chevalier master = knight;
            while (master.IdMaitres.Any())
            {
                master = master.IdMaitres.First();

                pathFromRootMaster.Add(master.IdChevalier);
            }

            pathFromRootMaster.Reverse();

            return pathFromRootMaster.ToArray();
        }

        private Node<SimpleKnight>? GenerateKnightTree(int id)
        {
            var loaded = _context.Chevaliers
                .Include(chevalier => chevalier.IdChevalierNavigation)
                .Include(chevalier => chevalier.IdMaitres)
                .Include(chevalier => chevalier.IdEcuyers)
                .Include(chevalier => chevalier.IdChevalierNavigation.IdLieuOrigineNavigation)
                .Include(chevalier => chevalier.IdChevalierNavigation.IdLieuResidenceNavigation)
                .ToList();

            Chevalier? chevalier = (from knight in _context.Chevaliers
                                    where knight.IdChevalier == id
                                    select knight).SingleOrDefault();

            if (chevalier == null)
                return null;

            int rootMasterID = GetRootMaster(id);
            Chevalier rootMaster = (from knight in _context.Chevaliers
                                    where knight.IdChevalier == rootMasterID
                                    select knight).Single();

            Node<SimpleKnight> rootNode = new Node<SimpleKnight>(
                    new SimpleKnight
                    {
                        Id = rootMaster.IdChevalier,
                        Name = rootMaster.IdChevalierNavigation.Nom,
                        Gender = rootMaster.IdChevalierNavigation.Sexe.ToString(),
                        BirthPlace = rootMaster.IdChevalierNavigation.IdLieuOrigineNavigation?.Nom,
                        HomePlace = rootMaster.IdChevalierNavigation.IdLieuResidenceNavigation?.Nom,
                        Generation = rootMaster.Generation
                    }
                );

            CreateSquireBranch(rootMaster, rootNode);

            return rootNode;
        }

        private void CreateSquireBranch(Chevalier master, Node<SimpleKnight> node)
        {
            if (master.IdEcuyers.Count == 0)
                return;

            foreach (Chevalier squire in master.IdEcuyers)
            {
                SimpleKnight newChild = new SimpleKnight
                {
                    Id = squire.IdChevalier,
                    Name = squire.IdChevalierNavigation.Nom,
                    Gender = squire.IdChevalierNavigation.Sexe.ToString(),
                    BirthPlace = squire.IdChevalierNavigation.IdLieuOrigineNavigation?.Nom,
                    HomePlace = squire.IdChevalierNavigation.IdLieuResidenceNavigation?.Nom,
                    Generation = squire.Generation
                };

                Node<SimpleKnight> newNode = node.Add(newChild);

                CreateSquireBranch(squire, newNode);
            }
        }
        #endregion
    }
}