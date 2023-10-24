using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestVegastar.Models;

namespace TestVegastar.Controllers
{
    [Route("Usergroups")]
    [ApiController]
    public class UsergroupsController : ControllerBase
    {
        private readonly UsersdbContext _context;

        public UsergroupsController(UsersdbContext context)
        {
            _context = context;
        }

        // GET: api/Usergroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usergroup>>> GetUsergroups()
        {
          if (_context.Usergroups == null)
          {
              return NotFound();
          }
            return await _context.Usergroups.ToListAsync();
        }

        // GET: api/Usergroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usergroup>> GetUsergroup(int id)
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

            return usergroup;
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
        public async Task<ActionResult<Usergroup>> PostUsergroup(Usergroup usergroup)
        {
          if (_context.Usergroups == null)
          {
              return Problem("Entity set 'UsersdbContext.Usergroups'  is null.");
          }
            _context.Usergroups.Add(usergroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsergroup", new { id = usergroup.Id }, usergroup);
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
