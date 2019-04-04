using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JHOEC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JHOEC.Controllers
{
    public class RemotesController : Controller
    {
        private readonly OECContext _context;
        public RemotesController(OECContext context)
        {
            _context = context;
        }

        public JsonResult CheckProvinceCode(string provinceCode)
        {
            if (provinceCode.Length!=2 || !Regex.IsMatch(provinceCode, @"^[a-zA-Z]+$"))
            {
                return Json("Province code must be exactly 2 letters and only letters are allowed");
            }
            try
            {
                var provinceCodeOnFile = _context.Province.Where(p => p.ProvinceCode == provinceCode.ToUpper()).Select(p => p.ProvinceCode).FirstOrDefault();
                if (provinceCodeOnFile == null)
                    return Json("Province code is not on file, enter a valid province code for this country");
            }
            catch (Exception ex)
            {
                return Json($"error validating province code: {ex.GetBaseException().Message}");
            }

            return Json(true);


        }
    }
}