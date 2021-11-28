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

        public void GenerateKeys(int id){
           int aux=0;

           var teams = _context.Teams.Where(c => c.Championship.Id == id).ToList();
           var typekey = _context.TypeKeys.First();

           var shortteams = teams.OrderBy(t => Guid.NewGuid()).ToList();
           int qtdteams = teams.Count();
           int qtdteamstokey = qtdteams / typekey.QuantityKey;

           for (int i = 1; i <= typekey.QuantityKey; i++){

                Key key = new Key(); 
                List<Team> team_list = new List<Team>();
                for (int c = 1; c <= qtdteamstokey; c++){
                    team_list.Add(shortteams[aux]);
                    aux += 1;
                }
                key.Name = "Key " + i;
                key.Teams = team_list;
    
                _context.Keys.Add(key);
                _context.SaveChanges();         
           }
            
        }
    }
}