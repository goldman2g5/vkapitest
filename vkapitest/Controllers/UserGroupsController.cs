﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vkapitest.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace vkapitest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private readonly VkApiTestDbContext _context;
        private readonly ILogger<UserGroupsController> _logger;

        public UserGroupsController(VkApiTestDbContext context, ILogger<UserGroupsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/UserGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups()
        {
          if (_context.UserGroups == null)
          {
              return NotFound();
          }
            return await _context.UserGroups.ToListAsync();
        }

        //// GET: api/UserGroups/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserGroup>> GetUserGroup(int id)
        //{
        //  if (_context.UserGroups == null)
        //  {
        //      return NotFound();
        //  }
        //    var userGroup = await _context.UserGroups.FindAsync(id);

        //    if (userGroup == null)
        //    {
        //        return NotFound();
        //    }

        //    return userGroup;
        //}

        //// PUT: api/UserGroups/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserGroup(int id, UserGroup userGroup)
        //{
        //    if (id != userGroup.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userGroup).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserGroupExists(id))
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

        //// POST: api/UserGroups
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserGroup>> PostUserGroup(UserGroup userGroup)
        //{
        //  if (_context.UserGroups == null)
        //  {
        //      return Problem("Entity set 'VkApiTestDbContext.UserGroups'  is null.");
        //  }
        //    _context.UserGroups.Add(userGroup);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserGroup", new { id = userGroup.Id }, userGroup);
        //}

        //// DELETE: api/UserGroups/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserGroup(int id)
        //{
        //    if (_context.UserGroups == null)
        //    {
        //        return NotFound();
        //    }
        //    var userGroup = await _context.UserGroups.FindAsync(id);
        //    if (userGroup == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserGroups.Remove(userGroup);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool UserGroupExists(int id)
        {
            return (_context.UserGroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
