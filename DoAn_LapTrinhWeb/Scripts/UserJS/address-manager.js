//1. Thêm mới địa chỉ
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
var createmodal = $('#ModalCreate');
$('#popupcreateaddress').click(function () {
    var myobj = $('.nice-select');
    myobj.remove();
    createmodal.modal('show');
})
$('#closemodal').click(function () {
    editModal.modal('hide');
});
$('#closemodal1').click(function () {
    editModal.modal('hide');
});
$('#closemodal4').click(function () {
    editModal2.modal('hide');
});
$('#closemodal5').click(function () {
    editModal2.modal('hide');
});
$('#closemodal3').click(function () {
    createmodal.modal('hide');
});
var SaveAddress = function () {
    var username = $("#address_name").val();
    var phonenumber = $("#address_phone").val();
    var province = $("#province").val();
    var disctric = $("#district").val();
    var address = $("#address_content").val();
    var ward = $("#ward").val();
    if (username == "" || phonenumber == "" || province == "" || disctric == "" || ward == "" || address == "") {
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
    var data = $("#create_address").serialize();
    $.ajax({
        type: "POST",
        url: "/Account/AddressCreate", //kiểm tra tồn tại username, username và password đã trùng chưa (kiểm tra ở acition checksignin của cotroller Account)
        data: data,
        success: function (result) {
            createmodal.modal('hide');
            if (result == true) {
                setTimeout(function () {
                    window.location.reload();
                }, 1);
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
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
//2. Sửa thông tin địa chỉ
$(document).ready(function () {
    $('#province_edit').change(function () {
        $('#district_edit').removeClass('cursor-disable');
        $('#district_edit').removeAttr('disabled');
        $.get("/Account/GetDistrictsList", { province_id: $('#province_edit').val() }, function (data) {
            $('#district_edit').html("<option value>Quận/ Huyện</option>");
            $.each(data, function (index, row) {
                $('#district_edit').append("<option value='" + row.district_id + "'>" + row.type + " " + row.district_name + "</option>")
            });
        });
    })
    $('#district_edit').change(function () {
        $('#ward_edit').removeClass('cursor-disable');
        $('#ward_edit').removeAttr('disabled');
        $.get("/Account/GetWardsList", { district_id: $('#district_edit').val() }, function (data) {
            $('#ward_edit').html("<option value>Phường/ Xã</option>");
            $.each(data, function (index, row) {
                $('#ward_edit').append("<option value='" + row.ward_id + "'>" + row.type + " " + row.ward_name + "</option>")
            });
        });
    })
});
var ide;
var editModal = $('#EditAddress');
var EditAddress = function (id, username, phonenumber, province_id, district_id, ward_id, address_content) {
    var myobj = $('.nice-select');
    myobj.remove();
    ide = id;
    $('#province_edit').val(province_id);
    $('#address_name_edit').val(username);
    $('#district_edit').val(district_id);
    $('#ward_edit').val(ward_id);
    $('#address_content_edit').val(address_content);
    $('#address_phone_edit').val(phonenumber);
    editModal.modal('show');
}
$('#confirmeditBtn').click(function () {
    var _province_id = $('#province_edit').val();
    var _username = $('#address_name_edit').val();
    var _district_id = $('#district_edit').val();
    var _ward_id = $('#ward_edit').val();
    var _address_content = $('#address_content_edit').val();
    var _phonenumber = $('#address_phone_edit').val();
    if (_province_id == "" || _username == "" || _district_id == "" || _ward_id == "" || _address_content == "" || _phonenumber == "") {
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
            icon: 'warning',
            title: 'Hãy nhập đầy đủ thông tin'
        })
        return false;
    }
    else if (_address_content.length > 50) {
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
            icon: 'warning',
            title: 'Địa chỉ cụ thể không quá 50 ký tự'
        })
        $('#confirmeditBtn').attr('disabled');
        return false;
    }
    else if (_phonenumber.length < 10 || _phonenumber.length>10) {
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
            icon: 'warning',
            title: 'Số điện thoại phải đúng 10 chữ số'
        })
        return false;
    }
    else if (_username.length > 20) {
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
            icon: 'warning',
            title: 'Họ tên không quá 20 ký tự'
        })
        return false;
    }
    else {
        $.ajax({
            type: "post",
            url: '/Account/AddressEdit',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, username: _username, province_id: _province_id, district_id: _district_id, ward_id: _ward_id, address_content: _address_content, phonenumber: _phonenumber}),
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    editModal.modal('hide');
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
                        title: 'Cập nhật thành công'
                    })
                    var province = $('#province_edit').find(":selected");
                    var district = $('#district_edit').find(":selected");
                    var ward = $('#ward_edit').find(":selected");
                    $('#item-user-name_' + ide).text(_username);
                    $('#item-phone-number_' + ide).text(_phonenumber);
                    $('#item-content_' + ide).text(_address_content + ', ' + ward.text() + ', ' + district.text() + ', ' + province.text());
                    return;
                }
                else {
                    editModal.modal('hide');
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top',
                        showConfirmButton: false,
                        timer: 2500,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })
                    Toast.fire({
                        icon: 'error',
                        title: 'Lối! không tìm thấy ID'
                    })
                    return false;
                }
            },
            error: function () {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'warning',
                    title: 'Sửa thất bại'
                })
            }
        });
    }
});

//3. Đổi địa chỉ mặc định
var defaultmodal = $('#defaultModal');
var defaultID;
var defaultConfirm = function (id) {
    defaultmodal.modal('show');
    defaultID = id;
}
$("#cancle-defalt").click(function () {
    defaultmodal.modal('hide');
});
$('#btn-default-submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Account/DefaultAddress',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: defaultID }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
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
                    icon: 'success',
                    title: 'Cập nhật địa chỉ mặc định thành công'
                })
                defaultmodal.modal('hide');
            } else {
                defaultmodal.modal('hide');
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
                    title: 'Địa chỉ đang là mặc định!'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'error',
                title: '!Lỗi'
            })
        }
    });
});

//4. Xóa địa chỉ
var delmodal = $('#deleteModal');
var idde;
var deleteConfirm = function (id) {
    delmodal.modal('show');
    idde = id;
}
$("#cancle_delete_address").click(function () {
    delmodal.modal('hide');
});
$('#btndelete_address').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Account/AddressDelete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $("#item_" + idde).remove();
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
                    title: 'Xóa thành công'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'error',
                title: '!Lỗi'
            })
        }
    });
});
//5. Check thông tin trước khi lưu đơn hàng
$('#save-order').click(function () {
    const provinceID = $('#province').val();
    const districtID = $('#district').val();
    const wardID = $('#ward').val();
    const userName = $('#user-name').val();
    const userPhoneNumber = $('#phone-number').val();
    const content = $('#content').val();
    if (provinceID == "" || districtID == "" || wardID == "" || userName == "" || userPhoneNumber == "" || content == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 3000,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'error',
            title: 'Nhập thông tin còn thiếu'
        })
        return false;
    }
    else {
        $('#save-order').attr('type', 'submit');
    }
});