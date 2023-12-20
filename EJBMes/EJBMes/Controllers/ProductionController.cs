using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EJBMes.Models;
using System.Security.Claims;
using EJBMes.Services.Contract;
using EJBMes.Services.Implementation;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EJBMes.Resources;

namespace EJBMes.Controllers
{
    public class ProductionController : Controller
    {
        private readonly EjbproductionReportContext _context;
        private readonly IKineticService _kineticService;

        public ProductionController(EjbproductionReportContext context, IConfiguration configuration)
        {
            _context = context;
            _kineticService = new KineticService(configuration.GetValue<string>("URLKinetic"), configuration.GetValue<string>("UserKinetic"), configuration.GetValue<string>("PwdKinetic"),configuration.GetValue<string>("CompanyID"));
        }

        // GET: Production
        //public async Task<IActionResult> Index()
        //{
        //      return _context.ProdReports != null ? 
        //                  View(await _context.ProdReports.ToListAsync()) :
        //                  Problem("Entity set 'EjbproductionReportContext.ProdReports'  is null.");
        //}
        public async Task<IActionResult> Index(string? ActiveAct, DateTime? FromDate, DateTime? ToDate)
        {
            if (_context.ProdReports == null)
            {
                return Problem("Entity set 'EjbproductionReportContext.ProdReports'  is null.");
            }

            var vProdReports = await _context.ProdReports.ToListAsync();
            if (ActiveAct != null)
            {
                if (ActiveAct == "A")
                {
                    vProdReports = vProdReports.Where(x => x.ActiveLabor == true).ToList();
                }
                if (ActiveAct == "I")
                {
                    vProdReports = vProdReports.Where(x => x.ActiveLabor == false).ToList();
                }
                ViewData["ActiveAct"] = ActiveAct;
            }
            else
            {
                ViewData["ActiveAct"] = "A";
                vProdReports = vProdReports.Where(x => x.ActiveLabor == true).ToList();
            }
            if(FromDate == null)
            {
                DateTime pDate = System.DateTime.Today.AddDays(-(System.DateTime.Today.Day-1));
                ViewData["FromDate"] = pDate.ToString("yyyy-MM-dd");
                vProdReports = vProdReports.Where(x => x.StartDate >= pDate).ToList();
            }
            else
            {
                DateTime pDate = (DateTime) FromDate;
                ViewData["FromDate"] = pDate.ToString("yyyy-MM-dd");
                vProdReports = vProdReports.Where(x => x.StartDate >= pDate).ToList();
            }
            if (ToDate == null)
            {
                DateTime pDate = System.DateTime.Today.AddDays(1).AddSeconds(-1);
                ViewData["ToDate"] = pDate.ToString("yyyy-MM-dd");
                vProdReports = vProdReports.Where(x => x.StartDate <= pDate).ToList();
            }
            else
            {
                DateTime pDate = ((DateTime)ToDate).AddDays(1).AddSeconds(-1);
                ViewData["ToDate"] = pDate.ToString("yyyy-MM-dd");
                vProdReports = vProdReports.Where(x => x.StartDate <= pDate).ToList();
            }
            
            return View(vProdReports);
        }

        // GET: Production/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var prodReport = await _context.ProdReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodReport == null)
            {
                return NotFound();
            }

            return View(prodReport);
        }

        // GET: Production/Create
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
            ViewData["StartDate"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewData["EndDate"] = System.DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");

            //List<SelectListItem> lst = new List<SelectListItem>();

            //List<JobData> oJobData = _kineticService.GetJobs(siteID);

            //foreach(JobData oJob in oJobData)
            //{
            //    lst.Add(new SelectListItem() { Text = oJob.JobNum, Value = oJob.JobNum });
            //}

            //ViewBag.modelListJobs = lst;
            //List<SelectListItem> lstAsm = new List<SelectListItem>();
            //ViewBag.modelListJobsAsm = lstAsm;
            //List<SelectListItem> lstOpr = new List<SelectListItem>();
            //ViewBag.modelListJobsOpr = lstOpr;

            return View();
        }

        // POST: Production/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,ReferenceNotes,LaborQty,ActiveLabor,Procesed")] ProdReport prodReport)
        {
            bool itsValid = false;
            prodReport.LaborQty = 0;
            prodReport.ReferenceNotes = "";
            prodReport.EndDate = DateTime.Now;
            prodReport.ActiveLabor = true;
            prodReport.Procesed = false;

            if(prodReport.JobNum != null && prodReport.AssemblyNum >= 0 && prodReport.OpSeq > 0 && prodReport.ResourceGroup != null && prodReport.ResourceId != null)
            {
                itsValid = true;
            }

            if (itsValid && ModelState.IsValid)
            {
                _context.Add(prodReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
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
                ViewData["StartDate"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ViewData["EndDate"] = System.DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");

                //List<SelectListItem> lst = new List<SelectListItem>();

                //List<JobData> oJobData = _kineticService.GetJobs(siteID);

                //foreach (JobData oJob in oJobData)
                //{
                //    lst.Add(new SelectListItem() { Text = oJob.JobNum, Value = oJob.JobNum });
                //}

                //ViewBag.modelListJobs = lst;

                //if (prodReport.JobNum != null)
                //{
                //    List<SelectListItem> lstAsm = new List<SelectListItem>();
                //    foreach (JobAsm oJobAsm in _kineticService.GetJobsAsmblys(siteID, prodReport.JobNum))
                //    {
                //        lstAsm.Add(new SelectListItem() { Text = oJobAsm.AssemblySeq.ToString(), Value = oJobAsm.AssemblySeq.ToString() });
                //        prodReport.AssemblyNum = oJobAsm.AssemblySeq;
                //    }
                //    ViewBag.modelListJobsAsm = lstAsm;
                //    List<SelectListItem> lstOpr = new List<SelectListItem>();
                //    foreach (JobOper oJobOper in _kineticService.GetJobsAsmOprs(siteID, prodReport.JobNum, prodReport.AssemblyNum))
                //    {
                //        lstOpr.Add(new SelectListItem() { Text = oJobOper.OpCodeOpDesc.ToString(), Value = oJobOper.OprSeq.ToString() });
                //        prodReport.OpSeq = oJobOper.OprSeq;
                //    }
                //    ViewBag.modelListJobsOpr = lstOpr;
                //}

                ViewData["message"] = string.Empty;
                foreach (KeyValuePair<string, ModelStateEntry> modelState in ViewData.ModelState)
                {
                    
                    foreach (ModelError error in modelState.Value.Errors)
                    {
                        // Get the Error details.
                        ViewData["message"] += "Error " + modelState.Key + ":" + error.ErrorMessage + "; ";
                    }
                }
            }
            return View(prodReport);
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

        // GET: Production/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var prodReport = await _context.ProdReports.FindAsync(id);
            if (prodReport == null)
            {
                return NotFound();
            }
            return View(prodReport);
        }

        // POST: Production/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,ReferenceNotes,LaborQty,ActiveLabor,Procesed")] ProdReport prodReport)
        {
            if (id != prodReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prodReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdReportExists(prodReport.Id))
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
            return View(prodReport);
        }

        // GET: Production/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var prodReport = await _context.ProdReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodReport == null)
            {
                return NotFound();
            }

            return View(prodReport);
        }

        // POST: Production/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProdReports == null)
            {
                return Problem("Entity set 'EjbproductionReportContext.ProdReports'  is null.");
            }
            var prodReport = await _context.ProdReports.FindAsync(id);
            if (prodReport != null)
            {
                _context.ProdReports.Remove(prodReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdReportExists(int id)
        {
          return (_context.ProdReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Production/Edit/5
        public async Task<IActionResult> EndActivity(int? id)
        {
            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var prodReport = await _context.ProdReports.FindAsync(id);
            if (prodReport == null)
            {
                return NotFound();
            }
            else
            {
                prodReport.EndDate = DateTime.Now;
                prodReport.ActiveLabor = false;
            }
            return View(prodReport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndActivity(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,ReferenceNotes,LaborQty,ActiveLabor,Procesed")] ProdReport prodReport)
        {
            if (id != prodReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {            
                    _context.Update(prodReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdReportExists(prodReport.Id))
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
            return View(prodReport);
        }

        public IActionResult BackToList()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateScrap()
        {
            return RedirectToAction("Create","Scrap");
        }

        public IActionResult CreateScrapByID(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,ReferenceNotes,LaborQty,ActiveLabor,Procesed")] ProdReport prodReport)
        {
            bool ItsValid = false;
            if (id != prodReport.Id)
            {
                return NotFound();
            }
            
            if (id > 0)
            {
                ItsValid = true;
            }

            if (ItsValid)
            {
                return RedirectToAction("CreateById", "Scrap", new { id = id });
            }
            else
            {
                ViewData["message"] = "A Valid JobNum its required.";
                //foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                //{
                //    foreach (ModelError error in modelState.Errors)
                //    {
                //        // Get the Error details.
                //        ViewData["message"] += "Error." + error.ErrorMessage;
                //    }
                //}
            }
            return View(prodReport);
        }

        // GET: Production/ReportQty/5
        public async Task<IActionResult> ReportQty(int? id)
        {
            if (id == null || _context.ProdReports == null)
            {
                return NotFound();
            }

            var prodReport = await _context.ProdReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prodReport == null)
            {
                return NotFound();
            }

            return View(prodReport);
        }
        // POST: Production/ReportQty/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportQty(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,ReferenceNotes,LaborQty,ActiveLabor,Procesed")] ProdReport prodReport, decimal NewReportQty)
        {
            if (id != prodReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    prodReport.LaborQty += NewReportQty;
                    _context.Update(prodReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdReportExists(prodReport.Id))
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
            return View(prodReport);
        }
    }
}
