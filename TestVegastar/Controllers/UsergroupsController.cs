using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestVegastar.Models;
using TestVegastar.ModelsDTO;

namespace TestVegastar.Controllers
{
    [Route("Usergroups")]
    [ApiController]
    public class UsergroupsController : ControllerBase
    {
        private readonly UsersdbContext _context;
        private readonly IMapper _mapper;

        public UsergroupsController(UsersdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Usergroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usergroup>>> GetUsergroups()
        {
          if (_context.Usergroups == null)
          {
              return NotFound();
          }
            var usergroups =  _mapper.Map<List<UsergroupDto>>( await _context.Usergroups.ToListAsync());

            return Ok(usergroups);
        }

        // GET: api/Usergroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usergroup>> GetUsergroup(int id)
        {
          if (_context.Usergroups == null)
          {
              return NotFound();
          }
            var usergroup = _mapper.Map<UsergroupDto>(await _context.Usergroups.FindAsync(id));

            if (usergroup == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(usergroup);
        }

        // PUT: api/Usergroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsergroup(int id, Usergroup usergroup)
        {
            if (id != usergroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(usergroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsergroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usergroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usergroup>> PostUsergroup([FromBody] UsergroupDto usergroupCreate)
        {
          if (_context.Usergroups == null)
          {
              return BadRequest(ModelState);
          }

            var usergroup = _context.Usergroups.ToList()
                .Where(g => g.Code.Trim().ToUpper() == usergroupCreate.Code.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (usergroup != null)
            {
                ModelState.AddModelError("", "Usergroup already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usergroupMap = _mapper.Map<Usergroup>(usergroupCreate);

            _context.Usergroups.Add(usergroupMap);
            
            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        // DELETE: api/Usergroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsergroup(int id)
        {
            if (_context.Usergroups == null)
            {
                return NotFound();
            }
            var usergroup = await _context.Usergroups.FindAsync(id);
            if (usergroup == null)
            {
                return NotFound();
            }

            _context.Usergroups.Remove(usergroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsergroupExists(int id)
        {
            return (_context.Usergroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
