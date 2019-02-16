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
    public class JHPlotController : Controller
    {
        private readonly OECContext _context;
        const string CROP_ROUTE = "crop";
        const string VARIETY_ROUTE = "varitey";

        public JHPlotController(OECContext context)
        {
            _context = context;
        }

        //private string cropid;
        //public string CropId
        //{
        //    get { return cropid; }
        //    set { cropid = value; }
        //}


        // GET: JHPlot
        public async Task<IActionResult> Index(int? cropId, string cropName, int? varietyId, string varietyName, string filter)
        {
            var oECContext = _context.Plot.Include(p => p.Farm).Include(p => p.Variety).ThenInclude(p => p.Crop).Include(p => p.Treatment).AsQueryable();
            var accessRoute = HttpContext.Session.GetString("accessRoute");

            if ((varietyId!= null && varietyId != 0) || (cropId!=null && cropId !=0))
            {
                if (cropId!=0 && cropId!=null)
                {                
                    HttpContext.Session.SetString(nameof(cropId), cropId.ToString());
                    if (cropName==null || cropName=="")
                    {
                        HttpContext.Session.SetString(nameof(cropName), _context.Crop.SingleOrDefault(c => c.CropId == (Convert.ToInt32(cropId))).Name);
                    }
                    else
                    {
                        HttpContext.Session.SetString(nameof(cropName), cropName);
                    }
                    //hard way to is define it again like
                    //oECContext = _context.Plot.Include(p => p.Farm).Include(p => p.Variety).ThenInclude(p => p.Crop).Include(p => p.Treatment).Where(v => v.Variety.CropId == cropId).OrderByDescending(p => p.DatePlanted)
                    //oECContext = oECContext.Where(v => v.Variety.CropId == (Convert.ToInt32(cropId)));
                    //ViewData["spe"] = $"this is for crop {cropName}";
                    accessRoute = CROP_ROUTE;
                }
                else 
                {
                    HttpContext.Session.SetString(nameof(varietyId), varietyId.ToString());
                    if (varietyName==null || varietyName=="")
                    {
                        HttpContext.Session.SetString(nameof(varietyName), _context.Variety.SingleOrDefault(v => v.VarietyId == (Convert.ToInt32(varietyId))).Name);
                    }
                    else
                    {
                        HttpContext.Session.SetString(nameof(varietyName), varietyName);
                    }
                    //oECContext = oECContext.Where(v => v.Variety.VarietyId == (Convert.ToInt32(varietyId)));
                    //ViewData["spe"] = $"this is for variety {varietyName}";
                    accessRoute = VARIETY_ROUTE;
                }
                HttpContext.Session.SetString(nameof(accessRoute), accessRoute);
            }
            //else if (HttpContext.Session.GetString(nameof(cropId)) != null || HttpContext.Session.GetString(nameof(varietyId)) != null)
            //{
            //    if (HttpContext.Session.GetString(nameof(varietyId)) != null)
            //    {
            //        varietyId = Convert.ToInt32(HttpContext.Session.GetString(nameof(varietyId)));
            //        accessRoute = VARIETY_ROUTE;
            //        //oECContext = oECContext.Where(v => v.Variety.VarietyId == (Convert.ToInt32(varietyId)));
            //    }
            //    else if (HttpContext.Session.GetString(nameof(cropId)) != null)
            //    {
            //        cropId = Convert.ToInt32(HttpContext.Session.GetString(nameof(cropId)));
            //        accessRoute = CROP_ROUTE;
            //        //oECContext = oECContext.Where(v => v.Variety.CropId == (Convert.ToInt32(cropId)));
            //    }
            //    //cropName = HttpContext.Session.GetString(nameof(cropName));
            //}
            switch (accessRoute)
            {
                case VARIETY_ROUTE:
                    oECContext = oECContext.Where(v => v.Variety.VarietyId == (Convert.ToInt32(HttpContext.Session.GetString(nameof(varietyId)))));
                    break;
                case CROP_ROUTE:
                    oECContext = oECContext.Where(v => v.Variety.CropId == (Convert.ToInt32(HttpContext.Session.GetString(nameof(cropId)))));
                    break;
                default:
                    break;
            }
            //else if (HttpContext.Session.GetString(nameof(varietyId)) != null)
            //{
            //    varietyId = Convert.ToInt32(HttpContext.Session.GetString(nameof(varietyId)));
            //    //cropName = HttpContext.Session.GetString(nameof(cropName));
            //}
            //else
            //{
            //    TempData["message"] = "Please select a crop to view its variety ";
            //    return Redirect($"/JHCrop/Index/");
            //}

            //var oECContext = _context.Variety.Include(v => v.Crop).Where(v => v.CropId == cropId).OrderBy(v => v.Name);
            //return View(await oECContext.ToListAsync());
            if (filter == "farm")
                oECContext = oECContext.OrderBy(f => f.Farm.Name).ThenByDescending(p => p.DatePlanted);
            else if (filter == "variety")
                oECContext = oECContext.OrderBy(v => v.Variety.Name).ThenByDescending(p => p.DatePlanted);
            else if (filter == "cec")
                oECContext = oECContext.OrderBy(c => c.Cec).ThenByDescending(p => p.DatePlanted);
            else
                oECContext = oECContext.OrderByDescending(p => p.DatePlanted);
            return View(await oECContext.ToListAsync());
        }

        // GET: JHPlot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .FirstOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }

            return View(plot);
        }

        // GET: JHPlot/Create
        public IActionResult Create()
        {
            ViewData["FarmId"] = new SelectList(_context.Farm.OrderBy(f=>f.Name), "FarmId", "Name");
            if (HttpContext.Session.GetString("accessRoute")==CROP_ROUTE)
            {
                ViewData["VarietyId"] = new SelectList(_context.Variety.Where(c => c.CropId == (Convert.ToInt32(HttpContext.Session.GetString("cropId")))).OrderBy(v => v.Name), "VarietyId", "Name");
            }
            else if (HttpContext.Session.GetString("accessRoute") == VARIETY_ROUTE)
            {
                ViewData["VarietyId"] = new SelectList(_context.Variety.OrderBy(v => v.Name), "VarietyId", "Name", (Convert.ToInt32(HttpContext.Session.GetString("varietyId"))));
            }           
            return View();
        }

        // POST: JHPlot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlotId,FarmId,VarietyId,DatePlanted,DateHarvested,PlantingRate,PlantingRateByPounds,RowWidth,PatternRepeats,OrganicMatter,BicarbP,Potassium,Magnesium,Calcium,PHsoil,PHbuffer,Cec,PercentBaseSaturationK,PercentBaseSaturationMg,PercentBaseSaturationCa,PercentBaseSaturationH,Comments")] Plot plot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", plot.FarmId);                       
            if (HttpContext.Session.GetString("accessRoute") == CROP_ROUTE)
            {
                ViewData["VarietyId"] = new SelectList(_context.Variety.Where(c => c.CropId == (Convert.ToInt32(HttpContext.Session.GetString("cropId")))).OrderBy(v => v.Name), "VarietyId", "Name", plot.VarietyId);
            }
            else if (HttpContext.Session.GetString("accessRoute") == VARIETY_ROUTE)
            {
                ViewData["VarietyId"] = new SelectList(_context.Variety.OrderBy(v => v.Name), "VarietyId", "Name", plot.VarietyId);
            }
            return View(plot);
        }

        // GET: JHPlot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot.FindAsync(id);
            if (plot == null)
            {
                return NotFound();
            }
            ViewData["FarmId"] = new SelectList(_context.Farm.OrderBy(f=>f.Name), "FarmId", "Name", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety.OrderBy(v=>v.Name), "VarietyId", "Name", plot.VarietyId);
            return View(plot);
        }

        // POST: JHPlot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlotId,FarmId,VarietyId,DatePlanted,DateHarvested,PlantingRate,PlantingRateByPounds,RowWidth,PatternRepeats,OrganicMatter,BicarbP,Potassium,Magnesium,Calcium,PHsoil,PHbuffer,Cec,PercentBaseSaturationK,PercentBaseSaturationMg,PercentBaseSaturationCa,PercentBaseSaturationH,Comments")] Plot plot)
        {
            if (id != plot.PlotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlotExists(plot.PlotId))
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
            ViewData["FarmId"] = new SelectList(_context.Farm.OrderBy(f=>f.Name), "FarmId", "ProvinceCode", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety.OrderBy(v=>v.Name), "VarietyId", "VarietyId", plot.VarietyId);
            return View(plot);
        }

        // GET: JHPlot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .FirstOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }

            return View(plot);
        }

        // POST: JHPlot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plot = await _context.Plot.FindAsync(id);
            _context.Plot.Remove(plot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlotExists(int id)
        {
            return _context.Plot.Any(e => e.PlotId == id);
        }
    }
}
