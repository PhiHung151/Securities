using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Securities.Models;
using Securities.Data;

namespace Securities.Controllers
{
    public class RoleController : Controller
    {
        private readonly DataDbContext _context;
        public RoleController(DataDbContext context) { 
            _context = context;
        }
        [HttpGet("{id}")]
        public IActionResult Index(string SearchString)
        {

            RoleModel roleModel = new RoleModel();
            roleModel.RoleDetailLists = new List<Role>();

            var data = from m in _context.Roles
                       select m;

            data = data.Where(m => m.deleted_at == null);
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.Name.Contains(SearchString) || m.Description.Contains(SearchString));
            }
            data.ToList();

            foreach (var item in data)
            {
                roleModel.RoleDetailLists.Add(new Role
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    Status = item.Status,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }
            ViewData["CurrentFilter"] = SearchString;
            return View(roleModel);
        }

        [HttpGet]
        public IActionResult Add()
        {

            Role role = new Role();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var roleData = new Role()
                    {
                        Name = role.Name,
                        Description = role.Description,
                        Status = role.Status,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    _context.Roles.Add(roleData);
                    _context.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(RoleController.Index), "Role");
            }
            return View(role);
        }

        [HttpGet]
        public IActionResult Update(int ID = 0)
        {
            Role role = new Role();
            var data = _context.Roles.Where(m => m.ID == ID).FirstOrDefault();
            if (data != null)
            {
                role.ID = data.ID;
                role.Name = data.Name;
                role.Description = data.Description;

                role.Status = data.Status;
            }

            return View(role);
        }

        [HttpPost]
        public IActionResult Update(Role role, IFormFile file)
        {
            try
            {

                var data = _context.Roles.Where(m => m.ID == role.ID).FirstOrDefault();


                if (data != null)
                {
                    // gan lai du lieu trong db bang du lieu tu form model gui len
                    data.Name = role.Name;
                    data.Description = role.Description;
                    data.Status = role.Status;

                    _context.SaveChanges(true);
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateStatus"] = false;
            }
            return RedirectToAction(nameof(RoleController.Index), "Role");
        }

        [HttpGet]
        public IActionResult Delete(int ID = 0)
        {
            try
            {
                var data = _context.Roles.Where(m => m.ID == ID).FirstOrDefault();
                if (data != null)
                {
                    data.deleted_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _context.SaveChanges(true);
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch
            {
                TempData["DeleteStatus"] = false;
            }
            return RedirectToAction(nameof(RoleController.Index), "Role");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
