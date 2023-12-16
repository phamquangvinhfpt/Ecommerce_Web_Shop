////(function() {
////  $(function() {
////    var $preview, editor, mobileToolbar, toolbar;
////    Simditor.locale = 'en-US';
////    toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', 'table', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
////    mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
////    if (mobilecheck()) {
////      toolbar = mobileToolbar;
////    }
////    editor = new Simditor({
////      textarea: $('#txt-content'),
////      toolbar: toolbar,
////      pasteImage: true,
////      defaultImage: '/Content/Images/favicon.png',
////      upload: location.search === '?upload' ? {
////        url: '/upload'
////      } : false
////    });
////    $preview = $('#preview');
////    if ($preview.length > 0) {
////      return editor.on('valuechanged', function(e) {
////        return $preview.html(editor.getValue());
////      });
////    }
////  });

////}).call(this);
