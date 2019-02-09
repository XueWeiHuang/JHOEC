using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JHOEC.Models;
using Microsoft.AspNetCore.Http;

namespace JHOEC.Controllers
{
    public class JHVarietyController : Controller
    {
        private readonly OECContext _context;

        public JHVarietyController(OECContext context)
        {
            _context = context;
        }

        // GET: JHVariety
        public async Task<IActionResult> Index(int cropId)
        {
            
            if (Request.Query["cropId"].FirstOrDefault() != null && cropId!=0)
            {
                var _cropId = 0;
                HttpContext.Session.SetInt32(nameof(_cropId),cropId);

            }

            
            else if (HttpContext.Session.GetInt32(nameof(cropId))!=null)
            {
                
                cropId = Convert.ToInt32(HttpContext.Session.GetInt32(nameof(cropId)));
                //cropName = HttpContext.Session.GetString(nameof(cropName));


            }


            else
            {
                TempData["message"] = "fuck you ";
                return Redirect($"/JHCrop/Index/");
            }
            var oECContext = _context.Variety.Include(v => v.Crop).Where(v => v.CropId == cropId).OrderBy(v => v.Name);
            return View(await oECContext.ToListAsync());
            

        }

        // GET: JHVariety/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety
                .Include(v => v.Crop)
                .FirstOrDefaultAsync(m => m.VarietyId == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // GET: JHVariety/Create
        public IActionResult Create()
        {
            ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId");
            return View();
        }

        // POST: JHVariety/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VarietyId,CropId,Name")] Variety variety)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variety);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", variety.CropId);
            return View(variety);
        }

        // GET: JHVariety/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety.FindAsync(id);
            if (variety == null)
            {
                return NotFound();
            }
            ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", variety.CropId);
            return View(variety);
        }

        // POST: JHVariety/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VarietyId,CropId,Name")] Variety variety)
        {
            if (id != variety.VarietyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variety);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarietyExists(variety.VarietyId))
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
            ViewData["CropId"] = new SelectList(_context.Crop, "CropId", "CropId", variety.CropId);
            return View(variety);
        }

        // GET: JHVariety/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety
                .Include(v => v.Crop)
                .FirstOrDefaultAsync(m => m.VarietyId == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // POST: JHVariety/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variety = await _context.Variety.FindAsync(id);
            _context.Variety.Remove(variety);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarietyExists(int id)
        {
            return _context.Variety.Any(e => e.VarietyId == id);
        }
    }
}
