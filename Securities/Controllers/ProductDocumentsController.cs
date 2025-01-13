using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

public class ProductDocumentsController : ControllerBase
{
    private readonly DataDbContext _context;

    public ProductDocumentsController(DataDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDocument>>> GetProductDocuments()
    {
        return await _context.ProductDocuments.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDocument>> GetProductDocument(int id)
    {
        var ProductDocument = await _context.ProductDocuments.FindAsync(id);
        if (ProductDocument == null)
            return NotFound();
        return ProductDocument;
    }

    [HttpPost]
    public async Task<ActionResult<ProductDocument>> CreateProductDocument(ProductDocument ProductDocument)
    {
        _context.ProductDocuments.Add(ProductDocument);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProductDocument), new { id = ProductDocument.DocumentID }, ProductDocument);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductDocument(int id, ProductDocument ProductDocument)
    {
        if (id != ProductDocument.DocumentID)
            return BadRequest();

        _context.Entry(ProductDocument).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductDocumentExists(id))
                return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductDocument(int id)
    {
        var ProductDocument = await _context.ProductDocuments.FindAsync(id);
        if (ProductDocument == null)
            return NotFound();

        _context.ProductDocuments.Remove(ProductDocument);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool ProductDocumentExists(int id)
    {
        return _context.ProductDocuments.Any(e => e.DocumentID == id);
    }
}