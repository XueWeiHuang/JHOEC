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
    public class JHCropController : Controller
    {
        private readonly OECContext _context;

        public JHCropController(OECContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Allow user to view the list of crops in the crop table
        /// </summary>
        /// <returns>a list of crop data to show in the view</returns>

        // GET: JHCrop
        public async Task<IActionResult> Index()
        {
            return View(await _context.Crop.OrderBy(c=>c.Name).ToListAsync());
        }

        // GET: JHCrop/Details/5
        /// <summary>
        /// through selecting a crop and passing in nullable id to view the detail of that specific crop
        /// </summary>
        /// <param name="id"></param>
        /// <returns>if id is null 404 is returned, otherwise to direct to the crop detail view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop
                .FirstOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }
            
            return View(crop);
        }

        // GET: JHCrop/Create
        /// <summary>
        /// allow user to go to the view of create
        /// </summary>
        /// <returns>create view page</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: JHCrop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// only loof for listed varialbe from the browser and avoid malicious injection, and allow user to create a new crop with detail information
        /// </summary>
        /// <param name="crop"></param>
        /// <returns>if creation is sucessfull, redirect to the index view to see the list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create([Bind("CropId,Name,Image")] Crop crop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: JHCrop/Edit/5
        /// <summary>
        /// Allow user to modify the detail of a crop by passing in nullable id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return 404 if the id is null. if sucessfull, direct to edit view page to allow editing </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop.FindAsync(id);
            if (crop == null)
            {
                return NotFound();
            }
          
            return View(crop);
        }

        // POST: JHCrop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Allow user to modify the detail of crop and save change and return user to the list of crop view (index) page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="crop"></param>
        /// <returns>Index page if sucessfull</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CropId,Name,Image")] Crop crop)
        {
            if (id != crop.CropId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CropExists(crop.CropId))
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
            return View(crop);
        }

        // GET: JHCrop/Delete/5
        /// <summary>
        /// Let user select a crop to delete, and redirect user to delete view page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete view page</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop
                .FirstOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }

            return View(crop);
        }

        // POST: JHCrop/Delete/5
        /// <summary>
        /// Allow use to delete a specific crop type, and save changes to database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>if action is sucessfull, redirect user to index page to see the updated list of crop types</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crop = await _context.Crop.FindAsync(id);
            _context.Crop.Remove(crop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CropExists(int id)
        {
            return _context.Crop.Any(e => e.CropId == id);
        }
    }
}
