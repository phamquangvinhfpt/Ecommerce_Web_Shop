$(document).ready(function () {
    var myobj = $('.nice-select');
    myobj.remove();
});
//Thêm quận huyện thị xã khi nhập
$(document).ready(function () {
    $('#province').change(function () {
        $('#district').removeClass('cursor-disable');
        $('#district').removeAttr('disabled');
        $.get("/Account/GetDistrictsList", { province_id: $('#province').val() }, function (data) {
            $('#district').html("<option value>Quận/ Huyện</option>");
            $.each(data, function (index, row) {
                $('#district').append("<option value='" + row.district_id + "'>" + row.type + " " + row.district_name + "</option>")
            });
        });
    })
    $('#district').change(function () {
        $('#ward').removeClass('cursor-disable');
        $('#ward').removeAttr('disabled');
        $.get("/Account/GetWardsList", { district_id: $('#district').val() }, function (data) {
            $('#ward').html("<option value>Phường/ Xã</option>");
            $.each(data, function (index, row) {
                $('#ward').append("<option value='" + row.ward_id + "'>" + row.type + " " + row.ward_name + "</option>")
            });
        });
    })
});
//thêm mới địa chỉ
var createmodaladd = $('#ModalOrderAddCreate');
$('#popupcreateaddress').click(function () {
    createmodaladd.modal('show');
})
$('#closemodal3').click(function () {
    createmodaladd.modal('hide');
});
//thêm address_order vào database
var SaveAddressOrder = function () {
    var username = $("#address_name").val();
    var phonenumber = $("#address_phone").val();
    var province = $("#province").val();
    var province_name = $("#province :selected").text();
    var disctric = $("#district").val();
    var disctric_name = $("#district :selected").text();
    var address = $("#address_content").val();
    var ward = $("#ward").val();
    var ward_name = $("#ward :selected").text();
    if (username == "" || phonenumber == "" || province == "" || disctric == "" || ward == "" || address == "")
    {
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
            icon: 'warning',
            title: 'Nhập thông tin còn thiếu'
        })
        return false;
    }
    else if (phonenumber.length < 10 || phonenumber.length > 10) {
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
            icon: 'warning',
            title: 'Số điện thoại phải đúng 10 chữ số'
        })
        return false;
    }
    else if (username.length > 20) {
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
            icon: 'warning',
            title: 'Họ tên không quá 20 ký tự'
        })
        return false;
    }
    else if (address.length > 50) {
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
            icon: 'warning',
            title: 'Địa chỉ cụ thể không quá 50 ký tự'
        })
        return false;
    }
    else {
        var data = $("#create_address__order").serialize();
        $.ajax({
            type: "POST",
            url: "/Account/AddressCreate", //kiểm tra tồn tại username, username và password đã trùng chưa (kiểm tra ở acition checksignin của cotroller Account)
            data: data,
            success: function (result) {
                createmodaladd.modal('hide');
                if (result == true) {
                    $(".order_address_name").attr("value", username);
                    $(".order_address_phone").attr("value", phonenumber);
                    $(".ck_address_province").attr("value", province_name);
                    $(".ck_address_district").attr("value", disctric_name);
                    $(".ck_address_ward").attr("value", ward_name);
                    $(".ck_address_content").attr("value", address);

                    $(".btn_submit_order").attr("id", "submit_order");
                    $(".btn_submit_order").addClass("order_now");
                    $(".btn_submit_order").attr("type", "submit");
                    $(".message_order").text("(Đặt hàng để xác nhận thanh toán)");
                    $(".btn_submit_order").removeAttr("disabled");
                    $(".create_order_address").removeAttr("hidden");
                    $(".order_now").removeClass("btn_submit_order");
                    $(".ck_notexist_address").remove();
                    $(".order_address").text(address + ", " + ward_name + ", " + disctric_name + ", " + province_name);
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top',
                        showConfirmButton: false,
                        timer: 2000,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })
                    Toast.fire({
                        icon: 'success',
                        title: 'Thêm địa chỉ thành công'
                    })
                }
                else {
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top',
                        showConfirmButton: false,
                        timer: 1000,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })
                    Toast.fire({
                        icon: 'error',
                        title: 'Lỗi'
                    })
                }
            }
        })
    }
}
//đổi địa chỉ nhận hàng 
var change_od_add = $("#ckchangeaddress");
$('#op_ckchangeaddress').click(function () {
    change_od_add.modal('show')
})
$('.close_modal').click(function () {
    change_od_add.modal('hide')
})
var Change_order_address_confirm = function () {
    var data = $("#change_order__address").serialize();
    $.ajax({
        type: "POST",
        url: "/Cart/ChangeAdressOrder", 
        data: data,
        success: function (result) {
            change_od_add.modal('hide')
            if (result == true) {
                var _username_order, _phonenumber_order, _address_content_order
                $('input[name="account_address_id"]:radio:checked').each(function () {
                    _username_order = $(this).parent().find('.get_acc_username').text();
                    _phonenumber_order = $(this).parent().find('.get_acc_phone_num').text();
                    _address = $(this).parent().find('.get_address').text();
                    _address_province = $(this).parent().find('.get_address_province').text();
                    _address_district = $(this).parent().find('.get_address_district').text();
                    _address_ward = $(this).parent().find('.get_address_ward').text();
                    _address_content = $(this).parent().find('.get_address_content').text();
                });
                $(".ck_address").text(_address);

                $(".ck_username").val(_username_order);
                $(".ck_phone_num").val(_phonenumber_order);
                $(".ck_address_province").val(_address_province);
                $(".ck_address_district").val(_address_district);
                $(".ck_address_ward").val(_address_ward);
                $(".ck_address_content").val(_address_content);
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'success',
                    title: 'Cập nhật địa chỉ thành công'
                })
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Lỗi'
                })
            }
        }
    })
}
