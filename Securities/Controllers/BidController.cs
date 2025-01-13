using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

public class BidsController : Controller
{
    private readonly DataDbContext _context;

    public BidsController(DataDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bid>>> GetBids()
    {
        return await _context.Bids.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bid>> GetBid(int id)
    {
        var bid = await _context.Bids.FindAsync(id);
        if (bid == null)
            return NotFound();
        return bid;
    }

    [HttpPost]
    public async Task<ActionResult<Bid>> CreateBid(Bid bid)
    {
        _context.Bids.Add(bid);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBid), new { id = bid.BidID }, bid);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBid(int id, Bid bid)
    {
    if (id != bid.BidID)
        return BadRequest();

    _context.Entry(bid).State = EntityState.Modified;
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!BidExists(id))
            return NotFound();
            throw;
        }
        return NoContent();
    }

    private bool BidExists(int id)
    {
        return _context.Bids.Any(e => e.BidID == id);
    }
}