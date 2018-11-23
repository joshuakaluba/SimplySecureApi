using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Web.Controllers
{
    public class ModuleStateChangesController : Controller
    {
        private readonly SimplySecureDataContext _context;

        public ModuleStateChangesController(SimplySecureDataContext context)
        {
            _context = context;
        }

        // GET: ModuleStateChanges
        public async Task<IActionResult> Index()
        {
            var simplySecureDataContext 
                = _context.ModuleStateChanges
                    .Include(m => m.Module)
                        .OrderByDescending(m=>m.DateCreated);

            return View(await simplySecureDataContext.ToListAsync());
        }

        // GET: ModuleStateChanges/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleStateChange = await _context.ModuleStateChanges
                .Include(m => m.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleStateChange == null)
            {
                return NotFound();
            }

            return View(moduleStateChange);
        }

        // GET: ModuleStateChanges/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id");
            return View();
        }

        // POST: ModuleStateChanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("State,ModuleId,Id,DateCreated")] ModuleStateChange moduleStateChange)
        {
            if (ModelState.IsValid)
            {
                moduleStateChange.Id = Guid.NewGuid();
                _context.Add(moduleStateChange);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id", moduleStateChange.ModuleId);
            return View(moduleStateChange);
        }

        // GET: ModuleStateChanges/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleStateChange = await _context.ModuleStateChanges.FindAsync(id);
            if (moduleStateChange == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id", moduleStateChange.ModuleId);
            return View(moduleStateChange);
        }

        // POST: ModuleStateChanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("State,ModuleId,Id,DateCreated")] ModuleStateChange moduleStateChange)
        {
            if (id != moduleStateChange.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduleStateChange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleStateChangeExists(moduleStateChange.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id", moduleStateChange.ModuleId);
            return View(moduleStateChange);
        }

        // GET: ModuleStateChanges/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleStateChange = await _context.ModuleStateChanges
                .Include(m => m.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleStateChange == null)
            {
                return NotFound();
            }

            return View(moduleStateChange);
        }

        // POST: ModuleStateChanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var moduleStateChange = await _context.ModuleStateChanges.FindAsync(id);
            _context.ModuleStateChanges.Remove(moduleStateChange);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleStateChangeExists(Guid id)
        {
            return _context.ModuleStateChanges.Any(e => e.Id == id);
        }
    }
}
