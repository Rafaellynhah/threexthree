using Microsoft.AspNetCore.Mvc;
using threexthree.Models;
using threexthree.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace threexthree.Controllers
{
    [ApiController]
    [Route("v1/game")]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;
        public GameController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> List(){
            return Ok(await _context.Games.Include(o => o.OneTeam)
            .ThenInclude(c => c.Championship)
            .Include(t => t.TwoTeam)
            .Include(k => k.Key)
            .ToListAsync());
        } 

        [HttpPost]
        public IActionResult Create([FromBody] string game) {
            if (key == "create"){
                GenerateGames();
                return StatusCode(201);
            }
            return StatusCode(404);
           
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Game game)
        {   
            if (!GameExistsById(game)){
                return StatusCode(400);
            }
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            
            return StatusCode(204);
        }

        private bool GameExistsById(Game game) =>  _context.Games.Any(g => g.Id == game.Id);
        
        private void GenerateGames(){
            
            var keys = _context.Keys.Include(t => t.Teams).ToList();

            foreach (var key in keys)
            { 
                for (int i = 0; i <  key.Teams.Count(); i++)
                { 
                    Game game = new Game();
                    if (i < (key.Teams.Count() - 1)){
                        game.OneTeam = key.Teams[i];
                        game.TwoTeam = key.Teams[i + 1];
                        game.Key = key;
                    }
                    else if (i == (key.Teams.Count() - 1)){
                        game.OneTeam = key.Teams[i];
                        game.TwoTeam = key.Teams[0];
                        game.Key = key;
                    }
                    _context.Games.Add(game);
                    _context.SaveChanges();
                }
                
            }
        }
    }
}