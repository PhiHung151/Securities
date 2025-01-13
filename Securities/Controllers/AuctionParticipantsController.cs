using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

 public class AuctionParticipantsController : ControllerBase
{
    private readonly DataDbContext _context;

    public AuctionParticipantsController(DataDbContext context)
    {
         _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuctionParticipant>>> GetAuctionParticipants()
    {
        return await _context.AuctionParticipants.ToListAsync();
    }

    [HttpGet("{auctionId}/{userId}")]
    public async Task<ActionResult<AuctionParticipant>> GetAuctionParticipant(int auctionId, int userId)
    {
        var auctionParticipant = await _context.AuctionParticipants
                .FindAsync(auctionId, userId);
        if (auctionParticipant == null)
            return NotFound();
        return auctionParticipant;
    }

    [HttpPost]
    public async Task<ActionResult<AuctionParticipant>> CreateAuctionParticipant(AuctionParticipant auctionParticipant)
    {
        _context.AuctionParticipants.Add(auctionParticipant);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (AuctionParticipantExists(auctionParticipant.AuctionID, auctionParticipant.UserID))
                return Conflict();
            throw;
        }

        return CreatedAtAction(
            nameof(GetAuctionParticipant),
            new { auctionId = auctionParticipant.AuctionID, userId = auctionParticipant.UserID },
            auctionParticipant);
    }

    [HttpPut("{auctionId}/{userId}")]
    public async Task<IActionResult> UpdateAuctionParticipant(int auctionId, int userId, AuctionParticipant auctionParticipant)
    {
        if (auctionId != auctionParticipant.AuctionID || userId != auctionParticipant.UserID)
            return BadRequest();

        _context.Entry(auctionParticipant).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuctionParticipantExists(auctionId, userId))
                 return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{auctionId}/{userId}")]
    public async Task<IActionResult> DeleteAuctionParticipant(int auctionId, int userId)
    {
        var auctionParticipant = await _context.AuctionParticipants.FindAsync(auctionId, userId);
            if (auctionParticipant == null)
            return NotFound();

        _context.AuctionParticipants.Remove(auctionParticipant);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuctionParticipantExists(int auctionId, int userId)
    {
        return _context.AuctionParticipants.Any(e => e.AuctionID == auctionId && e.UserID == userId);
    }

    // Additional methods for auction participation
    [HttpGet("auction/{auctionId}")]
    public async Task<ActionResult<IEnumerable<AuctionParticipant>>> GetParticipantsByAuction(int auctionId)
    {
        return await _context.AuctionParticipants
            .Where(ap => ap.AuctionID == auctionId)
            .Include(ap => ap.User)
            .ToListAsync();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<AuctionParticipant>>> GetParticipationsByUser(int userId)
    {
        return await _context.AuctionParticipants
            .Where(ap => ap.UserID == userId)
            .Include(ap => ap.Auction)
            .ToListAsync();
    }

    [HttpPost("{auctionId}/{userId}/deposit")]
    public async Task<IActionResult> UpdateDepositStatus(int auctionId, int userId, [FromBody] decimal depositAmount)
    {
        var participant = await _context.AuctionParticipants.FindAsync(auctionId, userId);
        if (participant == null)
            return NotFound();

        participant.HasPaidDeposit = true;
        participant.DepositAmount = depositAmount;
        participant.Status = "DepositPaid";

        await _context.SaveChangesAsync();
        return Ok(participant);
    }

    [HttpPost("{auctionId}/{userId}/approve")]
    public async Task<IActionResult> ApproveParticipation(int auctionId, int userId)
    {
        var participant = await _context.AuctionParticipants.FindAsync(auctionId, userId);
        if (participant == null)
            return NotFound();

        participant.Status = "Approved";
        await _context.SaveChangesAsync();
        return Ok(participant);
    }

    [HttpPost("{auctionId}/{userId}/reject")]
    public async Task<IActionResult> RejectParticipation(int auctionId, int userId)
    {
        var participant = await _context.AuctionParticipants.FindAsync(auctionId, userId);
        if (participant == null)
            return NotFound();

        participant.Status = "Rejected";
        await _context.SaveChangesAsync();
        return Ok(participant);
    }
}