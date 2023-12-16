const UpdateModal = $('#update-order-modal');
const cancleModal = $('#cancle-modal');
let orderId;
let OrderStatus;
let statusName;
let att
$('.dimis-modal').click(function () {
    UpdateModal.modal('hide');
    cancleModal.modal('hide');
});
//1. Thay đổi trạng thái sang đang xử lý
var onProcessOpen = function (id) {
    UpdateModal.find('#update__id').text(id);
    UpdateModal.find('#update__status').text('Đang xử lý');
    UpdateModal.find('#icon').html('<i class="bi bi-cart-plus text-info" style="font-size:6rem"></i>');
    UpdateModal.modal('show');
    orderId = id;
    OrderStatus = '2';
    statusName = 'Đang xử lý';
}
//2. Thay đổi trạng thái sang hoàn thành
var CompletedOpen = function (id) {
    UpdateModal.find('#update__id').text(id);
    UpdateModal.find('#icon').html('<i class="bi bi-cart-check text-success" style="font-size:6rem"></i>');
    UpdateModal.find('#update__status').text('Hoàn thành');
    UpdateModal.modal('show');
    orderId = id;
    OrderStatus = '3';
    statusName = 'Hoàn thành';
}

$('#update__submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Orders/UpdateOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: orderId, status: OrderStatus }),
        dataType: "json",
        success: function (result) {
            if (result == "success") {
                if (OrderStatus == '2') {
                    $('#waiting_btn_' + orderId).removeAttr('class', 'btn btn-warning');
                    $('#on_process_btn_' + orderId).attr('class', 'btn btn-info');
                    $('#waiting_btn_'+orderId).attr('class', 'btn btn-light-warning');
                }
                else {
                    $('#on_process_btn_'+orderId).removeAttr('class', 'btn btn-info');
                    $('#waiting_btn_'+orderId).removeAttr('class', 'btn btn-warning');
                    $('#comlpleted_btn_'+orderId).attr('class', 'btn btn-success');
                    $('#on_process_btn_'+orderId).attr('class', 'btn btn-light-info');
                    $('#waiting_btn_'+orderId).attr('class', 'btn btn-light-warning');
                }
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
                    title: 'Chuyển trạng thái đơn hàng ' + orderId + ' sang ' + '"' + statusName + '"'+ ' thành công'
                })
                UpdateModal.modal('hide');
                return;
            } else {
                UpdateModal.modal('hide');
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
                    title: 'Đơn hàng đã hoàn thành không thể thay đổi trạng thái'
                })
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
                title: 'Chuyển trạng thái thất bại'
            })
        }
    });
});
//3. Hủy đơn hàng
var cancleOpen = function (id) {
    cancleModal.find('#cancle__id').text(id);
    cancleModal.modal('show');
    orderId = id;
}
$('#cancle__submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Orders/CancleOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: orderId }),
        dataType: "json",
        success: function (result) {
            if (result == "success") {
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
                    title: 'Hủy đơn hàng thành công'
                })
                cancleModal.modal('hide');
                let countTrash = $('#count-trash').text();
                let newCount = Number(countTrash)+1;
                $('#count-trash').text(newCount);
                $("#item_" + orderId).remove();
                return;
            }
            else {
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
                    title: 'Đơn hàng đã hoàn thành không thể hủy'
                })
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
                title: '!Lỗi'
            })
        }
    });
});

