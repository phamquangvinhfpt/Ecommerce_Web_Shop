using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class AuthController : BaseController
    {
        private DbContext db = new DbContext();

        public ActionResult Index(string search, int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            ViewBag.search = search;
            ViewBag.countTrash = db.Accounts.Where(a => a.status == "0").Count(); 
            var list = from a in db.Accounts
                       where a.status != "0"
                       orderby a.create_at descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Accounts
                       where a.Email.Contains(search) || a.account_id.ToString().Contains(search) || a.Name.Contains(search)
                       orderby a.create_at descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string search, int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            ViewBag.search = search;
            var list = from a in db.Accounts
                       where a.status == "0"
                       orderby a.create_at descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Accounts
                       where a.Email.Contains(search) || a.account_id.ToString().Contains(search) || a.Name.Contains(search)
                       orderby a.create_at descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int id)
        {
            Account account = db.Accounts.FirstOrDefault(m => m.account_id == id);
            ViewBag.ListAddress = db.AccountAddresses.Where(m => m.account_id == id).ToList();
            if (account == null)
            {
                Notification.setNotification1_5s("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            return View(account);
        }

        public JsonResult ChangeRoles(int accountID, int roleID)
        {
            var account = db.Accounts.FirstOrDefault(m => m.account_id == accountID);
            int role = User.Identity.GetRole();
            bool result = false;
            try
            {
                if (account != null && role == 0)
                {
                    account.Role = roleID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Disable(int id)
        {
            string result = "error";
            Account account = db.Accounts.FirstOrDefault(m => m.account_id == id);
            try
            {
                if (User.Identity.GetUserId() != id)
                {
                    result = "success";
                    account.status = "0";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsActive(int id)
        {
            string result = "error";
            Account account = db.Accounts.FirstOrDefault(m => m.account_id == id);
            try
            {
                result = "success";
                account.status = "1";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}