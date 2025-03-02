using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

public class NotificationsController : ControllerBase
{
    private readonly DataDbContext _context;

    public NotificationsController(DataDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
    {
        return await _context.Notifications.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Notification>> GetNotification(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null)
            return NotFound();
        return notification;
    }

    [HttpPost]
    public async Task<ActionResult<Notification>> CreateNotification(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationID }, notification);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNotification(int id, Notification notification)
    {
        if (id != notification.NotificationID)
            return BadRequest();

        _context.Entry(notification).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NotificationExists(id))
                return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null)
            return NotFound();

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool NotificationExists(int id)
    {
        return _context.Notifications.Any(e => e.NotificationID == id);
    }
}