////1. Thêm mới 
////2. Chỉnh sửa
////3. Xóa
////------------------------------------------------/
const createModalDiscount = $('#create-modal-discount')
const editModalDiscount = $('#edit-modal-discount')
const deleteModalDiscount = $('#delete-modal-discount');
let idDiscount;

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
        typeof define === 'function' && define.amd ? define(['exports'], factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, factory(global.vn = {}));
}(this, (function (exports) {
    'use strict';
    var fp = typeof window !== "undefined" && window.flatpickr !== undefined
        ? window.flatpickr
        : {
            l10ns: {},
        };
    var Vietnamese = {
        weekdays: {
            shorthand: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
            longhand: [
                "Chủ nhật",
                "Thứ hai",
                "Thứ ba",
                "Thứ tư",
                "Thứ năm",
                "Thứ sáu",
                "Thứ bảy",
            ],
        },
        months: {
            shorthand: [
                "01",
                "02",
                "03",
                "04",
                "55",
                "06",
                "07",
                "08",
                "09",
                "10",
                "11",
                "12",
            ],
            longhand: [
                "Tháng một",
                "Tháng hai",
                "Tháng ba",
                "Tháng tư",
                "Tháng năm",
                "Tháng sáu",
                "Tháng bảy",
                "Tháng tám",
                "Tháng chín",
                "Tháng mười",
                "Tháng mười một",
                "Tháng mười hai",
            ],
        },
        firstDayOfWeek: 1,
        rangeSeparator: " đến ",
    };
    fp.l10ns.vn = Vietnamese;
    var vn = fp.l10ns;
    exports.Vietnamese = Vietnamese;
    exports.default = vn;
    Object.defineProperty(exports, '__esModule', { value: true });
})));

$(".pick-date-date").flatpickr({
    "locale": "vn",
    enableTime: true,
    dateFormat: "m-d-Y H:i",
    time_24hr: true,
    minDate: "today",
    defaultHour: new Date().getHours(),
    defaultMinute: new Date().getMinutes()
});
const createModal = $('#create-modal')
const editModal = $('#edit-modal')
const deleteModal = $('#delete-modal');
let discoundID;
$('.dimis-modal').click(function () {
    createModal.modal('hide');
    editModal.modal('hide');
    deleteModal.modal('hide');
});
//1. thêm mới
$('#create__open').click(function () {
    createModal.modal('show');
});
$('#create__save').click(function () {
    const price = $('#create__input-price').val();
    const code = $('#create__input-code').val(); 
    const quan = $('#create__input-quan').val();
    const start = $('#create__input-start').val();
    const end = $('#create__input-expired').val();
    if (price == "") {
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
            title: 'Nhập mức giảm'
        })
        return;
    }
    else if (start == "") {
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
            title: 'Nhập ngày bắt đầu'
        })
        return;
    }
    else if (end == "") {
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
            title: 'Nhập ngày kết thúc'
        })
        return;
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Discounts/Create',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ discountPrice: price, discountStart: start, discountEnd: end, discountCode: code, quantity:quan }),
            dataType: "json",
            success: function (result) {
                if (result == "success") {
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
                        icon: 'success',
                        title: 'Thêm thành công'
                    })
                    setTimeout(function () {
                        window.location.reload();
                    }, 500);
                    createModalLogin.modal('hide');
                    return;
                }
            },
            error: function () {
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
                    icon: 'error',
                    title: 'Thêm thất bại'
                })
            }
        });
    }
});

//2. Chỉnh sửa
var regex = /(\d)(?=(\d{3})+(?!\d))/g;
var editOpen = function (id, discountprice, code, start, end,quan) {
    editModal.find('#edit__input-price').val(discountprice);
    editModal.find('#edit__input-code').val(code);
    editModal.find('#edit__input-quan').val(quan);
    editModal.find('#edit__input-start').val(start);
    editModal.find('#edit__input-expired').val(end);
    editModal.modal('show');
    discoundID = id;
}
$('#edit__save').click(function () {
    const price = $('#edit__input-price').val();
    const code = $('#edit__input-code').val();
    const quan = $('#edit__input-quan').val();
    const start = $('#edit__input-start').val();
    const end = $('#edit__input-expired').val();
    if (price == "") {
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
            title: 'Nhập mức giảm'
        })
        return;
    }
    else if (start == "") {
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
            title: 'Nhập ngày bắt đầu'
        })
        return;
    }
    else if (end == "") {
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
            title: 'Nhập ngày kết thúc'
        })
        return;
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Discounts/Edit',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: discoundID, discountPrice: price, discountStart: start, discountEnd: end, discountCode: code, quantity:quan  }),
            dataType: "json",
            success: function (result) {
                if (result == "exist") {
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
                        title: 'Tên đã tồn tại'
                    })
                    return;
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
                        icon: 'success',
                        title: 'Cập nhật thành công'
                    })
                    var newTotal = price;
                    newTotal += "";
                    $('#discount-price_' + discoundID).text(newTotal.replace(regex, "$1.") + "₫");
                    $('#dis-name_' + discoundID).text('Giảm ' + newTotal.replace(regex, "$1.") + "₫" + ' Từ ' + start+ ' => ' + end);
                    $('#discount-code_'+discoundID).text(code);
                    $('#discount-quan_'+discoundID).text(quan);
                    editModal.modal('hide');
                    return;
                }
            },
            error: function () {
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
                    icon: 'error',
                    title: 'Chỉnh sửa thành công'
                })
            }
        });
    }
});
//3. Xóa 
var deleteOpen = function (id, name) {
    deleteModal.find('#delete__name').text(name);
    deleteModal.modal('show');
    discoundID = id;
}
$('#delete__submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Discounts/Delete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: discoundID }),
        dataType: "json",
        success: function (result) {
            if (result == "delete") {
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
                deleteModal.modal('hide');
                $("#item_" + discoundID).remove();
                return;
            }
        },
        error: function () {
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
                title: 'Xóa thất bại'
            })
        }
    });
});
