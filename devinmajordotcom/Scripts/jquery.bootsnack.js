/*
bootsnack.js
Author: Erika Shewan
Description:
         Simple jquery plugin to create a snackbar style notification using bootstrap's built in notification class.
Options:
         Alert Type: specifies the type of alert (info, success, warning, error.) Required.
         Message: Custom string. Can use html. default is null. Required.
         Title: Title of alert. Defaults to Success/Warning/Error/Info based on alert type. Not Required.
*/

(function ($) {

    $.fn.bootsnack = function (options) {
        try {
            var settings = $.extend({
                alertType: 'info',
                message: null,
                title: null
            }, options);


            if (settings.alertType == 'success') {
                var notificationClass = "success";
                var icon = "fa-check-circle";
                var title = "Success";
            }

            else if (settings.alertType == 'warning') {
                var notificationClass = "warning";
                var icon = "fa-exclamation-circle";
                var title = "Warning";
            }

            else if (settings.alertType == 'error') {
                var notificationClass = "danger";
                var icon = "fa-times-circle";
                var title = "Error";
            }

            else if (settings.alertType == 'info') {
                var notificationClass = "info";
                var icon = "fa-info-circle";
                var title = "Info";
            }

            else {
                var notificationClass = "info";
                var icon = "fa-info-circle";
                var title = "Info";
                console.warn(settings.alertType + " is not a valid alertType. Defaulting to info alert.")
            }

            if (settings.title != null) {
                var title = settings.title;
            }


            return this.append('<div class="alert-dismissible alert alert-' + notificationClass + ' bootsnack show" id="ajaxAlert" role="alert"> \
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"> \
                              <span aria-hidden="true">&times;</span> \
                            </button> \
                            <span class="fa '+ icon + '"></span> \
                            <strong>'+ title + '</strong> \
                            <br>'
                + settings.message +
                '</div>');
        }
        catch (ex) {
            console.error("Bootsnack.js error: " + ex);
        }
        finally {
            setTimeout(function () {
                $("#ajaxAlert").alert('close');
            }, 9000);

        }

    }

}(jQuery));