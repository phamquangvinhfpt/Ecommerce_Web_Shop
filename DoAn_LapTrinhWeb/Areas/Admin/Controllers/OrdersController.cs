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
            //ws Doanh thu bán hàng từ ngày 1 đến ngày ... tháng ... năm ...
            var ws = wb.Worksheets.Add("Doanh thu");
            var ws1 = wb.Worksheets.Add("Sản phẩm bán ra");
            //Tạo tiêu đề cho file excel
            ws.Cell("A1").Value = "Doanh thu bán hàng từ ngày 1 đến ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            ws1.Cell("A1").Value = "Thống kê sản phẩm bán ra từ ngày 1 đến ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            //Đổ dữ liệu vào sheet Doanh thu và Sản phẩm bán ra

            ws.Cell("A2").Value = "STT";
            ws.Cell("A2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("B2").Value = "Mã hóa đơn";
            ws.Cell("B2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("C2").Value = "Mã khách hàng";
            ws.Cell("C2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("D2").Value = "Tên khách hàng";
            ws.Cell("D2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("E2").Value = "Thời gian đặt hàng";
            ws.Cell("E2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("F2").Value = "Tổng tiền";
            ws.Cell("F2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("G2").Value = "Trạng thái";
            ws.Cell("G2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

            var OrderList = db.Orders.Include("Account").Include("OrderAddress").Include("Oder_Detail").ToList();

            for (int i = 0; i < OrderList.Count; i++)
            {
                ws.Cell(i + 3, 1).Value = i + 1;
                ws.Cell(i + 3, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 3, 2).Value = OrderList[i].order_id;
                ws.Cell(i + 3, 2).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 3, 3).Value = OrderList[i].account_id;
                ws.Cell(i + 3, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 3, 4).Value = OrderList[i].Account.Name;
                ws.Cell(i + 3, 4).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 3, 5).Value = OrderList[i].oder_date.ToString("dd/MM/yyyy");
                ws.Cell(i + 3, 5).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 3, 6).Value = OrderList[i].total;
                ws.Cell(i + 3, 6).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 3, 7).Value = OrderList[i].status == "1" ? "Đang chờ xử lý" : OrderList[i].status == "2" ? "Đang giao hàng" : OrderList[i].status == "3" ? "Đã giao hàng" : "Đã hủy";
                ws.Cell(i + 2, 8).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            }

            //Add thêm Tổng tiền thanh toán cho sheet doanh thu
            ws.Cell(OrderList.Count + 3, 5).Value = "Tổng tiền thanh toán";
            ws.Cell(OrderList.Count + 3, 5).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell(OrderList.Count + 3, 5).Style.Font.Bold = true;
            ws.Cell(OrderList.Count + 3, 6).Value = OrderList.Where(m => m.status == "3").Sum(m => m.total);

            //Add border cho sheet doanh thu
            ws.RangeUsed().Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            //Add màu nền cho sheet doanh thu
            ws.RangeUsed().Style.Fill.BackgroundColor = XLColor.LightGray;
            //Border cho table
            ws.Columns().AdjustToContents();
            //Merge Cell cho sheet doanh thu
            ws.Range("A1:G1").Merge();
            //Adjust Cell A1 cho sheet doanh thu center
            ws.Cell("A1").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

            //Đổ dữ liệu vào sheet sản phẩm bán ra
            ws1.Cell("A2").Value = "STT";
            ws1.Cell("A2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws1.Cell("B2").Value = "Tên sản phẩm";
            ws1.Cell("B2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws1.Cell("C2").Value = "Số lượng bán";
            ws1.Cell("C2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws1.Cell("D2").Value = "Số đơn hàng";
            ws1.Cell("D2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

            var ProductList = db.Products.Include("Oder_Detail").ToList();
            var ListProductHasOrder = ProductList.Where(m => m.Oder_Detail.Count > 0).ToList();

            for (int i = 0; i < ListProductHasOrder.Count; i++)
            {
                    ws1.Cell(i + 3, 1).Value = i + 1;
                    ws1.Cell(i + 3, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    ws1.Cell(i + 3, 2).Value = ListProductHasOrder[i].product_name;
                    ws1.Cell(i + 3, 2).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    ws1.Cell(i + 3, 3).Value = ListProductHasOrder[i].Oder_Detail.Sum(m => m.quantity);
                    ws1.Cell(i + 3, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    ws1.Cell(i + 3, 4).Value = ListProductHasOrder[i].Oder_Detail.Count;
                    ws1.Cell(i + 3, 4).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            }

            //Add border cho sheet sản phẩm bán ra
            ws1.RangeUsed().Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws1.RangeUsed().Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            //Add màu nền cho sheet sản phẩm bán ra
            ws1.RangeUsed().Style.Fill.BackgroundColor = XLColor.LightGray;
            //Border cho table
            ws1.Columns().AdjustToContents();
            //Merge Cell cho sheet sản phẩm bán ra
            ws1.Range("A1:D1").Merge();
            //Adjust Cell A1 cho sheet sản phẩm bán ra center
            ws1.Cell("A1").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

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