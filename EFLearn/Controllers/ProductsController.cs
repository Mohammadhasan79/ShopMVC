using EFLearn.Data;
using EFLearn.Models;
using EFLearn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EFLearn.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole>? _rolemanager;
        private readonly UserManager<IdentityUser>? _userManager;
        public ProductsController(ApplicationDbContext context, RoleManager<IdentityRole> RoleManagr, UserManager<IdentityUser> UserManager)
        {
            _context = context;
            _rolemanager = RoleManagr;
            _userManager = UserManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync());

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "User,Manager,Admin")]

        public IActionResult Create()
        {
            var vm = new Productselectlist
            {
                Category = _context.Categorys
                .AsNoTracking().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList()
            };
            //ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name");
            return View(vm);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,CategoryId")] Product product)

        {
            if (ModelState.IsValid)
            {
                if (!_context.Categorys.Any(c => c.Id == product.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", "دسته انتخاب شده وجود ندارد.");
                    return View(product);
                }
                product.CreateId = _userManager.GetUserId(User);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "User,Manager,Admin")]


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var UserId = _userManager.GetUserId(User);
            var roles = await _userManager.GetRolesAsync(user);
            if (product.CreateId != UserId && !roles.Contains("Admin"))
                return Forbid();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.Categorys.Any(c => c.Id == product.CategoryId))
                    {
                        ModelState.AddModelError("CategoryId", "دسته انتخاب شده وجود ندارد.");
                        ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys,"Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "User,Manager,Admin")]


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var UserId = _userManager.GetUserId(User);
            var roles = await _userManager.GetRolesAsync(user);
            if (product.CreateId != UserId && !roles.Contains("Admin"))
                return Forbid();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        //public async Task<IActionResult> SeedData()
        //{
        //    var products = await _context.Products.Include(p => p.Category).ToListAsync();
        //    ViewBag.CatList = new SelectList(_context.Products, "Id","Name",12);
        //    return View("Index" , products);
        //}
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<IActionResult> Filter()
        {
            var product = await _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
            return View("Index", product.Where(p => p.Price > 500 && p.Price < 2000).OrderBy(n => n.Price).ToList());
        }
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<IActionResult> Sort()
        {
            var product = await _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
            return View("Index", product.OrderByDescending(p => p.Price).ToList());
        }
        [Authorize]
        public IActionResult MyProduct()
        {
            var userid = _userManager.GetUserId(User);
            var product = _context.Products.Include(p => p.Category).Where(n => n.CreateId == userid).ToList();
            return View("Index", product);
        }
    }
}

