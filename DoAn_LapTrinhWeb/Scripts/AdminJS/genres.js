////1. Thêm mới 
////2. Chỉnh sửa
////3. Xóa
////------------------------------------------------/
const createModal = $('#create-modal')
const editModal = $('#edit-modal')
const deleteModal = $('#delete-modal');
let genreID;
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
    const name = $('#create__input').val()
    if (name == "") {
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
            title: 'Nhập tên loại'
        })
        return;
    }
    return $.ajax({
        type: "POST",
        url: '/Genres/Create',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ genreName: name }),
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
                title: 'Thêm không thành công'
            })
        }
    });
});

//2. Chỉnh sửa
var editOpen = function (id, name) {
    editModal.find('#edit__input').val(name);
    editModal.modal('show');
    genreID = id;
}
$('#edit__save').click(function () {
    const name = $('#edit__input').val()
    if (name == "") {
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
            title: 'Nhập tên loại'
        })
        return;
    }
    return $.ajax({
        type: "POST",
        url: '/Genres/Edit',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: genreID, genreName: name }),
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
                setTimeout(function () {
                    window.location.reload();
                }, 500);
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
});
//3. Xóa 
var deleteOpen = function (id, name) {
    deleteModal.find('#delete__name').text(name);
    deleteModal.modal('show');
    genreID = id;
}
$('#delete__submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/Genres/Delete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: genreID }),
        dataType: "json",
        success: function (result) {
            if (result == "delete") {
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
                    title: 'Xóa thành công'
                })
                deleteModal.modal('hide');
                $("#item_" + genreID).remove();
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
