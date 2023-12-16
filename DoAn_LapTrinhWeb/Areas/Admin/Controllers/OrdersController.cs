using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClosedXML.Excel;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Orders
        public ActionResult Index(string search, int? size, int? page)
        {
            var pageSize = size ?? 15;
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            ViewBag.countTrash = db.Orders.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            var list = from a in db.Orders
                       where a.status != "0"
                       orderby a.create_at descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Orders
                       where a.order_id.ToString().Contains(search)
                       orderby a.create_at descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string search, int? size, int? page)
        {
            var pageSize = size ?? 15;
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            var list = from a in db.Orders
                       where a.status == "0"
                       orderby a.create_at descending
                       select a;
            if (!string.IsNullOrEmpty(search))
            {
                list = from a in db.Orders
                       where a.order_id.ToString().Contains(search)
                       orderby a.create_at descending
                       select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            ViewBag.ListProduct = db.Oder_Detail.Where(m => m.order_id == order.order_id).ToList();
            ViewBag.OrderHistory = db.Orders.Where(m => m.account_id == order.account_id).OrderByDescending(m=>m.oder_date).Take(10).ToList();
            if (order == null)
            {
                Notification.setNotification1_5s("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            return View(order);
        }

        public JsonResult UpdateOrder(int id,string status)
        {
            string result = "error";
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            try
            {
                if (order.status != "3")
                {
                    result = "success";
                    order.status = status;
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "false";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CancleOrder(int id)
        {
            string result = "error";
            Order order = db.Orders.FirstOrDefault(m => m.order_id == id);
            try
            {
                if (order.status != "3")
                {
                    result = "success";
                    order.status = "0";
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "false";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //Xuất đơn hàng và tổng doanh thu ra file excel theo từng tháng trong năm
        public ActionResult ExportExcel()
        {
            var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.Worksheets.Add("Danh sách đơn hàng");
            var ws1 = wb.Worksheets.Add("Tổng doanh thu");
            //Đổ dữ liệu vào sheet danh sách đơn hàng và tổng doanh thu
            var OrderList = db.Orders.Include("Account").Include("OrderAddress").Include("Oder_Detail").ToList();
            //Add tên cột cho sheet danh sách đơn hàng = tên các thuộc tính trong model
            ws.Cell(1, 1).Value = "Mã đơn hàng";
            ws.Cell(1, 2).Value = "Tên khách hàng";
            ws.Cell(1, 3).Value = "Số điện thoại";
            ws.Cell(1, 4).Value = "Địa chỉ";
            ws.Cell(1, 5).Value = "Ngày đặt hàng";
            ws.Cell(1, 6).Value = "Tổng tiền";
            ws.Cell(1, 7).Value = "Trạng thái";
            ws.Cell(1, 8).Value = "Ngày cập nhật";
            ws.Cell(1, 9).Value = "Người cập nhật";
            ws.Cell(1, 10).Value = "Ngày tạo";
            ws.Cell(1, 11).Value = "Người tạo";
            //Add tên cột cho sheet tổng doanh thu = tên các thuộc tính trong model
            ws1.Cell(1, 1).Value = "Tháng";
            ws1.Cell(1, 2).Value = "Tổng doanh thu";
            //Add dữ liệu cho sheet danh sách đơn hàng
            for (int i = 0; i < OrderList.Count; i++)
            {
                //Status trạng thái đơn hàng 1 = Đang chờ xử lý, 2 = Đang giao hàng, 3 = Đã giao hàng, 4 = Đã hủy
                ws.Cell(i + 2, 1).Value = OrderList[i].order_id;
                ws.Cell(i + 2, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 2).Value = OrderList[i].Account.Name;
                ws.Cell(i + 2, 2).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 3).Value = OrderList[i].Account.Phone;
                ws.Cell(i + 2, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 4).Value = OrderList[i].OrderAddress.content;
                ws.Cell(i + 2, 4).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                //format ngày tháng
                ws.Cell(i + 2, 5).Value = OrderList[i].oder_date.ToString("dd/MM/yyyy");
                ws.Cell(i + 2, 5).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 6).Value = OrderList[i].total;
                ws.Cell(i + 2, 6).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 7).Value = OrderList[i].status == "1" ? "Đang chờ xử lý" : OrderList[i].status == "2" ? "Đang giao hàng" : OrderList[i].status == "3" ? "Đã giao hàng" : "Đã hủy";
                ws.Cell(i + 2, 7).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 8).Value = OrderList[i].update_at.ToString("dd/MM/yyyy"); ;
                ws.Cell(i + 2, 8).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 9).Value = OrderList[i].update_by;
                ws.Cell(i + 2, 9).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 10).Value = OrderList[i].create_at.ToString("dd/MM/yyyy");
                ws.Cell(i + 2, 10).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 2, 11).Value = OrderList[i].create_by;
                ws.Cell(i + 2, 11).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            }
            ws.Columns().AdjustToContents();
            //Add dữ liệu cho sheet tổng doanh thu
            var total = db.Orders.Where(m => m.status == "3").GroupBy(m => m.oder_date.Month).Select(m => new { month = m.Key, total = m.Sum(n => n.total) }).ToList();
            for (int i = 0; i < total.Count; i++)
            {
                ws1.Cell(i + 2, 1).Value = total[i].month;
                ws1.Cell(i + 2, 2).Value = total[i].total;
            }
            string nameFile = "ExportOrder_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
            string path = Path.Combine(Server.MapPath("~/Content/Export/"), nameFile);
            using (var stream = new MemoryStream())
            {
                wb.SaveAs(path);
                //return file excel về client
                stream.Position = 0;
                //return static file về client
                return Content("/Content/Export/" + nameFile);
            }
        }
    }
}