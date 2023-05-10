using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class UsersController : ControllerBase
    {
        private readonly VkApiTestDbContext _context;
        private readonly ILogger<UsersController> _logger;

        private static List<User> LastUsers = new List<User>();

        private readonly UserGroup adminGroup;
        private readonly UserState activeState;

        public UsersController(VkApiTestDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
            adminGroup = _context.UserGroups.FirstOrDefault(x => x.Code == "Admin");
            activeState = _context.UserStates.FirstOrDefault(x => x.Code == "Active");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
           
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
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

            return user;
        }

        [HttpGet("ids")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByIds([FromQuery] string userIds)
        {
            if (string.IsNullOrEmpty(userIds))
            {
                return BadRequest("No user IDs provided.");
            }

            var ids = userIds.Split(',').Select(int.Parse).ToList();

            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

            return users;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPutModel user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            User adminUser = await _context.Users.FirstOrDefaultAsync(x => x.UserGroupId == adminGroup.Id);
            if (user.UserGroupId == adminGroup.Id & adminUser != null & adminUser.Id != user.Id)
            {
                return Problem("Admin user already exists");
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

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserPostModel user)
        { 
          if (_context.Users == null)
          {
              return Problem("Entity set 'VkApiTestDbContext.Users'  is null.");
          }

            if (user.UserGroupId == adminGroup.Id)
          {
                if(_context.Users.Any(x => x.UserGroupId == adminGroup.Id))
                {
                    return Problem("Admin user already exists");
                }
          }

          if (LastUsers.Any(x => x.Login == user.Login))
          {
               return Problem("User with same login was added in last 5 seconds");
          }

            LastUsers.Add(user);
            new Thread(new ThreadStart(async () => await Task.Delay(5000)
            .ContinueWith(t => LastUsers.Remove(user)))).Start();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
