using EJBMes.Models;
using EJBMes.Resources;
using EJBMes.Services.Contract;
using EJBMes.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;

namespace EJBMes.Controllers
{
    public class DownTimeController : Controller
    {
        private readonly EjbproductionReportContext _context;
        private readonly IKineticService _kineticService;

        public DownTimeController(EjbproductionReportContext context, IConfiguration configuration)
        {
            _context = context;
            _kineticService = new KineticService(configuration.GetValue<string>("URLKinetic"), configuration.GetValue<string>("UserKinetic"), configuration.GetValue<string>("PwdKinetic"), configuration.GetValue<string>("CompanyID"));
        }

        public async Task<IActionResult> Index()
        {
            if (_context.ProdReports == null)
            {
                return Problem("Entity set 'EjbproductionReportContext.DowntimeReports'  is null.");
            }
            var DowntimeReports = await _context.DowntimeReports.ToListAsync();

            return View(DowntimeReports);
        }

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

            //foreach (JobData oJob in oJobData)
            //{
            //    lst.Add(new SelectListItem() { Text = oJob.JobNum, Value = oJob.JobNum });
            //}

            //ViewBag.modelListJobs = lst;

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Indirect> oReason = _kineticService.GetIndirectReeasons(siteID);
            foreach (Indirect oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.IndirectDescription, Value = oData.IndirectCode });
            }
            ViewBag.modelListReason = lstReason;

            return View();
        }

        // POST: Downtime/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,DownTimeStartDate,DownTimeEndDate,ResourceGroup,ResourceId,ReferenceNotes,ReasonCode,ActiveDowntime,Procesed")] DowntimeReport downTimeReport)
        {
            bool itsValid = false;
            downTimeReport.DownTimeEndDate = DateTime.Now;
            downTimeReport.ActiveDowntime = true;
            downTimeReport.Procesed = false;

            if (downTimeReport.JobNum != null && downTimeReport.AssemblyNum >= 0 && downTimeReport.OpSeq > 0 && downTimeReport.ResourceGroup != null && downTimeReport.ResourceId != null)
            {
                itsValid = true;
            }

            if (itsValid && ModelState.IsValid)
            {
                _context.Add(downTimeReport);
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

                List<SelectListItem> lstReason = new List<SelectListItem>();
                List<Indirect> oReason = _kineticService.GetIndirectReeasons(siteID);
                foreach (Indirect oData in oReason)
                {
                    lstReason.Add(new SelectListItem() { Text = oData.IndirectDescription, Value = oData.IndirectCode });
                }
                ViewBag.modelListReason = lstReason;

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
            return View(downTimeReport);
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

        private bool DowntimeReportExists(int id)
        {
            return (_context.DowntimeReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //GET End DownTime
        public async Task<IActionResult> EndDownTime(int? id)
        {
            if (id == null || _context.DowntimeReports == null)
            {
                return NotFound();
            }

            var downTimeReport = await _context.DowntimeReports.FindAsync(id);
            if (downTimeReport == null)
            {
                return NotFound();
            }
            else
            {
                downTimeReport.DownTimeEndDate = DateTime.Now;
                downTimeReport.ActiveDowntime = false;
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

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Indirect> oReason = _kineticService.GetIndirectReeasons(siteID);
            foreach (Indirect oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.IndirectDescription, Value = oData.IndirectCode });
            }
            ViewBag.modelListReason = lstReason;

            return View(downTimeReport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndDownTime(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,DownTimeStartDate,DownTimeEndDate,ResourceGroup,ResourceId,ReferenceNotes,ReasonCode,ActiveDowntime,Procesed")] DowntimeReport downTimeReport)
        {
            if (id != downTimeReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(downTimeReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DowntimeReportExists(downTimeReport.Id))
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
            return View(downTimeReport);
        }

        public IActionResult BackToList()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: DownTime/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DowntimeReports == null)
            {
                return NotFound();
            }

            var downTimeReport = await _context.DowntimeReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (downTimeReport == null)
            {
                return NotFound();
            }

            return View(downTimeReport);
        }

        // GET: Downtime/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DowntimeReports == null)
            {
                return NotFound();
            }

            var downTimeReport = await _context.DowntimeReports.FindAsync(id);
            if (downTimeReport == null)
            {
                return NotFound();
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

            List<SelectListItem> lstReason = new List<SelectListItem>();
            List<Indirect> oReason = _kineticService.GetIndirectReeasons(siteID);
            foreach (Indirect oData in oReason)
            {
                lstReason.Add(new SelectListItem() { Text = oData.IndirectDescription, Value = oData.IndirectCode });
            }
            ViewBag.modelListReason = lstReason;
            return View(downTimeReport);
        }

        // POST: Downtime/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,DownTimeStartDate,DownTimeEndDate,ResourceGroup,ResourceId,ReferenceNotes,ReasonCode,ActiveDowntime,Procesed")] DowntimeReport downTimeReport)
        {
            if (id != downTimeReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(downTimeReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DowntimeReportExists(downTimeReport.Id))
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
            return View(downTimeReport);
        }

        // GET: Production/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DowntimeReports == null)
            {
                return NotFound();
            }

            var downTimeReport = await _context.DowntimeReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (downTimeReport == null)
            {
                return NotFound();
            }

            return View(downTimeReport);
        }

        // POST: Production/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DowntimeReports == null)
            {
                return Problem("Entity set 'EjbproductionReportContext.DowntimeReports'  is null.");
            }
            var downTimeReport = await _context.DowntimeReports.FindAsync(id);
            if (downTimeReport != null)
            {
                _context.DowntimeReports.Remove(downTimeReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
