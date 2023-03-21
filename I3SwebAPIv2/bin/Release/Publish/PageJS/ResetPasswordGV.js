$(document).ready(function () {

    var user_id = getUrlParameter('uid');
    var email = getUrlParameter('email');
    var token = getUrlParameter('token');

    function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    }

    $('#btnReset').on('click', function () {
        var Pswd = $('#password').val().trim();
        var confirmPswd = $('#confirm_password').val().trim();

        if (Pswd === confirmPswd) {
            ResetPassword();
        } else {
            showNotification('bg-red', 'Password not match.', 'bottom', 'right');
            $('#password').val('');
            $('#confirm_password').val('');
        }
    });

    function ResetPassword() {
        var body = {
            user_id: user_id,
            email: email,
            token: token,
            password: $('#password').val().trim(),
            confirm_password: $('#confirm_password').val().trim()
        };
        $.ajax(
            {
                url: 'http://giis.myedutech.my/api/v2/user/reset-password',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: body,
                success: function (resp) {
                    if (resp.Success === true) {
                        showNotification('bg-green', resp.Message, 'bottom', 'right');
                    }
                    else {
                        showNotification('bg-red', resp.Message, 'bottom', 'right');
                    }
                },
                error: function (xhr) {
                    showNotification('bg-red', xhr.statusText, 'bottom', 'right');
                }
            });
        return false;
    }

    function showNotification(colorName, text, placementFrom, placementAlign, animateEnter, animateExit) {
        if (colorName === null || colorName === '') { colorName = 'bg-black'; }
        if (text === null || text === '') { text = 'Turning standard Bootstrap alerts'; }
        if (animateEnter === null || animateEnter === '') { animateEnter = 'animated fadeInDown'; }
        if (animateExit === null || animateExit === '') { animateExit = 'animated fadeOutUp'; }
        var allowDismiss = true;

        $.notify({
            message: text
        },
            {
                type: colorName,
                allow_dismiss: allowDismiss,
                newest_on_top: true,
                timer: 5000,
                placement: {
                    from: placementFrom,
                    align: placementAlign
                },
                animate: {
                    enter: animateEnter,
                    exit: animateExit
                },
                template: '<div data-notify="container" class="bootstrap-notify-container alert alert-dismissible {0} ' + (allowDismiss ? "p-r-35" : "") + '" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
            });
    }

});