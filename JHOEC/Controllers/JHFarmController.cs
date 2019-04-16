using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JHOEC.Models;

namespace JHOEC.Controllers
{
    public class JHFarmController : Controller
    {
        private readonly OECContext _context;
        public JHFarmController(OECContext context)
        {
            _context = context;
        }

        // GET: JHFarm
        public async Task<IActionResult> Index()
        {
            var oECContext = _context.Farm.Include(f => f.ProvinceCodeNavigation).OrderBy(f => f.Name);
            return View(await oECContext.ToListAsync());
        }

        // GET: JHFarm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm
                .Include(f => f.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }
            return View(farm);
        }

        // GET: JHFarm/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: JHFarm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmId,Name,Address,Town,County,ProvinceCode,PostalCode,HomePhone,CellPhone,Email,Directions,DateJoined,LastContactDate")] Farm farm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    if (await TryUpdateModelAsync(farm))
                    {
                        _context.Add(farm);
                        await _context.SaveChangesAsync();
                        TempData["message"] = "Farm creation sucessfull";
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"error creating farm {ex.GetBaseException().Message}");
            }

            Create();
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", farm.ProvinceCode);
            return View(farm);
        }

        // GET: JHFarm/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm.FindAsync(id);
            if (farm == null)
            {
                return NotFound();
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(p => p.Name), "ProvinceCode", "Name", farm.ProvinceCode);
            return View(farm);
        }

        // POST: JHFarm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmId,Name,Address,Town,County,ProvinceCode,PostalCode,HomePhone,CellPhone,Email,Directions,DateJoined,LastContactDate")] Farm farm)
        {
            // to confirm this part
            if (id != farm.FarmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //should just pass in the object instead of context
                    if (await TryUpdateModelAsync(farm))
                    {
                        _context.Update(farm);
                        TempData["message"] = "Update sucessfull";
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException de)
                {
                    if (!FarmExists(farm.FarmId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", $"error updating farm information: {de.GetBaseException().Message}");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await Edit(id);
            ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(p => p.Name), "ProvinceCode", "Name", farm.ProvinceCode);
            return View(farm);
        }

        // GET: JHFarm/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm
                .Include(f => f.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }

            return View(farm);
        }

        // POST: JHFarm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var farm = await _context.Farm.FindAsync(id);
                _context.Farm.Remove(farm);
                await _context.SaveChangesAsync();
                TempData["message"] = "Delete completed sucessfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["message"] = $"error inserting new order: {ex.GetBaseException().Message}";
                return await Delete(id);
            }
        }
        private bool FarmExists(int id)
        {
            return _context.Farm.Any(e => e.FarmId == id);
        }
    }
}
