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
    [Route("v1/player")]
    public class PlayerController : ControllerBase
    {
        private readonly DataContext _context;
        public PlayerController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Player player)
        {
            Team team = await _context.Teams.FindAsync(player.Team.Id);
            if (team == null)
            {
                return StatusCode(404);
            }

            if (PlayerExists(player)){
                return StatusCode(400);
            }

            if (VerifyCountPlayerTeam(player)){
                player.Team = team;
                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            return StatusCode(404);
        }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _context.Players.Include(t => t.Team).Include(c => c.Team.Championship).ToListAsync());

        [HttpGet("byteam/{id}")]
        public async Task<IActionResult> GetByTeam([FromRoute] int id) 
        {
            return Ok(await _context.Players.Include(t => t.Team).Include(c => c.Team.Championship).Where(x => x.Team.Id == id).ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Player player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return StatusCode(404);
            }
            return Ok(player);
            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Player player)
        {
            if (!PlayerExistsById(player)){
                return StatusCode(400);
            }
            Console.WriteLine(player.Team.Id);

            _context.Players.Update(player);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Player player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return StatusCode(404);
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool PlayerExists(Player player) =>  _context.Players.Any(p => p.Name == player.Name);
        private bool PlayerExistsById(Player player) =>  _context.Players.Any(p => p.Id == player.Id);

        private bool VerifyCountPlayerTeam(Player player)
        {
            int peoples = _context.Players.Include(t => t.Team).Where(x => x.Team.Id == player.Team.Id).Count();;

            if(peoples < 4){
                return true;
            }        
            return false;
        }
    }
}