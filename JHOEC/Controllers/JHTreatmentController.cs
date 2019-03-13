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
    public class JHTreatmentController : Controller
    {
        private readonly OECContext _context;

        public JHTreatmentController(OECContext context)
        {
            _context = context;
        }

        // GET: JHTreatment
        public async Task<IActionResult> Index(int? plotId, string farmName)
        {
            var oECContext = _context.Treatment.Include(t => t.Plot).AsQueryable();
            if (plotId!=null && plotId!=0)
            {
                HttpContext.Session.SetString(nameof(plotId), plotId.ToString());


            }
            else if (HttpContext.Session.GetString(nameof(plotId)) !=null)
            {
                plotId = Convert.ToInt32(HttpContext.Session.GetString(nameof(plotId)));
                var treatmentFertilizers = await oECContext.Include(t => t.TreatmentFertilizer).Where(t => t.PlotId == plotId).ToListAsync();
                foreach (var treatmentFertilizer in treatmentFertilizers)
                {
                    
                    var fertilizerName = treatmentFertilizer.TreatmentFertilizer.OrderBy(t => t.FertilizerName).Select(t => t.FertilizerName).ToList();
                    if (fertilizerName.Any())
                    {
                        treatmentFertilizer.Name = String.Join(" + ", fertilizerName);
                    }
                    else
                    {
                        treatmentFertilizer.Name = "no fertilizer";
                    }
                    await Edit(treatmentFertilizer.TreatmentId, treatmentFertilizer);
                }                
            }
            else
            {
                TempData["message"] = "Please select a plot to view treatments";
                return RedirectToAction("Index", "JHPlot");
            }
            if (farmName == null || farmName == "")
            {
                HttpContext.Session.SetString(nameof(farmName), _context.Plot.Include(p => p.Farm).SingleOrDefault(p => p.PlotId == plotId).Farm.Name);
            }
            else
            {
                HttpContext.Session.SetString(nameof(farmName), farmName);
            }
            oECContext = oECContext.Where(t=>t.PlotId==plotId).OrderBy(t=>t.Name);
            return View(await oECContext.ToListAsync());
        }
        // GET: JHTreatment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatment
                .Include(t => t.Plot)
                .FirstOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("treatmentName", _context.Treatment.SingleOrDefault(t => t.TreatmentId == id).Name);

            return View(treatment);
        }

        // GET: JHTreatment/Create
        public IActionResult Create()
        {
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId");
            return View();
        }

        // POST: JHTreatment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TreatmentId,Name,PlotId,Moisture,Yield,Weight")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId", treatment.PlotId);
            return View(treatment);
        }

        // GET: JHTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatment.FindAsync(id);
            if (treatment == null)
            {
                return NotFound();
            }
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId", treatment.PlotId);
            HttpContext.Session.SetString("treatmentName", _context.Treatment.SingleOrDefault(t => t.TreatmentId == id).Name);
            return View(treatment);
        }

        // POST: JHTreatment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatmentId,Name,PlotId,Moisture,Yield,Weight")] Treatment treatment)
        {
            if (id != treatment.TreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentExists(treatment.TreatmentId))
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
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId", treatment.PlotId);
            HttpContext.Session.SetString("treatmentName", _context.Treatment.SingleOrDefault(t => t.TreatmentId == id).Name);

            return View(treatment);
        }

        // GET: JHTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatment
                .Include(t => t.Plot)
                .FirstOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // POST: JHTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatment.FindAsync(id);
            _context.Treatment.Remove(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatment.Any(e => e.TreatmentId == id);
        }
    }
}
