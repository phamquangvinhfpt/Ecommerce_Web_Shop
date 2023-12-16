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
    public class FeedbacksController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Brands
        public ActionResult Index(string search,int? size, int? page)
        {
            var pageSize = (size ?? 15);
            var pageNumber = (page ?? 1);
            ViewBag.search = search;
            var list = from a in db.Feedbacks
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Feedbacks
                       where a.account_id.ToString().Contains(search)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        //phản hồi bình luận
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ReplyComment(int id, string reply_content, ReplyFeedback reply)
        {
            bool result = false;
            if (User.Identity.IsAuthenticated)
            {
                reply.feedback_id = id;
                reply.account_id = User.Identity.GetUserId();
                reply.content = reply_content;
                reply.stastus = "2";
                reply.create_at = DateTime.Now;
                db.ReplyFeedbacks.Add(reply);
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}