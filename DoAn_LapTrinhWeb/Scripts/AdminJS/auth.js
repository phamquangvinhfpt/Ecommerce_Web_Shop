//3. Xóa
let disableModal = $('#disable-modal');
let activeModal = $('#active-modal');
//list-role
let listRole = $('#list-role');
let roles = $('#role-list');
function AddNewRole() {
    Swal.fire({
        title: "Bạn muốn thêm quyền mới?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Thêm",
        cancelButtonText: "Hủy",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
    }).then((result) => {
        //if user click confirm
        if (result.isConfirmed) {
            //get new role name
            let newRoleName = $('#new-role-name').val();
            //if new role name is empty
            if (newRoleName == "") {
                Swal.fire({
                    title: "Tên quyền không được để trống",
                    icon: "error",
                    confirmButtonText: "Đóng",
                    confirmButtonColor: "#d33",
                })
                return;
            }
            //if new role name is not empty
            $.ajax({
                type: "POST",
                url: '/Admin/Auth/AddNewRole',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ roleName: newRoleName }),
                dataType: "json",
                success: function (result) {
                    if (result == false) {
                        Swal.fire({
                            title: "Lỗi",
                            icon: "error",
                            confirmButtonText: "Đóng",
                            confirmButtonColor: "#d33",
                        })
                        return false;
                    }
                    else {
                        Swal.fire({
                            title: "Thêm thành công",
                            icon: "success",
                            confirmButtonText: "Đóng",
                            confirmButtonColor: "#d33"
                        })
                        //reload datatable
                        table.ajax.reload();
                        window.location.reload();
                        return true;
                    }
                }
            })
        }
    })
}

//Lấy danh sách quyền từ server về và đổ vào datatable list-role
let table = new DataTable('#list-role', {
    ajax: {
        url: '/Admin/Auth/GetAllRole',
        dataSrc: ''
    },
    columns: [
        { data: 'role_id' },
        {
            data: 'role_name',
            render: function (data, type, row) {
                //if role_id = 1, set disabled
                if (row.role_id == 1) {
                    return `<input type="text" class="form-control" id="role-name-${row.role_id}" value="${data}" disabled>`;
                }
                if (row.role_id == 2) {
                    return `<input type="text" class="form-control" id="role-name-${row.role_id}" value="${data}" disabled>`;
                }
                if (row.role_id == 3) {
                    return `<input type="text" class="form-control" id="role-name-${row.role_id}" value="${data}" disabled>`;
                }
                return `<input type="text" class="form-control" id="role-name-${row.role_id}" value="${data}">`;
            }
        },
        {
            data: 'role_id',
            render: function (data, type, row) {
                //return edit and delete button
                return `<button class="btn btn-primary" onclick="editRole(${data})"><i class="fas fa-edit"></i></button>
                        <button class="btn btn-danger" onclick="deleteRole(${data})"><i class="fas fa-trash-alt"></i></button>`;
            }
        }
    ],
    columnDefs: [
        { className: 'text-center', targets: [0, 1, 2] }
]
});

var deleteRole = function (id) {
    //if role_id = 1, 2, 3 alert can not delete
    if (id == 1 || id == 2 || id == 3) {
        Swal.fire({
            title: "Không thể xóa quyền này",
            icon: "error",
            confirmButtonText: "Đóng",
            confirmButtonColor: "#d33",
        })
        return;
    }
//if role_id != 1, 2, 3
    Swal.fire({
        title: "Bạn muốn xóa quyền này?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Xóa",
        cancelButtonText: "Hủy",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
    }).then((result) => {
//if user click confirm
    if (result.isConfirmed) {
        $.ajax({
            type: "POST",
            url: '/Admin/Auth/DeleteRole',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ roleId: id }),
            dataType: "json",
            success: function (result) {
                if (result == false) {
                    Swal.fire({
                        title: "Lỗi",
                        icon: "error",
                        confirmButtonText: "Đóng",
                        confirmButtonColor: "#d33",
                    })
                    return false;
                }
                else {
                    Swal.fire({
                        title: "Xóa thành công",
                        icon: "success",
                        confirmButtonText: "Đóng",
                        confirmButtonColor: "#d33"
                    })
                    //reload datatable
                    table.ajax.reload();
                    return true;
                }
            }
        })
        }
    })
}

var editRole = function (id) {
    //get new role name
    let newRoleName = $('#role-name-' + id).val();
    //get old role name
    let oldRoleName = $('#role-name-' + id).attr('value');
    //if new role name is empty
    if (newRoleName == "") {
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
            title: 'Tên quyền không được để trống'
        })
        return;
    }
    // if new role name is not change
    if (newRoleName == oldRoleName) {
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
            title: 'Tên quyền không được trùng'
        })
        return;
    }

    //if new role name is change
    $.ajax({
        type: "POST",
        url: '/Admin/Auth/EditRole',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ roleID: id, roleName: newRoleName }),
        dataType: "json",
        success: function (result) {
            if (result == false) {
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
                return false;
            }
            else {
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
                    title: 'Sửa thành công'
                })
                window.location.reload();
                return;
            }
        }
    });
}

$('.dimis-modal').click(function () {
    disableModal.modal('hide');
    activeModal.modal('hide');
})
let accountID;
var changerole = function (id) {
    let role = $('#role-id-' + id).val() - 1;
    console.log(role);
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