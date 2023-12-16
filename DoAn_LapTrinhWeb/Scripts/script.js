function ShowImagePreview(imageUpload, previewImage){
    if (imageUpload.file && imageUpload.file[0]){
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src',e.target.result);
        }
        reader.readAsDataURL(imageUpload.file[0]);
    }
}