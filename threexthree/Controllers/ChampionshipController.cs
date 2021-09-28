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
    [Route("v1/championship")]
    public class ChampionshipController : ControllerBase
    {
        private readonly DataContext _context;
        public ChampionshipController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Championship championship)
        {
            if (ChampionshipExists(championship)){
                return StatusCode(400);
            }
            _context.Championships.Add(championship);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }


        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _context.Championships.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Championship championship = await _context.Championships.FindAsync(id);
            if (championship == null)
            {
                return StatusCode(404);
            }
            return Ok(championship);
            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Championship championship)
        {
            if (!ChampionshipExistsById(championship)){
                return StatusCode(400);
            }
            _context.Championships.Update(championship);
            await _context.SaveChangesAsync();
            
            return StatusCode(204);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Championship championship = await _context.Championships.FindAsync(id);
            if (championship == null)
            {
                return StatusCode(404);
            }

            _context.Championships.Remove(championship);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool ChampionshipExists(Championship championship) =>  _context.Championships.Any(c => c.Name == championship.Name);
        private bool ChampionshipExistsById(Championship championship) =>  _context.Championships.Any(c => c.Id == championship.Id);
    }
}