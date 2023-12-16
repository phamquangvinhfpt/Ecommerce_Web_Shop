//1. Notification
$(window).on('load', function () {
    const msgType1_5s = $('input[name=msgType1_5s]').val();
    const msg1_5s = $('input[name=msg1_5s]').val();

    const msgType3s = $('input[name=msgType3s]').val();
    const msg3s = $('input[name=msg3s]').val();

    const msgType5s = $('input[name=msgType5s]').val();
    const msg5s = $('input[name=msg5s]').val();
    //notifi 1.5s
    if (msgType1_5s != '' && msg1_5s != '') {
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
        switch (msgType1_5s) {
            case 'success':
                Toast.fire({
                    icon: 'success',
                    title: msg1_5s
                })
                break;
            case 'info':
                Toast.fire({
                    icon: 'info',
                    title: msg1_5s
                })
                break;
            case 'warning':
                Toast.fire({
                    icon: 'warning',
                    title: msg1_5s
                })
                break;
            case 'error':
                Toast.fire({
                    icon: 'error',
                    title: msg1_5s
                })
                break;
        }
        return;
    }
    //notifi 3s
    if (msgType3s != '' && msg3s != '') {
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
        switch (msgType3s) {
            case 'success':
                Toast.fire({
                    icon: 'success',
                    title: msg3s
                })
                break;
            case 'info':
                Toast.fire({
                    icon: 'info',
                    title: msg3s
                })
                break;
            case 'warning':
                Toast.fire({
                    icon: 'warning',
                    title: msg3s
                })
                break;
            case 'error':
                Toast.fire({
                    icon: 'error',
                    title: msg3s
                })
                break;
        }
        return;
    }
    //notifi 5s
    if (msgType5s != '' && msg5s != '') {
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
        switch (msgType5s) {
            case 'success':
                Toast.fire({
                    icon: 'success',
                    title: msg5s
                })
                break;
            case 'info':
                Toast.fire({
                    icon: 'info',
                    title: msg5s
                })
                break;
            case 'warning':
                Toast.fire({
                    icon: 'warning',
                    title: msg5s
                })
                break;
            case 'error':
                Toast.fire({
                    icon: 'error',
                    title: msg5s
                })
                break;
        }
        return;
    }
});