using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vkapitest.Models;

namespace vkapitest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatesController : ControllerBase
    {
        private readonly VkApiTestDbContext _context;

        public UserStatesController(VkApiTestDbContext context)
        {
            _context = context;
        }

        // GET: api/UserStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserState>>> GetUserStates()
        {
          if (_context.UserStates == null)
          {
              return NotFound();
          }
            return await _context.UserStates.ToListAsync();
        }

        // GET: api/UserStates/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserState>> GetUserState(int id)
        //{
        //  if (_context.UserStates == null)
        //  {
        //      return NotFound();
        //  }
        //    var userState = await _context.UserStates.FindAsync(id);

        //    if (userState == null)
        //    {
        //        return NotFound();
        //    }

        //    return userState;
        //}

        //// PUT: api/UserStates/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserState(int id, UserState userState)
        //{
        //    if (id != userState.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userState).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserStateExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/UserStates
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserState>> PostUserState(UserState userState)
        //{
        //  if (_context.UserStates == null)
        //  {
        //      return Problem("Entity set 'VkApiTestDbContext.UserStates'  is null.");
        //  }
        //    _context.UserStates.Add(userState);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserState", new { id = userState.Id }, userState);
        //}

        //// DELETE: api/UserStates/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserState(int id)
        //{
        //    if (_context.UserStates == null)
        //    {
        //        return NotFound();
        //    }
        //    var userState = await _context.UserStates.FindAsync(id);
        //    if (userState == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserStates.Remove(userState);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool UserStateExists(int id)
        {
            return (_context.UserStates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
