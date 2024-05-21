ClassicEditor
    .create(document.querySelector('#ckDetail'), {
        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
    })
    .then(editor => {
        window.editor = editor;
    })
    .catch(err => {
        console.error(err.stack);
    });

var loadFile = function (input) {
    var image = document.getElementById('imagePreview');
    image.src = URL.createObjectURL(input.files[0]);
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('output');
        output.src = reader.result;
    };
    reader.readAsDataURL(input.files[0]);

     $( function() {
        $( "#Date" ).datepicker();
    } );
};
