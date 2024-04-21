//var JSSetting = JSSetting || {};
var JSSetting = function () {
    //------------------------------------------ global functions :
    var isElementExsit = function ($element) {
        if ($element.length) {
            return true;
        } else {
            return false;
        }
    }
    var loading = function () {
        //var loadingInner = '\n <div class="loader-overlay-trans">\n     <div class="loader-content">\n <h2>جارِ المعالجة ..</h2>\n       <div class="loader-index">\n          <div></div>\n          <div></div>\n          <div></div>\n          <div></div>\n          <div></div>\n          <div></div>\n        </div>\n      </div>\n</div>';
        // $(".custom-loader").find(".loader-overlay").remove();
        //$(loadingInner).appendTo(".custom-loader")
        $(".loadingPage").show();



        //var over_load = "<div class='loading'>Loading&#8230;</div>";
        //$(".overlay-content").find(".loading").remove();
        //$(over_load).appendTo(".overlay-content")

    };
    var stopLoading = function () {
        //$(".custom-loader").find(".loader-overlay-trans").remove();
        $(".loadingPage").hide();


        //$(".overlay-content").find(".loading").remove();

    };
    var Alert_msg = function (type, msg) {
        var msg_type = "info";
        if (type == 1)
            msg_type = "info";
        else if (type == 2)
            msg_type = "warning";
        else if (type == 3)
            msg_type = "danger";
        try {
            $.notify({
                message: msg
            }, {
                // settings
                type: msg_type,
                timer: 1000,
                delay: 10000,
                clickToHide: true,
                showProgressbar: false,
                mouse_over: 'pause',
                z_index: 99999,
            });
        } catch (e) {
            console.log(e);
            if (type == 3)
                alert(msg);
        }
    }
    var fun_redirctURL = function (url) {
        ////debugger;
        if (url == "" || url == null) { }
        else {
            loading();
            window.location.replace(url);
        }
    }
    var fun_updateFormValidation = function (fv, ERROR_COLUMN) {
        ////debugger;
        if (ERROR_COLUMN != null)
            for (var i = 0; i < ERROR_COLUMN.length; i++) {
                ////debugger;
                var error = ERROR_COLUMN[i];
                fv
                    .updateMessage(error.NME_COLMN, 'blank', error.TXT_MSG)
                    .updateStatus(error.NME_COLMN, 'INVALID', 'blank');
            }
    }
    var updateCaptcha = function (response) {
        try {
            $('#CaptchaImage1').attr('src', response.CPATCHA.CaptchaImage);
            $('#CaptchaDeText1').val(response.CPATCHA.CaptchaDeText);
        } catch (e) {

        }
        $('#CaptchaInputText1').val('');
    }
    return {
        loading: loading,
        stopLoading: stopLoading,
        Alert_msg: Alert_msg,
        fun_redirctURL: fun_redirctURL,
        fun_updateFormValidation: fun_updateFormValidation,
        updateCaptcha: updateCaptcha,
        isElementExsit: isElementExsit
    };
}();
$(function () {
    //------------------------------------------- handling AJAX :
    $(document).ajaxError(function (event, jqXHR, settings, thrownError) {
        console.log(jqXHR.responseText);
        var url = settings.url
        var msg = '';
        if (jqXHR.status === 0) {
            msg = 'عفوا لايمكن الدخول للموقع.\n هناك مشكلة بالانترنت.';
        } else if (jqXHR.status == 404) {
            msg = 'عفوا الصفحة المطلوبة غير متوفرة [400]. ';
        } else if (jqXHR.status == 500) {
            msg = 'عفوا هناك خطأ في الخادم [500].';
        } else if (exception === 'parsererror') {
            msg = 'عفوا لايمكن اتمام الاجراء ، الرجاء تحديث الصفحة.';
        } else if (exception === 'timeout') {
            msg = 'عفوا هناك ضغط على الخادم الرجاء المحاولة مرة اخرة.';
        } else if (exception === 'abort') {
            msg = 'عفوا لايمكن اتمام الاجراء ، الرجاء تحديث الصفحة.';
        } else {
            msg = 'عفوا حصل خطأ غي متوقع.\n' + jqXHR.responseText;
        }
        JSSetting.stopLoading();
        JSSetting.Alert_msg(3, msg);
    })
    $(document).ajaxComplete(function () {
        JSSetting.stopLoading();
    });
    $(document).ajaxStart(function () {
        // debugger;
        JSSetting.loading();
    });
    $(document).ajaxSuccess(function (event, xhr, settings) {
        var response = xhr.responseJSON;
        if (response == "" || response == undefined) {
            //   JSSetting.Alert_msg(3, "عفوا حدث خطأ الرجاء تحديث الصفحة")
        }
        else if (response.CDE_MSG == "-3") {
            JSSetting.Alert_msg(3, response.TXT_MSG)

            var count = 3;
            var timer = $.timer(
                function () {
                    count--;
                    if (count == 0) {
                        JSSetting.fun_redirctURL(response.redirct_url);
                    }
                },
                1000,
                true
            );
        }
        else if (response.CDE_MSG == "-2") {
            JSSetting.Alert_msg(3, "هناك خطأ غير متوقع")
        }
    });
    window.onerror = function () {
        JSSetting.Alert_msg(1, "حصل خطأ الرجاء تحديث الصفحة ، وعند تكرار ظهور الرسالة نرجو التواصل مع الدعم الفني");
    };

    try {
        $('[data-webui]').webuiPopover();
    } catch (e) {
        console.log("PLEASE ADD 'webuiPopover' REFRENCE");
    }
    $(".number_input").keypress(function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;

        if (evt.which == 13) {
            return true;
        }
        if (charCode == 8 || (evt.which == 0)) // BackSpace
        {
            return true;
        }
        if ((charCode > 47 && charCode < 58) || charCode == 9)
            return true;
        try {
            //      JSSetting.Alert_msg(3, 'يجب إدخال أرقام فقط');
        }
        catch (err) {
            //     JSSetting.Alert_msg(3, 'يجب إدخال أرقام فقط');
        }
        return false;
    });
    $(".arabic_input").keypress(function (evt) {
        evt = (evt) ? evt : window.event;
        //var enter_code = evt.keyCode || evt.which;
        var charCode = (evt.which) ? evt.which : evt.keyCode;

        if (charCode == 13 || charCode == 8 || (evt.which == 0)) // BackSpace
        {
            return true;
        }
        if ((charCode > 1568 && charCode < 1595) || (charCode > 1600 && charCode < 1611) || (charCode == 32) || (charCode == 9) || (charCode == 44) || (charCode == 46))
            return true;
        try {
            JSSetting.Alert_msg(3, 'يجب إدخال أحرف عربية فقط');

        }
        catch (err) {
            JSSetting.Alert_msg(3, 'يجب إدخال أحرف عربية فقط');
        }

        return false;
    });
});

window.onpaint = function () {
    JSSetting.loading();
}
window.onbeforeunload = function (e) {

    var hh = $(e.target.activeElement).attr("id")
    var href = $(e.target.activeElement).attr("href")
    //if (typeof hh === "undefined")//typeof(undefined))
    //{
    //    JSSetting.loading();

    //}

    if (typeof href === "undefined")//typeof(undefined))
    {
        //  JSSetting.loading();

    }
    else if (href.toLowerCase().indexOf("__dopostback") > -1) {
    }
    else {
        //JSSetting.loading();

    }
    //var hh = $(e.target.activeElement).attr("id")
    //if (typeof hh === "undefined")//typeof(undefined))
    //{
    //    JSSetting.loading();

    //}
    //else if (hh.toLowerCase().indexOf("print") > -1) {
    //}
    //else {
    //    JSSetting.loading();

    //}
    //debugger;
    //var elemntTagName = e.target.tagName;
    //if (elemntTagName == 'A') {
    //   var att =  e.target.getAttribute("href");
    //   if (att.substring(0, 3) == "tel") {
    //    }
    //    else {
    //       JSSetting.loading();
    //    }
    //}
    //  JSSetting.loading();
}
function isNumber(evt) {
    debugger;

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (evt.which == 13) {
        return false;
    }
    if (charCode == 8 || (evt.which == 0)) // BackSpace
    {
        return true;
    }
    if ((charCode > 47 && charCode < 58) || charCode == 9)
        return true;
    try {
        Alert_msg(3, 'يجب إدخال أرقام فقط');
    }
    catch (err) {
        Alert_msg(3, 'يجب إدخال أرقام فقط');
    }
    return false;
}

function isArabicLetter(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 13 || charCode == 8 || (evt.which == 0)) // BackSpace
    {
        return true;
    }
    if ((charCode > 1568 && charCode < 1595) || (charCode > 1600 && charCode < 1611) || (charCode == 32) || (charCode == 9) || (charCode == 44) || (charCode == 46))
        return true;
    try {
        Alert_msg(3, 'يجب إدخال أحرف عربية فقط');

    }
    catch (err) {
        Alert_msg(3, 'يجب إدخال أحرف عربية فقط');
    }

    return false;
}
