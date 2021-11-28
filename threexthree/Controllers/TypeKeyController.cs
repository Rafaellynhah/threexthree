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
    [Route("v1/type-key")]
    public class TypeKeyController : ControllerBase
    {
        private readonly DataContext _context;
        public TypeKeyController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TypeKey typekey)
        {
            Championship championship = await _context.Championships.FindAsync(typekey.Championship.Id);
            if (championship == null)
            {
                return StatusCode(404);
            }
            if (TypeKeyCountOne(typekey) >= 1){
                return StatusCode(400);
            }
            typekey.Championship = championship;
            _context.TypeKeys.Add(typekey);
            await _context.SaveChangesAsync();

            KeyController keycontroller = new KeyController(_context);
            keycontroller.GenerateKeys(typekey.Championship.Id);

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _context.TypeKeys.ToListAsync());

        private int TypeKeyCountOne(TypeKey typekey) =>  _context.TypeKeys.Count();
        
    }
}