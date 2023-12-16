// Sự kiện click [Thêm vào giỏ] ở trang detail
$(".btnAddToCart").click(function (ev) {
	$('body').css('padding-right', '0px!important');
	var exdays = 30;
	// Sản phẩm trong giỏ hàng sẽ tự động xóa sau 30 ngày
	var id = $(ev.currentTarget).data("id");
	var quan = $("#quantity").val();
	var cookieName = "product_" + id;
	var productInCart = Cookie.get(cookieName);
	if (productInCart) {
		// Có sản phẩm trong giỏ hàng
		quan = Number(productInCart) + Number(quan);
	}
	Cookie.set(cookieName, quan, exdays);
	var cartCountUI = $(".lblCartCount");
	if (cartCountUI.length) {
		cartCountUI.text(Cookie.countProduct());
	} else {
		$(".lblCartCount").text("1");
	}
	// Hiển thị thông báo
	Swal.fire({
		position: 'center',
		icon: 'success',
		title: 'Thêm vào giỏ thành công',
		showConfirmButton: false,
		timer: 1000
	})
});
$(".buyNow").click(function (ev) {
	$('body').css('padding-right', '0px!important');
	var exdays = 30;
	// Sản phẩm trong giỏ hàng sẽ tự động xóa sau 30 ngày
	var id = $(ev.currentTarget).data("id");
	var quan = $("#quantity").val();
	var cookieName = "product_" + id;
	var productInCart = Cookie.get(cookieName);
	if (productInCart) {
		// Có sản phẩm trong giỏ hàng
		quan = Number(productInCart) + Number(quan);
	}
	Cookie.set(cookieName, quan, exdays);
	var cartCountUI = $(".lblCartCount");
	if (cartCountUI.length) {
		cartCountUI.text(Cookie.countProduct());
	} else {
		$(".lblCartCount").text(1);
	}
	Swal.fire({
		position: 'center',
		icon: 'success',
		title: 'Thêm vào giỏ thành công',
		showConfirmButton: false,
		timer: 1000
	})
	setTimeout(function () {
		window.location.href = 'cart';
	}, 1000);
	// Hiển thị thông báo
});


