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
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersdbContext _context;
        private readonly IMapper _mapper;

        public UsersController(UsersdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users
                .Include(b => b.Usergroup)
                .Include(p => p.Userstate)
                .ToListAsync();

            if (_context.Users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = _mapper.Map<UserDto>(await _context.Users.FindAsync(id));

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Userid)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserDto userCreate)
        {
      
            if (_context.Users.Any(o => o.Usergroupid == 1))
            {
                if (userCreate.Usergroupid == 1)
                    return BadRequest("Admin user already exist");
            }

            if (_context.Users == null)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.ToList()
                 .Where(g => g.Login.Trim().ToUpper() == userCreate.Login.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            _context.Users.Add(userMap);

            await _context.SaveChangesAsync();

            return Ok("Success"); ;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Userid == id)).GetValueOrDefault();
        }
    }
}
