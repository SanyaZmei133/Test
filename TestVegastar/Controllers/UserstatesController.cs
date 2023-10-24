﻿using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class UserstatesController : ControllerBase
    {
        private readonly UsersdbContext _context;
        private readonly IMapper _mapper;

        public UserstatesController(UsersdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Userstates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Userstate>>> GetUserstates()
        {
          if (_context.Userstates == null)
          {
              return NotFound();
          }
          var userstates = _mapper.Map<List<UserstateDto>>(await _context.Userstates.ToListAsync());

          return Ok(userstates);
        }

        // GET: api/Userstates/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Userstate>> GetUserstate(int id)
        {
          if (_context.Userstates == null)
          {
              return NotFound();
          }

            var userstate = _mapper.Map<UserstateDto>(await _context.Userstates.FindAsync(id));

            if (userstate == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userstate);
        }

        // PUT: api/Userstates/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserstate(int id, Userstate userstate)
        {
            if (id != userstate.Id)
            {
                return BadRequest();
            }

            _context.Entry(userstate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserstateExists(id))
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

        // POST: api/Userstates
        [HttpPost]
        public async Task<ActionResult<Userstate>> PostUserstate([FromBody] UserstateDto userstateCreate)
        {
          if (_context.Userstates == null)
          {
                return BadRequest(ModelState);
          }

            var userstate = _context.Userstates.ToList()
                 .Where(g => g.Code.Trim().ToUpper() == userstateCreate.Code.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (userstate != null)
            {
                ModelState.AddModelError("", "Userstate already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userstateMap = _mapper.Map<Userstate>(userstateCreate);

            _context.Userstates.Add(userstateMap);

            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        // DELETE: api/Userstates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserstate(int id)
        {
            if (_context.Userstates == null)
            {
                return NotFound();
            }
            var userstate = await _context.Userstates.FindAsync(id);
            if (userstate == null)
            {
                return NotFound();
            }

            _context.Userstates.Remove(userstate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserstateExists(int id)
        {
            return (_context.Userstates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
