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
            //var users = await _context.Users
            //    .Include(b => b.Usergroup)
            //    .Include(p => p.Userstate)
            //    .ToListAsync();

            var users = await _context.Users
                .Select(u => new
                {
                    u.Userid,
                    u.Login,
                    u.Password,
                    u.Createddate,
                    u.Userstateid,
                    u.Usergroupid,
                    userstate = 
                        _context.Userstates
                        .Select(s => new { s.Id, s.Code, s.Description })
                        .Where(s => s.Id == u.Userstateid).ToList(),
                    usegroups = 
                        _context.Usergroups
                        .Select(g => new { g.Id, g.Code, g.Description })
                        .Where(g => g.Id == u.Usergroupid).ToList()
                }).ToListAsync();

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
        public async Task<IActionResult> PutUser(int id, [FromBody]UserDto userUpdated)
        {
            if (userUpdated == null)
                return BadRequest(ModelState);

            if (id != userUpdated.Userid)
                return BadRequest(ModelState);

            if (!UserExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdated);

            _context.Update(userMap);
            await _context.SaveChangesAsync();

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

        [HttpPut("Block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            if (_context.Users == null)
                return NotFound();

            if (!UserExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _context.Users.FindAsync(id);

            if (user.Userstateid == 1)
            {
                user.Usergroupid = 2;
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
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

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
