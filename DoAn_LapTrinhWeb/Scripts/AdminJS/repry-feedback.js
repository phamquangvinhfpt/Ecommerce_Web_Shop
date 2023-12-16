var replyModal = $('#reply-modal');
$('.dimis-modal').click(function () {
    replyModal.modal('hide');
})
//2. Phản hồi bình luận/feedback
var CreateReplyFeedback = function (id, product_name, content, acc_name) {
    replyModal.modal('show');
    $('#feedback-product').text(product_name);
    $('#feedback-content').html(content);
    $('#reply_comment_con').text('<p style="font-weight:500;">@'+acc_name+'</p>');
    var $preview, editor, mobileToolbar, toolbar;
    Simditor.locale = 'en-US';
    toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
    mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
    if (mobilecheck()) {
        toolbar = mobileToolbar;
    }
    editor = new Simditor({
        textarea: $('#reply_comment_con'),
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
    //gửi reply bình luận
    $('#submit_reply_comm').click(function () {
        var _reply_content = $('#reply_comment_con').val();
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
                url: '/Feedbacks/ReplyComment',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: _id, reply_content: _reply_content }),
                dataType: "json",
                success: function (result) {
                    if (result == true) {
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
                            icon: 'success',
                            title: 'Phản hồi thành công'
                        })
                        replyModal.modal('hide');
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

