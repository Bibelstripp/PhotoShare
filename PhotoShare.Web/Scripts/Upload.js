$(function () {

    Dropzone.options.uploadForm = {

        // The configuration we've talked about above
        autoProcessQueue: false,
        uploadMultiple: true,
        maxFiles: 1,

        // The setting up of the dropzone
        init: function () {
            var myDropzone = this;

            $('input[type=submit]', this.element).on("click", function (e) {
                // Make sure that the form isn't actually being sent.
                e.preventDefault();
                e.stopPropagation();
                myDropzone.processQueue();
            });

                //if you drop more files it will remove the first and add the last one
                this.on("maxfilesexceeded", function (file) {
                this.removeAllFiles();
                this.addFile(file);
            });

                myDropzone.on("success", function (file, response) {
                    window.location = "/Photo/View/" + response;
            });
        }
    }
});