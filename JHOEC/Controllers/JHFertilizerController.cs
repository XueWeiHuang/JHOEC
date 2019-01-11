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
    public class JHFertilizerController : Controller
    {
        private readonly OECContext _context;

        public JHFertilizerController(OECContext context)
        {
            _context = context;
        }

        // GET: JHFertilizer
        /// <summary>
        /// Allow user to view the list of fertilizers
        /// </summary>
        /// <returns>Index view with fertilizers displayed in list</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fertilizer.ToListAsync());
        }

        // GET: JHFertilizer/Details/5
        /// <summary>
        /// When a fertilizer is selected, the id of the fertilizer is passed in as parameter, then this specific fertilizer is passed in 
        /// Detail view() page to dsiplay its detail
        /// </summary>
        /// <param name="id">points to the specific fertilizer</param>
        /// <returns>Detail view() with slected fertilizer</returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizer = await _context.Fertilizer
                .FirstOrDefaultAsync(m => m.FertilizerName == id);
            if (fertilizer == null)
            {
                return NotFound();
            }

            return View(fertilizer);
        }

        // GET: JHFertilizer/Create
        /// <summary>
        /// Allow Create view() page to show
        /// </summary>
        /// <returns>Create view() page</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: JHFertilizer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Checks input from form for binding variables and avoid malicious injection, create a new fertilizer and save it to database
        /// </summary>
        /// <param name="fertilizer"></param>
        /// <returns>updated index view() page if new fertilizer is sucessfully created</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FertilizerName,Oecproduct,Liquid")] Fertilizer fertilizer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fertilizer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fertilizer);
        }

        // GET: JHFertilizer/Edit/5
        /// <summary>
        /// validate the selected fertilizer Id, and pass into Edit view() to show detail of selected fertilizer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Edit view() with detail of selected fertilizer</returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizer = await _context.Fertilizer.FindAsync(id);
            if (fertilizer == null)
            {
                return NotFound();
            }
            return View(fertilizer);
        }

        // POST: JHFertilizer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Allow user to save modified fertilizer detail to the spcific fertilizer detail in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fertilizer"></param>
        /// <returns>Update Index view() page with updated list of fertilizer</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FertilizerName,Oecproduct,Liquid")] Fertilizer fertilizer)
        {
            if (id != fertilizer.FertilizerName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fertilizer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FertilizerExists(fertilizer.FertilizerName))
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
            return View(fertilizer);
        }

        // GET: JHFertilizer/Delete/5
        /// <summary>
        /// validate specific fertilizer id, and show delete confirmation page with detail of selected fertilizer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizer = await _context.Fertilizer
                .FirstOrDefaultAsync(m => m.FertilizerName == id);
            if (fertilizer == null)
            {
                return NotFound();
            }

            return View(fertilizer);
        }

        // POST: JHFertilizer/Delete/5
        /// <summary>
        /// Once the delete action is confirmed, delete the detail of specific fertilizer and update list of fertilizer in the Index view() page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>updated list of fertilizer in the Index view() if the delete action is sucessfull</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var fertilizer = await _context.Fertilizer.FindAsync(id);
            _context.Fertilizer.Remove(fertilizer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FertilizerExists(string id)
        {
            return _context.Fertilizer.Any(e => e.FertilizerName == id);
        }
    }
}
