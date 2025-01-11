using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Securities.Models;

namespace Securities.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(DbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: product
        public async Task<IActionResult> Index()
        {
            var properties = await _context.ProductID
                .Include(p => p.Seller)
                .Where(p => p.Status == "Available")
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return View(properties);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductID
                .Include(p => p.Seller)
                .Include(p => p.Auctions)
                .Include(p => p.LegalDocuments)
                .FirstOrDefaultAsync(m => m.productID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: product/Create
        [Authorize(Roles = "Seller")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create(Product model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    SellerID = int.Parse(User.FindFirst("UserID").Value),
                    Title = model.Title,
                    Description = model.Description,
                    Address = model.Address,
                    LandArea = model.LandArea,
                    Bedrooms = model.Bedrooms,
                    Bathrooms = model.Bathrooms,
                    YearBuilt = model.YearBuilt,
                    ProductType = model.ProductType,
                    City = model.City,
                    District = model.District,
                    Ward = model.Ward,
                    StartingPrice = model.StartingPrice,
                    ReservePrice = model.ReservePrice,
                    Status = "Pending",
                    CreatedDate = DateTime.Now
                };

                if (model.Images != null)
                {
                    product.Images = await SaveImages(model.Images);
                }

                if (model.Documents != null)
                {
                    product.Documents = await SaveDocuments(model.Documents);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private async Task<List<string>> SaveImages(IFormFileCollection files)
        {
            var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "properties");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileNames = new List<string>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); 
                    }

                    fileNames.Add(fileName); 
                }
            }
            return fileNames;
        }
    }
}