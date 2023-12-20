using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EJBMes.Models;
using System.Security.Claims;
using System.Security.Policy;
using EJBMes.Services.Contract;
using EJBMes.Services.Implementation;
using System.Configuration;
using EJBMes.Resources;

namespace EJBMes.Controllers
{
    public class ScrapController : Controller
    {
        private readonly EjbproductionReportContext _context;
        private readonly IKineticService _kineticService;

        public ScrapController(EjbproductionReportContext context, IConfiguration configuration)
        {
            _context = context;
            _kineticService = new KineticService(configuration.GetValue<string>("URLKinetic"), configuration.GetValue<string>("UserKinetic"), configuration.GetValue<string>("PwdKinetic"), configuration.GetValue<string>("CompanyID"));
        }

        // GET: Scrap
        public async Task<IActionResult> Index()
        {
            return _context.ScrapReports != null ?
            View(await _context.ScrapReports.ToListAsync()) :
            Problem("Entity set 'EjbproductionReportContext.ScrapReports'  is null.");
        }

        // GET: Scrap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScrapReports == null)
            {
                return NotFound();
            }
            var scrapReport = await _context.ScrapReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scrapReport == null)
            {
                return NotFound();
            }

            return View(scrapReport);
        }

        // GET: Scrap/Create
        public IActionResult Create()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string employeID = "";
            string siteID = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                employeID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "EmployeeId").Select(c => c.Value).SingleOrDefault();
                siteID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "Site").Select(c => c.Value).SingleOrDefault();
            }
            ViewData["userName"] = userName;
            ViewData["employeID"] = employeID;
            ViewData["siteID"] = siteID;
            ViewData["ScrapDate"] = System.DateTime.Today.ToString("yyyy-MM-dd");

            //List<SelectListItem> lst = new List<SelectListItem>();
            //List<JobData> oJobData = _kineticService.GetJobs(siteID);
            //foreach (JobData oJob in oJobData)
            //{
            //    lst.Add(new SelectListItem() { Text = oJob.JobNum, Value = oJob.JobNum });
            //}
            //ViewBag.modelListJobs = lst;

            List<SelectListItem> lstWhse = new List<SelectListItem>();
            List<WareHouseData> oWhseData = _kineticService.GetWareHouses(siteID);
            foreach (WareHouseData oData in oWhseData)
            {
                lstWhse.Add(new SelectListItem() { Text = oData.WareHouseDescription, Value = oData.WareHouseCode });
            }
            ViewBag.modelListWhse = lstWhse;


            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Reason> oReason = _kineticService.GetReasons(siteID);
            foreach (Reason oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.ReasonDescription, Value = oData.ReasonCode });
            }
            ViewBag.modelListReason = lstReason;

            return View();
        }
        // POST: Scrap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,ScrapDate,ResourceGroup,ResourceId,ReferenceNotes,ScrapQty,ReasonCode,WhseCode,BinNum,Procesed")] ScrapReport scrapReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scrapReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string employeID = "";
            string siteID = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                employeID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "EmployeeId").Select(c => c.Value).SingleOrDefault();
                siteID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "Site").Select(c => c.Value).SingleOrDefault();
            }
            ViewData["userName"] = userName;
            ViewData["employeID"] = employeID;
            ViewData["siteID"] = siteID;
            ViewData["ScrapDate"] = System.DateTime.Today.ToString("yyyy-MM-dd");

            //List<SelectListItem> lst = new List<SelectListItem>();
            //List<JobData> oJobData = _kineticService.GetJobs(siteID);
            //foreach (JobData oJob in oJobData)
            //{
            //    lst.Add(new SelectListItem() { Text = oJob.JobNum, Value = oJob.JobNum });
            //}
            //ViewBag.modelListJobs = lst;

            List<SelectListItem> lstWhse = new List<SelectListItem>();
            List<WareHouseData> oWhseData = _kineticService.GetWareHouses(siteID);
            foreach (WareHouseData oData in oWhseData)
            {
                lstWhse.Add(new SelectListItem() { Text = oData.WareHouseDescription, Value = oData.WareHouseCode });
            }
            ViewBag.modelListWhse = lstWhse;

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Reason> oReason = _kineticService.GetReasons(siteID);
            foreach (Reason oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.ReasonDescription, Value = oData.ReasonCode });
            }
            ViewBag.modelListReason = lstReason;

            return View(scrapReport);
        }

        public JsonResult GetJobs(string siteID)
        {
            List<JobData> oJobsList = _kineticService.GetJobs(siteID);
            return Json(oJobsList);
            //return Json(new SelectList(oJobsList, "JobNum", "JobNum"));
        }

        public JsonResult GetJobsAsmblys(string siteID, string JobNum)
        {
            List<JobAsm> oJobAsmList = _kineticService.GetJobsAsmblys(siteID, JobNum);
            //return Json(oJobAsmList, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new SelectList(oJobAsmList, "AssemblySeq", "AssemblySeq"));
        }

        public JsonResult GetJobsAsmOprs(string siteID, string JobNum, int AssemblyNum)
        {
            List<JobOper> oJobOperList = _kineticService.GetJobsAsmOprs(siteID, JobNum, AssemblyNum);
            //return Json(oJobOperList, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new SelectList(oJobOperList, "OprSeq", "OpCodeOpDesc"));
        }

        public JsonResult GetResources(string siteID, string JobNum, int AssemblyNum, int OprSeq)
        {
            List<JobResource> oJobResList = _kineticService.GetJobResources(siteID, JobNum, AssemblyNum, OprSeq);
            return Json(oJobResList);
        }

        public JsonResult GetBinNums(string siteID, string WareHouseCode)
        {
            List<WhseBinData> oDataList = _kineticService.GetBinNums(siteID, WareHouseCode);
            return Json(new SelectList(oDataList, "BinNum", "BinNum"));
        }

        // GET: Scrap/Create
        public async Task<IActionResult> CreateById(int? id)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string employeID = "";
            string siteID = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                employeID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "EmployeeId").Select(c => c.Value).SingleOrDefault();
                siteID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "Site").Select(c => c.Value).SingleOrDefault();
            }
            ViewData["userName"] = userName;
            ViewData["employeID"] = employeID;
            ViewData["siteID"] = siteID;
            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var ProdReports = await _context.ProdReports.FindAsync(id);
            if (ProdReports == null)
            {
                return NotFound();
            }

            ViewData["JobNum"] = ProdReports.JobNum;
            ViewData["AssemblyNum"] = ProdReports.AssemblyNum;
            ViewData["OpSeq"] = ProdReports.OpSeq;
            ViewData["ResourceGroup"] = ProdReports.ResourceGroup;
            ViewData["ResourceId"] = ProdReports.ResourceId;
            ViewData["ScrapDate"] = System.DateTime.Today.ToString("yyyy-MM-dd");
            ViewData["ReferenceNotes"] = "";
            ViewData["ScrapQty"] = "";
            ViewData["ReasonCode"] = "";
            ViewData["WhseCode"] = "";
            ViewData["BinNum"] = "";
           

            var ScrapReportByJobNum = _context.ScrapReports != null ?
                           await _context.ScrapReports.ToListAsync() : null;
            if (ScrapReportByJobNum != null && ProdReports.JobNum != null) {
                ScrapReportByJobNum = ScrapReportByJobNum.Where(x => x.JobNum.Contains(ProdReports.JobNum)).ToList();
            }

            ViewData["modelList"] = ScrapReportByJobNum;

            List<SelectListItem> lstWhse = new List<SelectListItem>();
            List<WareHouseData> oWhseData = _kineticService.GetWareHouses(siteID);
            foreach (WareHouseData oData in oWhseData)
            {
                lstWhse.Add(new SelectListItem() { Text = oData.WareHouseDescription, Value = oData.WareHouseCode });
            }
            ViewBag.modelListWhse = lstWhse;

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Reason> oReason = _kineticService.GetReasons(siteID);
            foreach (Reason oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.ReasonDescription, Value = oData.ReasonCode });
            }
            ViewBag.modelListReason = lstReason;

            return View();
        }

        // POST: Scrap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateById(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,ScrapDate,ResourceGroup,ResourceId,ReferenceNotes,ScrapQty,ReasonCode,WhseCode,BinNum,Procesed")] ScrapReport scrapReport)
        {
            if (ModelState.IsValid)
            {
                scrapReport.Id = 0;
                _context.Add(scrapReport);
                await _context.SaveChangesAsync();
            }

            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string employeID = "";
            string siteID = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                employeID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "EmployeeId").Select(c => c.Value).SingleOrDefault();
                siteID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "Site").Select(c => c.Value).SingleOrDefault();
            }
            ViewData["userName"] = userName;
            ViewData["employeID"] = employeID;
            ViewData["siteID"] = siteID;

            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var ProdReports = await _context.ProdReports.FindAsync(id);
            if (ProdReports == null)
            {
                return NotFound();
            }

            ViewData["JobNum"] = ProdReports.JobNum;
            ViewData["AssemblyNum"] = ProdReports.AssemblyNum;
            ViewData["OpSeq"] = ProdReports.OpSeq;
            ViewData["ResourceGroup"] = ProdReports.ResourceGroup;
            ViewData["ResourceId"] = ProdReports.ResourceId;
            ViewData["ScrapDate"] = System.DateTime.Today.ToString("yyyy-MM-dd");
            ViewData["ReferenceNotes"] = "";
            ViewData["ScrapQty"] = "";
            ViewData["ReasonCode"] = "";
            ViewData["WhseCode"] = "";
            ViewData["BinNum"] = "";

            scrapReport = new ScrapReport();

            var ScrapReportByJobNum = _context.ScrapReports != null ?
                        await _context.ScrapReports.ToListAsync() : null;
            if (ScrapReportByJobNum != null && ProdReports.JobNum != null)
            {
                ScrapReportByJobNum = ScrapReportByJobNum.Where(x => x.JobNum.Contains(ProdReports.JobNum)).ToList();
            }

            ViewData["modelList"] = ScrapReportByJobNum;

            List<SelectListItem> lstWhse = new List<SelectListItem>();
            List<WareHouseData> oWhseData = _kineticService.GetWareHouses(siteID);
            foreach (WareHouseData oData in oWhseData)
            {
                lstWhse.Add(new SelectListItem() { Text = oData.WareHouseDescription, Value = oData.WareHouseCode });
            }
            ViewBag.modelListWhse = lstWhse;

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Reason> oReason = _kineticService.GetReasons(siteID);
            foreach (Reason oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.ReasonDescription, Value = oData.ReasonCode });
            }
            ViewBag.modelListReason = lstReason;

            return View();
                //return RedirectToAction(nameof(Index));
            //}
            //return View(scrapReport);
        }

        // GET: Scrap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string employeID = "";
            string siteID = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                employeID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "EmployeeId").Select(c => c.Value).SingleOrDefault();
                siteID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "Site").Select(c => c.Value).SingleOrDefault();
            }
            ViewData["userName"] = userName;
            ViewData["employeID"] = employeID;
            ViewData["siteID"] = siteID;

            if (id == null || _context.ScrapReports == null)
            {
                return NotFound();
            }

            var scrapReport = await _context.ScrapReports.FindAsync(id);
            if (scrapReport == null)
            {
                return NotFound();
            }

            List<SelectListItem> lstWhse = new List<SelectListItem>();
            List<WareHouseData> oWhseData = _kineticService.GetWareHouses(siteID);
            foreach (WareHouseData oData in oWhseData)
            {
                lstWhse.Add(new SelectListItem() { Text = oData.WareHouseDescription, Value = oData.WareHouseCode });
            }
            ViewBag.modelListWhse = lstWhse;

            List<SelectListItem> lstWhseBin = new List<SelectListItem>();
            List<WhseBinData> oWhseBinData = _kineticService.GetBinNums(siteID, scrapReport.WhseCode);
            foreach (WhseBinData oData in oWhseBinData)
            {
                lstWhseBin.Add(new SelectListItem() { Text = oData.BinNum, Value = oData.BinNum });
            }
            ViewBag.modelListWhseBin = lstWhseBin;

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Reason> oReason = _kineticService.GetReasons(siteID);
            foreach (Reason oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.ReasonDescription, Value = oData.ReasonCode });
            }
            ViewBag.modelListReason = lstReason;

            return View(scrapReport);
        }

        // POST: Scrap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,ScrapDate,ResourceGroup,ResourceId,ReferenceNotes,ScrapQty,ReasonCode,WhseCode,BinNum,Procesed")] ScrapReport scrapReport)
        {
            if (id != scrapReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scrapReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScrapReportExists(scrapReport.Id))
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

            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string employeID = "";
            string siteID = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                employeID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "EmployeeId").Select(c => c.Value).SingleOrDefault();
                siteID = claimUser.Claims.Where(c => c.Type == ClaimTypes.UserData && c.ValueType == "Site").Select(c => c.Value).SingleOrDefault();
            }
            ViewData["userName"] = userName;
            ViewData["employeID"] = employeID;
            ViewData["siteID"] = siteID;

            List<SelectListItem> lstWhse = new List<SelectListItem>();
            List<WareHouseData> oWhseData = _kineticService.GetWareHouses(siteID);
            foreach (WareHouseData oData in oWhseData)
            {
                lstWhse.Add(new SelectListItem() { Text = oData.WareHouseDescription, Value = oData.WareHouseCode });
            }
            ViewBag.modelListWhse = lstWhse;

            List<SelectListItem> lstWhseBin = new List<SelectListItem>();
            List<WhseBinData> oWhseBinData = _kineticService.GetBinNums(siteID, scrapReport.WhseCode);
            foreach (WhseBinData oData in oWhseBinData)
            {
                lstWhseBin.Add(new SelectListItem() { Text = oData.BinNum, Value = oData.BinNum });
            }
            ViewBag.modelListWhseBin = lstWhseBin;

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Reason> oReason = _kineticService.GetReasons(siteID);
            foreach (Reason oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.ReasonDescription, Value = oData.ReasonCode });
            }
            ViewBag.modelListReason = lstReason;

            return View(scrapReport);
        }

        // GET: Scrap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScrapReports == null)
            {
                return NotFound();
            }

            var scrapReport = await _context.ScrapReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scrapReport == null)
            {
                return NotFound();
            }

            return View(scrapReport);
        }

        // POST: Scrap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScrapReports == null)
            {
                return Problem("Entity set 'EjbproductionReportContext.ScrapReports'  is null.");
            }
            var scrapReport = await _context.ScrapReports.FindAsync(id);
            if (scrapReport != null)
            {
                _context.ScrapReports.Remove(scrapReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScrapReportExists(int id)
        {
          return (_context.ScrapReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult BackToList()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BackToListProduction()
        {
            return RedirectToAction("Index", "Production");
        }
    }
}
