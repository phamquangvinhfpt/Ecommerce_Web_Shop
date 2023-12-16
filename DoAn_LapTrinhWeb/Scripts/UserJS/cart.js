/*
 * Format giỏ hàng: product_{id}={quantity}
 */
function reGex()
{
	return regex = /(\d)(?=(\d{3})+(?!\d))/g;//covert giá sang dấu chấm
}
//Nhập số lượng vào ô nhập và thay đổi
function Update_quan_mouse_ev() {
	var id = $('.qty-input').data("id");
	//no-pointer-events không cho click thực hiện 1 action
	$(".btn-inc").removeClass('no-pointer-events');
	var quan = $('.qty-input').val();
	if (quan != "") {
		Cookie.set("product_" + id, quan, 30);//set sản phẩm lên cookie, sản phẩm tự xóa sau 30 ngày
		updateOrderPrice();
	}
}
//các animation khi thay đổi số lượng sản phẩm ở ô input
$(".qty-input").mouseleave(function () {
	Update_quan_mouse_ev()
});
$(".qty-input").mouseover(function () {
	Update_quan_mouse_ev()
});
$(".qty-input").change(function (ev) {
	Update_quan_mouse_ev()
})
$(".qty-input").mouseout(function (ev) {
	Update_quan_mouse_ev()
})

// Button giảm số lượng
$(".btn-dec").click(function (ev) {
	$(".btn-inc").removeClass('no-pointer-events');
	var quanInput = $(ev.currentTarget).next();
	var id = quanInput.data("id");
	var price = quanInput.data("price");
	var quan = Number(quanInput.val());
	if (quan > 1) {
		quan = quan - 1;
		Cookie.set("product_" + id, quan, 30);//set sản phẩm lên cookie, sản phẩm tự xóa sau 30 ngày
		quanInput.val(quan);//update số lượng lên view
		updateOrderPrice();//update giá lên view
		var newTotal4 = 0;
		newTotal4 = quan*price;
		newTotal4+= "";
		$('#total3-' + id).text(newTotal4.replace(regex, "$1.") + "₫");
	}
});

// Button tăng số lượng
$(".btn-inc").click(function (ev) {
	var quanInput = $(ev.currentTarget).prev();
	var maxquan = quanInput.data("quan");
	var id = quanInput.data("id");
	var price = quanInput.data("price");
	var quan = Number(quanInput.val());
	if (quan < 1)//số lượng sản phẩm không được = 0 
	{
		quan = 1;
		Cookie.set("product_" + id, quan, 30);
		quanInput.val(quan);
		updateOrderPrice();
	}
	else if (quan >= maxquan) {
		//nếu quan >= maxquan(get từ trên front) thì quan lấy luôn số lượng của maxquan và thông báo cho user về số lượng đã đạt giới hạn
		$(".btn-inc").addClass('no-pointer-events');
		const Toast = Swal.mixin({
			toast: true,
			position: 'top-end',
			showConfirmButton: false,
			timer: 2000,
			didOpen: (toast) => {
				toast.addEventListener('mouseenter', Swal.stopTimer)
				toast.addEventListener('mouseleave', Swal.resumeTimer)
			}
		})
		Toast.fire({
			icon: 'error',
			title: 'Số lượng đã đạt giới hạn'
		})
	}
	else {
		quan = quan + 1;
		Cookie.set("product_" + id, quan, 10);		
		quanInput.val(quan);
		updateOrderPrice();
		reGex();
		var newTotal2 = 0;
		newTotal2 = quan * price;
		newTotal2+= "";
		$('#total3-'+id).text(newTotal2.replace(regex, "$1.") + "₫");
	}
});

// Button xóa sản phẩm khỏi giỏ hàng
$(".js-delete").click(function (ev) {
	Swal.fire({
		title: 'Xác nhận xóa?',
		text: "Xóa sản phẩm khỏi giỏ hàng",
		icon: 'warning',
		showCancelButton: true,
		cancelButtonColor: '#d33',
		confirmButtonColor: '#3085d6',
		cancelButtonText: 'Hủy',
		confirmButtonText: 'Xác nhận!',
		reverseButtons: true
	}).then((result) => {
		if (result.isConfirmed) {
			var id = $(ev.currentTarget).data("id");
			var item = $(ev.currentTarget).closest(".item");
			item.remove();
			Cookie.remove("product_" + id);
			var productCount = Cookie.countWithPrefix("product")
			$("#cartCount").text(productCount);
			$(".lblCartCount").text(productCount == 0 ? "0" : productCount);
			$('#emty-product').attr('class', productCount == 0 ? "d-block" : "d-none");
			updateOrderPrice();
			const Toast = Swal.mixin({
				toast: true,
				position: 'top-end',
				showConfirmButton: false,
				timer: 1500,
				didOpen: (toast) => {
					toast.addEventListener('mouseenter', Swal.stopTimer)
					toast.addEventListener('mouseleave', Swal.resumeTimer)
				}
			})
			Toast.fire({
				icon: 'success',
				title: 'Xóa thành công'
			})
		}
	})
});

//cập nhật giỏ hảng
function updateOrderPrice() {
	$(".lblCartCount").text(Cookie.countProduct());
	var quanInputs = $("input.qty-input");//số lượng của sản phẩm ở ô input
	var newTotal = 0;
	var totalWithFee;
	quanInputs.each(function (i, e) {
		var price = Number($(e).data('price'));
		var quan = Number($(e).val());
		newTotal += price * quan;
	});
	var eleDiscount = $("#discount");
	var discount = 0;
	if (eleDiscount.attr("data-price") == null) {
		totalWithFee = newTotal + 30000;
	}
	else
	{
        if (eleDiscount.attr("data-price") < (newTotal)) {
            discount = Number(eleDiscount.attr("data-price"));
            totalWithFee = newTotal + 30000 - discount;
        }
        else//nếu giảm giá lớn hơn tổng trị giá đơn hàng(tạm tính) thì tổng tiền bằng 30k(30k là phí ship chỉ trừ tiền sản phẩm chứ không trừ tiền ship)
        {
            discount = Number(eleDiscount.attr("data-price"));
            totalWithFee = 30000;
        }
    }
	totalWithFee += "";
	newTotal += "";
	discount += "";
	reGex();
	$(".totalPrice").text(newTotal.replace(regex, "$1.") + "₫");//gắn tổng trị giá đơn hàng lên view
	$(".totalPriceWithFee").text(totalWithFee.replace(regex, "$1.") + "₫");
	$("#discount").text(discount.replace(regex, "$1.") + "₫");
};

$(".js-checkout").click(function (ev) {
	ev.preventDefault();
	var count_product = Cookie.countWithPrefix("product");
	$.get("/Account/UserLogged", {},
		function (isLogged, textStatus, jqXHR) {
			if (!isLogged) {
				Swal.fire({
					title: 'Yêu cầu đăng nhập?',
					text: "Vui lòng đăng nhập để thực hiện được chức năng này",
					icon: 'error',
					showCancelButton: true,
					cancelButtonColor: '#d33',
					confirmButtonColor: '#3085d6',
					cancelButtonText: 'Hủy',
					confirmButtonText: 'Đăng nhập',
					reverseButtons: true
				}).then((result) => {
					if (result.isConfirmed) {
						location.href = 'login';
					}
				})
				return;
			}
			else
				if (count_product == 0) {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Giỏ hàng chưa có sản phẩm!'
				})
            }
            else {
                location.href = ev.currentTarget.href;
            }
		},
		"json"
	);
});

$(".btn-submitcoupon").click(function (ev) {
	var input = $(ev.currentTarget).prev();
	var _code = input.val().trim();
	if (_code.length) {
		$.getJSON("/cart/UseDiscountCode", { code: _code },
			function (data, textStatus, jqXHR) {
				if (data.success) {
					$("#discount").attr("data-price", data.discountPrice);
					$("#discount").attr("class", 'text-success');
					updateOrderPrice();
					const Toast = Swal.mixin({
						toast: true,
						position: 'top-end',
						showConfirmButton: false,
						timer: 2500,
						didOpen: (toast) => {
							toast.addEventListener('mouseenter', Swal.stopTimer)
							toast.addEventListener('mouseleave', Swal.resumeTimer)
						}
					})
					Toast.fire({
						icon: 'success',
						title: 'Mã giảm giá đã được áp dụng thành công!'
					})
				} else {
					const Toast = Swal.mixin({
						toast: true,
						position: 'top-end',
						showConfirmButton: false,
						timer: 2500,
						didOpen: (toast) => {
							toast.addEventListener('mouseenter', Swal.stopTimer)
							toast.addEventListener('mouseleave', Swal.resumeTimer)
						}
					})
					Toast.fire({
						icon: 'error',
						title: 'Mã giảm giá không hợp lệ!'
					})
				}
			}
		);
	}
})