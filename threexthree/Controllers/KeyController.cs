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
    [Route("v1/key")]
    public class KeyController : ControllerBase
    {
        private readonly DataContext _context;
        public KeyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> List(){
          return Ok(await _context.Keys
            .Include(t => t.Teams)
            .ToListAsync());  
        } 
    }
}