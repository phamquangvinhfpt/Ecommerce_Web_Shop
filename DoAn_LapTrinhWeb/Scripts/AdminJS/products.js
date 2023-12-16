//1. Vô hiệu hóa sản phẩm
//2. Khôi phục sản phẩm
//3. Xóa sản phẩm
//4. Preview Ảnh
//------------------------------------------------/
const deleteOrDisablemodal = $('#delete-disable');
const activateModal = $('#activate');
let id;
var deleteOrDisableConfirm = function (productId,ProductName, productPrice, ProductOldPrice, productImg) {
    deleteOrDisablemodal.find('#product__id').text(ProductName);
    deleteOrDisablemodal.find('#product__old-price').text(ProductOldPrice);
    deleteOrDisablemodal.find('#product__price').text(productPrice);
    deleteOrDisablemodal.find('#product__img').attr('src', productImg);
    deleteOrDisablemodal.modal('show');
    id = productId;
}
$('.dimis-modal').click(function () {
     deleteOrDisablemodal.modal('hide');
     activateModal.modal('hide');
});
//1. Vô hiệu hóa sản phẩm
$('#disable-product').click(function () {
    deleteOrDisablemodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/ProductsAdmin/Disable',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        success: function (result) {
            if (result == "disabled") {
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
                    title: 'Vô hiệu hóa thành công'
                })
                let countTrash = $('#count-trash').text();
                let newCount = Number(countTrash) + 1;
                $('#count-trash').text(newCount);
                $("#item_" + id).remove();
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
                title: 'Vô hiệu hóa không thành công'
            })
        }
    });
});
//2. Khôi phục sản phẩm
var activateProduct = function (productId, ProductName, productPrice2, ProductOldPrice2, productImg) {
    activateModal.find('#product__id').text(ProductName);
    activateModal.find('#product__old-price-trash').text(ProductOldPrice2);
    activateModal.find('#product__price-trash').text(productPrice2);
    $('#product__img').attr('src', productImg);
    activateModal.modal('show');
    id = productId;
}

$('#avtivate__product-submit').click(function () {
    $.ajax({
        type: "POST",
        url: '/ProductsAdmin/Undo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        dataType: "json",
        success: function (result) {
            if (result == "activate") {
                activateModal.modal('hide');
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
                    title: 'Khôi phục thành công'
                })
                let countTrash = $('#count-trash').text();
                let newCount = Number(countTrash)-1;
                $('#count-trash').text(newCount);
                $("#item_" + id).remove();
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
                title: 'Khôi phục không thành công'
            })
        }
    });
});
//$('.closeet').click(function (event) {
//  var item= $(event.target).closest(".item");
//    item.remove();
//});
//3. Xóa sản phẩm
$('#delete-product').click(function (event) {
    deleteOrDisablemodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/ProductsAdmin/Delete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
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
                $("#item_"+id).remove();
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
                title: 'Xóa không thành công'
            })
        }
    });
});
//4. Preview ảnh
var counter = 1;
$("#btn2").click(function () {
    counter++;
    if (counter < 10) //only allows 9 additional transactions
    {
        $(".append-image").append("<div class='form-input col-lg-4'><label for='file-ip-" + counter + "'>Chọn ảnh</label><img height='100px' />" +
            "<input type='file' id='file-ip-" + counter + "' name='ImageUploadMulti' accept='.png,.jpg,jpeg' onchange='showPreview(this)'></div>");
    }
    if (counter >= 11) {
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
            title: 'Số ảnh được thêm tối đa 9 ảnh'
        })
    }
});
function showPreview(input) {
    var preview = input.previousElementSibling;
    var file = input.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        preview.src = reader.result;
        preview.style.display = "block";
    }
    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.src = "";
    }
}
