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

            GenerateKeys(typekey.Championship.Id);

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _context.TypeKeys.ToListAsync());

        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TypeKey typekey)
        {
            if (!TypeKeyExistsById(typekey)){
                return StatusCode(400);
            }
            _context.TypeKeys.Update(typekey);
            await _context.SaveChangesAsync();
            
            return StatusCode(204);
        }

        private int TypeKeyCountOne(TypeKey typekey) =>  _context.TypeKeys.Count();
        private bool TypeKeyExistsById(TypeKey typekey) =>  _context.TypeKeys.Any(t => t.Id == typekey.Id);
        
        private void GenerateKeys(int id){
           int aux=0;
           List<Key> keys = new List<Key>();

           var teams = _context.Teams.Where(c => c.Championship.Id == id).ToList();
           var typekey = _context.TypeKeys.First();

           var shortteams = teams.OrderBy(t => Guid.NewGuid()).ToList();
           int qtdteams = teams.Count();
           int qtdteamstokey = qtdteams / typekey.QuantityKey;

           for (int i = 1; i <= typekey.QuantityKey; i++){
            Key key = new Key(); 
            List<Team> team_list = new List<Team>();

            for (int c = 1; c <= qtdteamstokey; c++){
                var team = _context.Teams.Find(shortteams[aux].Id);
                team_list.Add(team);
                aux += 1;
            }
            string name = "Key " + i;
            key.Name = name;
            key.Teams = team_list;
            keys.Add(key);           
           }
            
            foreach (var k in keys)
            {
                _context.Keys.Add(k);
                _context.SaveChanges();
            }  
        }
    }
}