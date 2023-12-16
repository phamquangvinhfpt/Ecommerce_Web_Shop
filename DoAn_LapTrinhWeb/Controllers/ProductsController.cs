using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class ProductsController : Controller
    {
        private DbContext db = new DbContext();
        //Lấy danh sách laptop
        public ActionResult Laptop(int? page,string sortOrder)
        {
            ViewBag.Type = "Laptop";
            ViewBag.SortBy = "laptop" + "?";
            ViewBag.CountProduct = db.Products.Where(m => m.type == 1).Count();
            return View("Product", GetProduct(m => m.status == "1" && m.type == ProductType.LAPTOP, page, sortOrder));
        }
        //Lấy danh sách phụ kiện
        public ActionResult Accessories(int? page, string sortOrder)
        {
            ViewBag.Type = "Phụ kiện";
            ViewBag.SortBy = "accessory" + "?";
            ViewBag.CountProduct = db.Products.Where(m => m.type ==2).Count();
            return View("Product", GetProduct(m => m.status == "1" && m.type == ProductType.ACCESSORY, page, sortOrder));
        }
        //xem chi tiết sản phẩm
        public ActionResult ProductDetail(int id, int? page)
        {
            int pagesize = 1;
            int cpage = page ?? 1;
            var product = db.Products.SingleOrDefault(m =>m.status=="1" && m.product_id == id);
            if (product == null)
            {
                return Redirect("/");
            }
            //sản phẩm liên quan
            ViewBag.relatedproduct = db.Products.Where(item => item.status == "1" && item.product_id != product.product_id && item.genre_id==product.genre_id).Take(8).ToList();
            ViewBag.ProductImage = db.ProductImages.Where(item => item.product_id == id).ToList();
            ViewBag.ProductImage = db.ProductImages.Where(item => item.product_id == id).ToList();
            ViewBag.ListFeedback = db.Feedbacks.Where(m => m.stastus == "2").ToList();
            ViewBag.ListReplyFeedback = db.ReplyFeedbacks.Where(m => m.stastus == "2").ToList();
            ViewBag.CountFeedback = db.Feedbacks.Where(m => m.stastus == "2" && m.product_id == product.product_id).Count();
            ViewBag.OrderFeedback = db.Oder_Detail.ToList();
            var comment = db.Feedbacks.Where(m => m.product_id == product.product_id && m.stastus == "2").OrderByDescending(m => m.create_at).ToList();
            ViewBag.PagerFeedback = comment.ToPagedList(cpage, pagesize);
            product.view++;
            db.SaveChanges();
            return View(product);
        }
        //Bình luận đánh giá
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult ProductComment(Feedback comment, int productID, int discountID, int genreID, int rateStar, string commentContent)
        {
            bool result = false;
            int userID = User.Identity.GetUserId();
            if (User.Identity.IsAuthenticated) { 
                comment.account_id = userID;
                comment.rate_star = rateStar;
                comment.product_id = productID;
                comment.disscount_id = discountID;
                comment.genre_id = genreID;
                comment.content = commentContent;
                comment.stastus = "2";
                comment.create_at = DateTime.Now;
                comment.update_at = DateTime.Now;
                comment.create_by = userID.ToString();
                comment.update_by = userID.ToString();
                db.Feedbacks.Add(comment);
                db.SaveChanges();
                result = true;
                Notification.setNotification3s("Bình luận thành công", "success");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //Phản hồi bình luận/đánh giá
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
                Notification.setNotification3s("Phản hồi thành công", "success");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //Tìm kiếm sản phẩm
        public ActionResult SearchResult(int? page, string sortOrder, string s)
        {
            ViewBag.SortBy = "search?s="+s+"&";
            ViewBag.Type = "Kết quả tìm kiếm - " + s;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
            ViewBag.DateAscSort = sortOrder == "date_asc" ? "date_asc" : "date_asc";
            ViewBag.DateDescSort = sortOrder == "date_desc" ? "date_desc" : "date_desc";
            ViewBag.PopularSort = sortOrder == "popular" ? "popular" : "popular";
            ViewBag.PriceDescSort= sortOrder == "price_desc" ? "price_desc" : "price_desc";
            ViewBag.PriceAscSort = sortOrder == "price_asc" ? "price_asc" : "price_asc";
            ViewBag.NameAscSort = sortOrder == "name_asc" ? "name_asc" : "name_asc";
            ViewBag.NameDescSort = sortOrder == "name_desc" ? "name_desc" : "name_desc";
            var list = db.Products.OrderByDescending(m => m.product_id);
            switch (sortOrder)
            {
                case "price_asc":
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => (m.price - m.Discount.discount_price)).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "price_desc":
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.price - m.Discount.discount_price).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "date_asc":
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => m.create_at).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "date_desc":
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.create_at).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "name_asc":
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => m.product_name).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "name_desc":
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_name).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                default:
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.create_at);
                    break;
            }
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.product_name.Contains(s)).Count();          
            return View("Product", GetProduct(m => m.status == "1" && (m.product_name.Contains(s) || m.product_id.ToString().Contains(s)), page, sortOrder));
        }   
        //Get product 
        private IPagedList GetProduct(Expression<Func<Product, bool>> expr, int? page, string sortOrder)
        {
            int pageSize = 9; //1 trang hiện thỉ tối đa 9 sản phẩm
            int pageNumber = (page ?? 1); //đánh số trang
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            ViewBag.OrderDetail = db.Oder_Detail.ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
            ViewBag.DateAscSort = sortOrder == "date_asc" ? "date_asc" : "date_asc";
            ViewBag.DateDescSort = sortOrder == "date_desc" ? "date_desc" : "date_desc";
            ViewBag.PopularSort = sortOrder == "popular" ? "popular" : "popular";
            ViewBag.PriceDescSort = sortOrder == "price_desc" ? "price_desc" : "price_desc";
            ViewBag.PriceAscSort = sortOrder == "price_asc" ? "price_asc" : "price_asc";
            ViewBag.NameAscSort = sortOrder == "name_asc" ? "name_asc" : "name_asc";
            ViewBag.NameDescSort = sortOrder == "name_desc" ? "name_desc" : "name_desc";
            var list = db.Products.Where(expr).OrderByDescending(m => m.product_id).ToPagedList(pageNumber, pageSize);
            switch (sortOrder)
            {
              case "price_asc":
                    list = db.Products.Where(expr).OrderBy(m => (m.price - m.Discount.discount_price)).ToPagedList(pageNumber, pageSize);
                    break;
                case "price_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => (m.price - m.Discount.discount_price)).ToPagedList(pageNumber, pageSize);
                    break;
               case "date_asc":
                    list = db.Products.Where(expr).OrderBy(m => m.create_at).ToPagedList(pageNumber, pageSize);
                    break;
                case "date_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.create_at).ToPagedList(pageNumber, pageSize);
                    break;
                case "name_asc":
                    list = db.Products.Where(expr).OrderBy(m => m.product_name).ToPagedList(pageNumber, pageSize);
                    break;
                case "name_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_name).ToPagedList(pageNumber, pageSize);
                    break;
                default:
                    list = db.Products.Where(expr).OrderByDescending(m => m.create_at).ToPagedList(pageNumber, pageSize);
                    break;
            }
            ViewBag.Showing = list.Count();
            return list;
        }
    }
}