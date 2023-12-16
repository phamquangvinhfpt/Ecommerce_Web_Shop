//3. Xóa
let disableModal = $('#disable-modal');
let activeModal = $('#active-modal');
$('.dimis-modal').click(function () {
    disableModal.modal('hide');
    activeModal.modal('hide');
})
let accountID;
var changerole = function (id) {
    let role = $('#role-id-'+id).val();
    let accoundID = id;
    $.ajax({
        type: "POST",
        url: '/Auth/ChangeRoles',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ accountID: accoundID, roleID: role }),
        dataType: "json",
        success: function (result) {
            if (result == false) {
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
                    title: 'Quản trị viên mới có quyền chỉnh sửa'
                })
                return false;
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
                    title: 'Chuyển quyền thành công'
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
                title: 'Lỗi'
            })
        }
    });
}
//2. Vô hiệu hóa
var disableOpen = function (id,email) {
    disableModal.find('#disable__name').text(email);
    disableModal.modal('show');
    accountID = id;
}

$('#disable__submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Auth/Disable',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: accountID }),
        dataType: "json",
        success: function (result) {
            if (result == "success") {
                disableModal.modal('hide');
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
                    title: 'Vô hiệu hóa tài khoản thành công'
                })
                let countTrash = $('#count-trash').text();
                let newCount = Number(countTrash) + 1;
                $('#count-trash').text(newCount);
                $("#item_" + accountID).remove();
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
                title: 'Lỗi'
            })
        }
    });
});

//3. Khôi phục tài khoản
var isActiveOpen = function (id, email) {
    activeModal.find('#active__name').text(email);
    activeModal.modal('show');
    accountID = id;
}

$('#active__submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Auth/IsActive',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: accountID }),
        dataType: "json",
        success: function (result) {
            if (result == "success") {
                disableModal.modal('hide');
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
                    title: 'Khôi phục tài khoản thành công'
                })
                $("#item_" + accountID).remove();
                activeModal.modal('hide');
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
                title: 'Lỗi'
            })
        }
    });
});