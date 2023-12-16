using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class AccountController : Controller
    {
        private DbContext db = new DbContext();
        //View đăng nhập
        public ActionResult Login(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("Login", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //Code xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModels model,string returnUrl)
        {
            model.Password = Crypto.Hash(model.Password);
            Account account = db.Accounts.FirstOrDefault(m => m.Email == model.Email && m.password == model.Password && m.status == "1");
            if (account != null)
            {
                LoggedUserData userData = new LoggedUserData
                {
                    UserId = account.account_id,
                    Name = account.Name,
                    Email = account.Email,
                    RoleCode = account.Role,
                    Avatar = account.Avatar
                };
                Notification.setNotification1_5s("Đăng nhập thành công", "success");
                FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(userData), false);
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            Notification.setNotification3s("Email, mật khẩu không đúng, hoặc tài khoản bị vô hiệu hóa", "error");
            return View(model);
        }
        //Đăng xuất tài khoản
        public ActionResult Logout(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("Logout", new { returnUrl = Request.UrlReferrer.ToString() });//tạo url khi đăng xuất, đăng xuất thành công thì quay lại trang trước đó
            }
            FormsAuthentication.SignOut();
            Notification.setNotification1_5s("Đăng xuất thành công", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        //View đăng ký
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //Code xử lý đăng ký
       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModels model,Account account)
        {
            string fail = "";
            string success = "";
            var checkemail = db.Accounts.Any(m => m.Email == model.Email);
            if (checkemail)
            {
                fail = "email đã được sử dụng";
                return View();
            }
            account.Role = Const.ROLE_MEMBER_CODE; //admin quyền là 0: thành viên quyền là 1             
            account.status = "1";
            account.Role = 1;
            account.Email = model.Email;
            account.create_by = model.Email;
            account.update_by = model.Email;
            account.Name = model.Name;
            account.Phone = model.PhoneNumber;
            account.update_at = DateTime.Now;
            account.Avatar = "/Content/Images/logo/icon.png";
            db.Configuration.ValidateOnSaveEnabled = false;
            account.password = Crypto.Hash(model.Password); //mã hoá password
            account.create_at = DateTime.Now; //thời gian tạo tạo khoản
            db.Accounts.Add(account);
            db.SaveChanges(); //add dữ liệu vào database
            success = "<script>alert('Đăng ký thành công');</script>";
            ViewBag.Success = success;
            ViewBag.Fail = fail;
            return RedirectToAction("Login","Account");
        }
        //View quên mật khẩu
        public ActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //Code xử lý quên mật khẩu
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModels model)
        {
            Account account = db.Accounts.Where(m => m.Email == model.Email).FirstOrDefault(); // kiểm tra email đã trùng với email đăng ký tài khoản chưa, nếu chưa đăng ký sẽ trả về fail
            if (account != null)
            {
                //Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.Email, resetCode); // gửi code reset đến mail đã nhập ở form quên mật khẩu , kèm code resetpass,  tên tiêu đề gửi
                string sendmail = account.Email;
                account.Requestcode = resetCode; //request code phải giống reset code         
                db.Configuration.ValidateOnSaveEnabled = false; // tắt validdation
                db.SaveChanges();
                Notification.setNotification5s("Đường dẫn reset password đã được gửi, vui lòng kiểm tra email", "success");
            }
            else
            {
                Notification.setNotification1_5s("Email chưa tồn tại trong hệ thống", "error");
            }
            return View(model);
        }
        //View cập nhật mật khẩu
        public ActionResult Resetpassword(string id)
        {
            var user = db.Accounts.Where(a => a.Requestcode == id).FirstOrDefault();
            if (user != null && !User.Identity.IsAuthenticated)
            {
                ResetPasswordViewModels model = new ResetPasswordViewModels();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Code xử lý cập nhật mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModels model)
        {
                var user = db.Accounts.Where(m => m.Requestcode == model.ResetCode).FirstOrDefault();
            if (user != null)
            {
                user.password = Crypto.Hash(model.NewPassword);
                //sau khi đổi mật khẩu thành công khi quay lại link cũ thì sẽ không đôi được mật khẩu nữa 
                user.Requestcode = "";
                user.update_by = user.Email;
                user.update_at = DateTime.Now;
                user.status = "1";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                Notification.setNotification1_5s("Cập nhật mật khẩu thành công", "success");
                return RedirectToAction("Login");
            }
            return View(model);
        }
        //Gửi Email quên mật khẩu
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/Account/ResetPassword/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress(EmailConfig.emailID, EmailConfig.emailName); 
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = EmailConfig.emailPassword; //có thể thay bằng mật khẩu gmail của bạn
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "ResetPassword" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            string subject = "Cập nhật mật khẩu mới";
            body = body.Replace("{{viewBag.Confirmlink}}", link); //hiển thị nội dung lên form html
            body = body.Replace("{{viewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl));//hiển thị nội dung lên form html
            var smtp = new SmtpClient
            {
                Host = EmailConfig.emailHost, 
                Port = 587,
                EnableSsl = true, //bật ssl
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        //View cập nhật thông tin cá nhân
        [Authorize]     // Đăng nhập mới có thể truy cập
        public ActionResult Editprofile()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Accounts.Where(u => u.account_id == userId).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return View();
        }
        //Code xử lý cập nhật thông tin cá nhân
        [Authorize]// Đăng nhập mới có thể truy cập
        public JsonResult UpdateProfile(string userName,string phoneNumber)
        {
            bool result = false;
            var userId = User.Identity.GetUserId();
            var account = db.Accounts.Where(m => m.account_id == userId).FirstOrDefault();
            if (account != null)
            {
                account.account_id = userId;
                account.Name = userName;
                account.Phone = phoneNumber;
                account.update_by = userId.ToString();
                account.update_at = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //Cập nhật ảnh đại diện
        public JsonResult UpdateAvatar()
        {
            var userId = User.Identity.GetUserId();
            var account = db.Accounts.Where(m => m.account_id == userId).FirstOrDefault();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                fileName = fileName + extension;
                account.Avatar = "/Content/Images/"+ fileName;
                file.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), fileName));
                db.Configuration.ValidateOnSaveEnabled = false;
                account.update_at = DateTime.Now;
                db.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        //View thay đổi mật khẩu
        public ActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //Code xử lý Thay đổi mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModels model)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userID = User.Identity.GetUserId();
                model.NewPassword = Crypto.Hash(model.NewPassword);
                Account account = db.Accounts.FirstOrDefault(m=>m.account_id== userID);
                if (model.NewPassword == account.password)
                {
                    Notification.setNotification3s("Mật khẩu mới và cũ không được trùng!", "error");
                    return View(model);
                }
                account.update_at = DateTime.Now;
                account.update_by = User.Identity.GetEmail();
                account.password = model.NewPassword;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                Notification.setNotification3s("Đổi mật khẩu thành công", "success");
                return RedirectToAction("ChangePassword", "Account");
            }
            return View(model);
        }
        //Quản lý sổ địa chỉ
        public ActionResult Address()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userID = User.Identity.GetUserId();
                var address = db.AccountAddresses.Where(m => m.account_id == userID).ToList();
                ViewBag.Check_address = db.AccountAddresses.Where(m => m.account_id == userID).Count();
                ViewBag.ProvincesList = db.Provinces.OrderBy(m => m.province_name).ToList();
                ViewBag.DistrictsList = db.Districts.OrderBy(m => m.type).ThenBy(m => m.district_name).ToList();
                ViewBag.WardsList = db.Wards.OrderBy(m => m.type).ThenBy(m => m.ward_name).ToList();
                return View(address);
            }
            return RedirectToAction("Index", "Home");
        }
        //Thêm mới địa chỉ 
        public ActionResult AddressCreate(AccountAddress address)
        {
            bool result = false;
            var userid = User.Identity.GetUserId();
            var checkdefault = db.AccountAddresses.Where(m => m.account_id == userid).ToList();
            var limit_address = db.AccountAddresses.Where(m => m.account_id == userid).ToList();
            try
            {
                if (limit_address.Count() == 4)//tối đa 4 ký tự
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                foreach (var item in checkdefault)
                {
                    if (item.isDefault == true && address.isDefault == true)
                    {
                        item.isDefault = false;
                        db.SaveChanges();
                    }
                }
                address.account_id = userid;
                db.AccountAddresses.Add(address);
                db.SaveChanges();
                result = true;
                Notification.setNotification1_5s("Thêm thành công", "success");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //Sửa địa chỉ
        [HttpPost]
        public JsonResult AddressEdit(int id, string username, string phonenumber, int province_id, int district_id, int ward_id, string address_content)
        {
            var address = db.AccountAddresses.FirstOrDefault(m => m.account_address_id == id);
            bool result;
            if (address != null)
            {
                address.province_id = province_id;
                address.accountUsername = username;
                address.accountPhoneNumber = phonenumber;
                address.district_id = district_id;
                address.ward_id = ward_id;
                address.content = address_content;
                address.account_id = User.Identity.GetUserId();
                db.SaveChanges();
                result = true;               
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Thay đổi địa chỉ mặc định
        public JsonResult DefaultAddress(int id)
        {
            bool result = false;
            var userid = User.Identity.GetUserId();
            var address = db.AccountAddresses.FirstOrDefault(m => m.account_address_id == id);
            var checkdefault = db.AccountAddresses.ToList();
            if (User.Identity.IsAuthenticated && !address.isDefault==true)
            {
                foreach (var item in checkdefault)
                {
                    if (item.isDefault == true && item.account_id == userid)
                    {
                        item.isDefault = false;
                        db.SaveChanges();
                    }
                }
                address.isDefault = true;
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //Xóa địa chỉ
        [HttpPost]
        public JsonResult AddressDelete(int id)
        {
            bool result = false;
            try
            {
                var address = db.AccountAddresses.FirstOrDefault(m => m.account_address_id == id);
                db.AccountAddresses.Remove(address);
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }
        //lấy danh sách quận huyện
        public JsonResult GetDistrictsList(int province_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Districts> districtslist = db.Districts.Where(m => m.province_id == province_id).OrderBy(m => m.type).ThenBy(m => m.district_name).ToList();
            return Json(districtslist, JsonRequestBehavior.AllowGet);
        }
        //lấy danh sách phường xã
        public JsonResult GetWardsList(int district_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Wards> wardslist = db.Wards.Where(m => m.district_id == district_id).OrderBy(m => m.type).ThenBy(m => m.ward_name).ToList();
            return Json(wardslist, JsonRequestBehavior.AllowGet);
        }
        //Lịch sử mua hàng
        public ActionResult TrackingOrder(int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("TrackingOrder", GetOrder(page));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Chi tiết đơn hàng đã mua
        public ActionResult TrackingOrderDetail(int id)
        {
            List<Oder_Detail> order = db.Oder_Detail.Where(m => m.order_id == id).ToList();
            ViewBag.Order = db.Orders.FirstOrDefault(m => m.order_id == id);
            ViewBag.OrderID = id;
            if (User.Identity.IsAuthenticated)
            {
                return View(order);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //đánh số trang
        private IPagedList GetOrder(int? page)
        {
            var userId = User.Identity.GetUserId();
            int pageSize = 10;
            int pageNumber = (page ?? 1); //đánh số trang
            var list = db.Orders.Where(m => m.account_id == userId).OrderByDescending(m => m.order_id)
                .ToPagedList(pageNumber, pageSize);
            return list;
        }

        public ActionResult AddAddress()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult UserLogged()
        {
            // Được gọi khi nhấn [Thanh toán]
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }
    }
}