using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using PagedList;
using ClosedXML.Excel;
using System.IO;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DashBoardsController : BaseController
    {
        private readonly DbContext db = new DbContext();
        // GET: Admin/DashBoards
        public ActionResult Index(string search, DateTime? start, DateTime? end, int? size, int? page)
        {
            var pageSize = size ?? 5; // số lượng record hiển thị trên một trang
            var pageNumber = page ?? 1; // số trang hiện tại
            ViewBag.search = search;
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();
            ViewBag.Order = db.Orders.ToList();
            ViewBag.OrderDetail = db.Oder_Detail.ToList();
            ViewBag.ListOrderDetail = db.Oder_Detail.OrderByDescending(m => m.create_at).Take(3).ToList();
            //ViewBag.ListOrder = db.Orders.Take(7).ToList();
            var listOrder = db.Orders.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                listOrder = db.Orders.Where(m => m.Account.Name.Contains(search)).ToList();
            }

            if (start != null && end != null)
            {
                start = start.Value.AddDays(1).AddTicks(-1);
                end = end.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.create_at >= start && m.create_at <= end && m.status.Equals("3")).ToList();
            }

            if (start != null && end == null)
            {
                start = start.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.oder_date >= start && m.status.Equals("3")).ToList();
            }

            if (start == null && end != null)
            {
                end = end.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.oder_date <= end && m.status.Equals("3")).ToList();
            }

            //Nếu có cả start, end và search
            if (!string.IsNullOrEmpty(search) && start != null && end != null)
            {
                start = start.Value.AddDays(1).AddTicks(-1);
                end = end.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.Account.Name.Contains(search) && m.oder_date >= start && m.oder_date <= end && m.status.Equals("3")).ToList();
            }

            //Nếu có cả start, end và search

            if (!string.IsNullOrEmpty(search) && start != null && end == null)
            {
                start = start.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.Account.Name.Contains(search) && m.oder_date >= start && m.status.Equals("3")).ToList();
            }

            //Nếu có cả start, end và search

            if (!string.IsNullOrEmpty(search) && start == null && end != null)
            {
                end = end.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.Account.Name.Contains(search) && m.oder_date <= end && m.status.Equals("3")).ToList();
            }
            return View(listOrder.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ExportExcel(DateTime? start, DateTime? end)
        {
            var listOrder = db.Orders.Include("OrderAddress").ToList();
            if (start != null && end != null)
            {
                start = start.Value.AddDays(1).AddTicks(-1);
                end = end.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.create_at >= start && m.create_at <= end && m.status.Equals("3")).ToList();
            }
            if (start != null && end == null)
            {
                start = start.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.oder_date >= start && m.status.Equals("3")).ToList();
            }
            if (start == null && end != null)
            {
                end = end.Value.AddDays(1).AddTicks(-1);
                listOrder = db.Orders.Where(m => m.oder_date <= end && m.status.Equals("3")).ToList();
            }

            var wb = new ClosedXML.Excel.XLWorkbook();

            //Tạo ws

            var ws = wb.Worksheets.Add("Danh sách đơn hàng");

            //Tạo header
            ws.Cell("A1").Value = "Kizspy Corporation\r\n Contact us:\r\n 2021010104@sv.ufm.edu.vn\r\n 0383667439";
            ws.Range("A1:C1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Range("A1:C1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("A1:C1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range("A1:C1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range("A1:C1").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Range("A1:C1").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("A1:C1").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Range("A1:C1").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
            ws.Range("A1:C1").Style.Font.Bold = true;
            ws.Range("A1:C1").Style.Font.FontSize = 14;
            ws.Range("A1:C1").Style.Font.FontColor = XLColor.Red;
            ws.Range("A1:C1").Style.Fill.BackgroundColor = XLColor.White;

            ws.Cell("A2").Value = "Doanh thu bán hàng từ ngày " + start.Value.ToString("dd/MM/yyyy") + " đến ngày " + end.Value.ToString("dd/MM/yyyy");
            ws.Cell("A2").Style.Font.Bold = true;
            ws.Cell("A2").Style.Font.FontSize = 16;
            ws.Cell("A2").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Range("A2:G2").Merge();
            ws.Cell("A3").Value = "Order ID";
            ws.Cell("A3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("B3").Value = "Ngày đặt hàng";
            ws.Cell("B3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("C3").Value = "Tên khách hàng";
            ws.Cell("C3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("D3").Value = "Số điện thoại";
            ws.Cell("D3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("E3").Value = "Địa chỉ";
            ws.Cell("E3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("F3").Value = "Tổng tiền";
            ws.Cell("F3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell("G3").Value = "Trạng thái";
            ws.Cell("G3").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

            for (int i = 0; i < listOrder.Count; i++) //Duyệt danh sách
            {
                ws.Cell(i + 4, 1).Value = listOrder[i].order_id;
                ws.Cell(i + 4, 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 4, 2).Value = listOrder[i].oder_date.ToString("dd/MM/yyyy");
                ws.Cell(i + 4, 2).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 4, 3).Value = listOrder[i].Account.Name;
                ws.Cell(i + 4, 3).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 4, 4).Value = listOrder[i].Account.Phone;
                ws.Cell(i + 4, 4).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 4, 5).Value = listOrder[i].OrderAddress.content;
                ws.Cell(i + 4, 5).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 4, 6).Value = listOrder[i].total;
                ws.Cell(i + 4, 6).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                ws.Cell(i + 4, 7).Value = listOrder[i].status == "1" ? "Đang chờ xử lý" : listOrder[i].status == "2" ? "Đang giao hàng" : listOrder[i].status == "3" ? "Đã giao hàng" : "Đã hủy";
                ws.Cell(i + 4, 7).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            }

            ws.Cell(listOrder.Count + 4, 5).Value = "Tổng tiền thanh toán";
            ws.Cell(listOrder.Count + 4, 5).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            ws.Cell(listOrder.Count + 4, 5).Style.Font.Bold = true;
            ws.Cell(listOrder.Count + 4, 6).Value = listOrder.Sum(m => m.total);
            ws.Cell(listOrder.Count + 4, 6).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
            //Add border cho sheet doanh thu
            ws.RangeUsed().Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            //Add màu nền cho sheet doanh thu
            ws.RangeUsed().Style.Fill.BackgroundColor = XLColor.LightGray;
            //Border cho table
            ws.Columns().AdjustToContents();

            string nameFile = "ExportOrder_" + start.Value.ToString("ddMMyyyy") + "_" + end.Value.ToString("ddMMyyyy") + ".xlsx";
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