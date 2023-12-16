//phần chọn sao đánh giá
//di chuột ra khỏi đối tượng
$(document).ready(function () {
    CRateSelected(5);
    var $preview, editor, mobileToolbar, toolbar;
    Simditor.locale = 'en-US';
    toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
    mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
    if (mobilecheck()) {
        toolbar = mobileToolbar;
    }
    editor = new Simditor({
        textarea: $('#comment__con'),
        toolbar: toolbar,
        pasteImage: true,
        defaultImage: '/Content/Images/favicon.png',
        upload: location.search === '?upload' ? {
            url: '/upload'
        } : false
    });
    $preview = $('#preview');
    if ($preview.length > 0) {
        return editor.on('valuechanged', function (e) {
            return $preview.html(editor.getValue());
        });
    }
});
function CRateOut(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star-o');
    }
}
//di chuột vào một đối tượng
function CRateOver(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
    }
}
//click vào đối tượng
function CRateClick(rating) {
    $('.uploadimgcheck').removeAttr('disabled');
    $('.write_content_fb').removeClass('cursor-disable');
    $('#ConfirmAdd').removeAttr('disabled');
    $('#FBk_Content').removeAttr('disabled');
    $("#dcript_content_fb").css("color", "#666");
    $("#dcript_content_fb").text("Nhập nội dung đánh giá");
    $("#lblRating").val(rating);
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');

    }
    for (var i = 1; i <= 5; i++) {
        $("#rate" + i).attr('class', 'fa fa-star-o');
    }
}
//di chuột ra khỏi đối tượng
function CRateSelected() {
    var rating = $("#lblRating").val();
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
    }
}

//1. Thêm bình luận/Đánh giá
$('#create_submit_comment').click(function () {
    const com_content = $("#comment__con").val();
    const proID = $("#product_id").val();
    const discID = $("#discount_id").val();
    const genreID = $("#genre_id").val();
    const rateStar = $("#lblRating").val();
    if (com_content == "") {
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
            title: 'Nhập nội dung bình luận'
        })
        return false;
    }
    else if (com_content.length < 20) {
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
            title: 'Nội dung bình luận tối thiểu 20 ký tự'
        })
        return false;
    }
    else if (com_content.length > 500) {
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
            title: 'Nội dung bình luận không quá 500 ký tự'
        })
        return false;
    }
    else {
        $.ajax({
            type: "Post",
            url: "/Products/ProductComment",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ productID: proID, discountID: discID, genreID: genreID, rateStar: rateStar, commentContent: com_content }),
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    setTimeout(function () {
                        window.location.reload();
                    }, 1);
                    //$('.comment-wrapper').append('<input class="form-control" type="text"/>');
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
                    title: 'Chức năng yêu cầu đăng nhập?'
                })
            }
        })
    };
});

//2. Phản hồi bình luận/feedback
var CreateReplyFeedback = function (id, acc_name) {
    $('#reply_comment_con_' + id).text('<p style="font-weight:500;">Chào ' + acc_name + ',</p>');
    var $preview, editor, mobileToolbar, toolbar;
    Simditor.locale = 'en-US';
    toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
    mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
    if (mobilecheck()) {
        toolbar = mobileToolbar;
    }
    editor = new Simditor({
        textarea: $('#reply_comment_con_' + id),
        toolbar: toolbar,
        pasteImage: true,
        defaultImage: '/Content/Images/favicon.png',
        upload: location.search === '?upload' ? {
            url: '/upload'
        } : false
    });
    $preview = $('#preview');
    if ($preview.length > 0) {
        return editor.on('valuechanged', function (e) {
            return $preview.html(editor.getValue());
        });
    }
    let _id = id;
    $('#submit_reply_comm_' + _id).removeAttr('hidden');
    //gửi reply bình luận
    $('#submit_reply_comm_' + _id).click(function () {
        var _reply_content = $('#reply_comment_con_' + _id).val();
        if (_reply_content == "") {
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
                title: 'Vui lòng nhập nội dung bình luận!'
            })
        }
        else if (_reply_content.length < 20) {
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
                title: 'Nội dung tối thiểu 20 ký tự'
            })
            return false;
        }
        else if (_reply_content.length > 500) {
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
                title: 'Nội dung bình luận không quá 500 ký tự'
            })
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: '/Products/ReplyComment',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: _id, reply_content: _reply_content }),
                dataType: "json",
                success: function (result) {
                    if (result == true) {
                        setTimeout(function () {
                            window.location.reload();
                        }, 1);
                        return;
                    }
                    else {
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
                            title: 'Đã có lỗi xảy ra'
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
                        icon: 'error',
                        title: '!Lỗi'
                    })
                }
            });
        }
    });
}

//3.phản hồi bình luận con
var createChildReply = function (id, reply_id, acc_name) {
    child_ide = id;
    child_rep = reply_id
    $('#childRepContent_' + child_rep).text('<p style="font-weight:500;">@' + acc_name + '</p>');
    $('#submit_child_reply_comm_' + child_rep).removeAttr('hidden');
    var $preview, editor, mobileToolbar, toolbar;
    Simditor.locale = 'en-US';
    toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
    mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
    if (mobilecheck()) {
        toolbar = mobileToolbar;
    }
    editor = new Simditor({
        textarea: $('#childRepContent_'+child_rep),
        toolbar: toolbar,
        pasteImage: true,
        defaultImage: '/Content/Images/favicon.png',
        upload: location.search === '?upload' ? {
            url: '/upload'
        } : false
    });
    $preview = $('#preview');
    if ($preview.length > 0) {
        return editor.on('valuechanged', function (e) {
            return $preview.html(editor.getValue());
        });
    }

    $('#submit_child_reply_comm_'+child_rep).click(function () {
        var _child_reply_content = $('#childRepContent_'+child_rep ).val();
        if (_child_reply_content == "") {
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
                title: 'Vui lòng nhập nội dung bình luận!'
            })
        }
        else if (_child_reply_content.length < 20) {
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
                title: 'Nội dung bình luận tối thiểu 20 ký tự'
            })
            return false;
        }
        else if (_child_reply_content.length > 500) {
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
                title: 'Nội dung bình luận không quá 500 ký tự'
            })
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: '/Products/ReplyComment',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: child_ide, reply_content: _child_reply_content }),
                dataType: "json",
                success: function (result) {
                    if (result == true) {
                        setTimeout(function () {
                            window.location.reload();
                        }, 1);
                    }
                    else {
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
                        icon: 'danger',
                        title: 'Lỗi'
                    })
                }
            });
        }
    });
}
