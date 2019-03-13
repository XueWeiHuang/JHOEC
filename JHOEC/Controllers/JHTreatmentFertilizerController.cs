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
    public class JHTreatmentFertilizerController : Controller
    {
        private readonly OECContext _context;

        public JHTreatmentFertilizerController(OECContext context)
        {
            _context = context;
        }

        // GET: JHTreatmentFertilizer
        public async Task<IActionResult> Index(int? treatmentId)
        {
            if (treatmentId!=null && treatmentId!=0 )
            {
                HttpContext.Session.SetString(nameof(treatmentId), treatmentId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(treatmentId))!=null)
            {
                treatmentId = Convert.ToInt32(HttpContext.Session.GetString(nameof(treatmentId)));
            }
            else
            {
                TempData["message"] = "Please select a treatment to view compositions";
                return RedirectToAction("Index", "JHTreatment");
            }
            HttpContext.Session.SetString("treatmentName", _context.Treatment.SingleOrDefault(t => t.TreatmentId == treatmentId).Name);
            ViewData["treatmentName"] = HttpContext.Session.GetString("treatmentName");
            var oECContext = _context.TreatmentFertilizer.Include(t => t.FertilizerNameNavigation).Include(t => t.Treatment).Where(t=>t.TreatmentId==treatmentId).OrderBy(t=>t.FertilizerName);
            return View(await oECContext.ToListAsync());
        }

        // GET: JHTreatmentFertilizer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentFertilizer = await _context.TreatmentFertilizer
                .Include(t => t.FertilizerNameNavigation)
                .Include(t => t.Treatment)
                .FirstOrDefaultAsync(m => m.TreatmentFertilizerId == id);
            if (treatmentFertilizer == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("fertilizerName", _context.TreatmentFertilizer.SingleOrDefault(f => f.TreatmentFertilizerId == id).FertilizerName);
            return View(treatmentFertilizer);
        }

        // GET: JHTreatmentFertilizer/Create
        public IActionResult Create()
        {
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer.OrderBy(f=>f.FertilizerName), "FertilizerName", "FertilizerName");
            ViewData["TreatmentId"] = Convert.ToInt32(HttpContext.Session.GetString("treatmentId"));
            ViewData["Liquid"] = new SelectList(_context.Fertilizer.OrderBy(a => a.FertilizerName), "FertilizerName", "Liquid");
            return View();
        }

        // POST: JHTreatmentFertilizer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TreatmentFertilizerId,TreatmentId,FertilizerName,RatePerAcre,RateMetric")] TreatmentFertilizer treatmentFertilizer)
        {
            if (ModelState.IsValid)
            {
                //treatmentFertilizer.RateMetric = GetMetric(treatmentFertilizer.FertilizerName);
                _context.Add(treatmentFertilizer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer.OrderBy(f=>f.FertilizerName), "FertilizerName", "FertilizerName", treatmentFertilizer.FertilizerName);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId", treatmentFertilizer.TreatmentId);
            ViewData["Liquid"] = new SelectList(_context.Fertilizer.OrderBy(a => a.FertilizerName), "FertilizerName", "Liquid");

            return View(treatmentFertilizer);
        }
        //public string GetMetric(string fertilizerName)
        //{
        //    var status = _context.Fertilizer.SingleOrDefault(f => f.FertilizerName == fertilizerName).Liquid;
        //    if (status == true)
        //    {
        //        return "Gal";
        //    }
        //    else
        //        return "LB";
        //}

        // GET: JHTreatmentFertilizer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentFertilizer = await _context.TreatmentFertilizer.FindAsync(id);
            if (treatmentFertilizer == null)
            {
                return NotFound();
            }
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer.OrderBy(f=>f.FertilizerName), "FertilizerName", "FertilizerName", treatmentFertilizer.FertilizerName);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId", treatmentFertilizer.TreatmentId);
            HttpContext.Session.SetString("fertilizerName", _context.TreatmentFertilizer.SingleOrDefault(f => f.TreatmentFertilizerId == id).FertilizerName);
           
            return View(treatmentFertilizer);
        }

        // POST: JHTreatmentFertilizer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatmentFertilizerId,TreatmentId,FertilizerName,RatePerAcre,RateMetric")] TreatmentFertilizer treatmentFertilizer)
        {
            if (id != treatmentFertilizer.TreatmentFertilizerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatmentFertilizer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentFertilizerExists(treatmentFertilizer.TreatmentFertilizerId))
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
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer.OrderBy(f=>f.FertilizerName), "FertilizerName", "FertilizerName", treatmentFertilizer.FertilizerName);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId", treatmentFertilizer.TreatmentId);
            
            return View(treatmentFertilizer);
        }

        // GET: JHTreatmentFertilizer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentFertilizer = await _context.TreatmentFertilizer
                .Include(t => t.FertilizerNameNavigation)
                .Include(t => t.Treatment)
                .FirstOrDefaultAsync(m => m.TreatmentFertilizerId == id);
            if (treatmentFertilizer == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("fertilizerName", _context.TreatmentFertilizer.SingleOrDefault(f => f.TreatmentFertilizerId == id).FertilizerName);

            return View(treatmentFertilizer);
        }

        // POST: JHTreatmentFertilizer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var treatmentFertilizer = await _context.TreatmentFertilizer.FindAsync(id);
                _context.TreatmentFertilizer.Remove(treatmentFertilizer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        private bool TreatmentFertilizerExists(int id)
        {
            return _context.TreatmentFertilizer.Any(e => e.TreatmentFertilizerId == id);
        }
    }
}
