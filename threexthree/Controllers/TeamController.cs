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
    [Route("v1/team")]
    public class TeamController : ControllerBase
    {
        private readonly DataContext _context;
        public TeamController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Team team)
        {
            Championship championship = await _context.Championships.FindAsync(team.Championship.Id);
            if (championship == null)
            {
                return StatusCode(404);
            }

            if (TeamExists(team)){
                return StatusCode(400);
            }
            
            team.Championship = championship;
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            
            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _context.Teams.Include(c => c.Championship).ToListAsync());

        [HttpGet("bychampionship/{id}")]
        public async Task<IActionResult> GetByChampionship([FromRoute] int id) 
        {
            return Ok(await _context.Teams.Include(c => c.Championship).Where(x => x.Championship.Id == id).ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Team team = await _context.Teams.Include(c => c.Championship).FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return StatusCode(404);
            }
            return Ok(team);
            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Team team)
        {   
            Championship championship = await _context.Championships.FindAsync(team.Championship.Id);
            if (championship == null)
            {
                return StatusCode(404);
            }
            if (!TeamExistsById(team)){
                return StatusCode(400);
            }
            team.Championship = championship;
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return StatusCode(404);
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool TeamExists(Team team) =>  _context.Teams.Any(t => t.Name == team.Name);
        private bool TeamExistsById(Team team) =>  _context.Teams.Any(t => t.Id == team.Id);
    }
}