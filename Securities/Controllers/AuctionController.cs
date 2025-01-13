using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

public class AuctionsController : ControllerBase
{
    private readonly DataDbContext _context;

    public AuctionsController(DataDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
    {
        return await _context.Auctions.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Auction>> GetAuction(int id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null)
            return NotFound();
        return auction;
    }

    [HttpPost]
    public async Task<ActionResult<Auction>> CreateAuction(Auction auction)
    {
        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuction), new { id = auction.AuctionID }, auction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuction(int id, Auction auction)
    {
        if (id != auction.AuctionID)
            return BadRequest();

        _context.Entry(auction).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuctionExists(id))
                return NotFound();
            throw;
        }
        return NoContent();
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(int id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null)
            return NotFound();

        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool AuctionExists(int id)
    {
        return _context.Auctions.Any(e => e.AuctionID == id);
    }
}