/*
 * Javascript xử lý chung cho toàn bộ trang web
 */
// (function () {
// 	// Khởi tạo thông tin giỏ hàng khi refresh trang
// 	var cartEle = $("#goToCart");
// 	if (cartEle.length) {
// 		var cartCount = Cookie.countProduct();
// 		if (cartCount > 0) {
// 			$(".lblCartCount").text(cartCount);
// 		}
// 	}
// })();
//Khởi tạo thông tin giỏ hàng và số lượng
var cartEle = $("#goToCart");
//Lấy số lượng sản phẩm trong giỏ hàng từ cookie
if (cartEle.length) {
	var cartCount = Cookie.countProduct();
	console.log(cartCount);
	if (cartCount > 0) {
		$(".lblCartCount").text(cartCount);
	}
}