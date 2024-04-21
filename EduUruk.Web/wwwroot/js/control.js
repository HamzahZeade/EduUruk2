var dt;
var fileUploadUrl;
var fileUploadPath;
var fileUploadText;
var fileUploadFileName;
var fileUploadFileType;
var fileUploadFileIcon;
var lang;
var mymap;
var marker;
var myDropzone;
var uploadSettings = {};
$(document).ready(function () {
    var defaultOptions = {
        validClass: 'has-success',
        errorClass: 'has-error',
        highlight: function (element, errorClass, validClass) {
            $(element).closest('[class^= "form-group"]')
                .removeClass(validClass)
                .addClass('has-error');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).closest('[class^= "form-group"]')
                .removeClass('has-error')
                .addClass(validClass);
        }
    };
    $.validator.setDefaults(defaultOptions);
    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass,
    };

    fileUploadFileIcon = '<i class="fa fa-picture-o"></i>';


    $("#btn-profile").trigger("click");

    refreshDashboard();

    SetInputControls();
});

function refreshDashboard() {
    $(".custom-dashboard").each(function () {
        $(this).trigger("click");
    });
}

function SetInputControls() {
    $.validator.unobtrusive.parse("form");

    // Select 2
    if ($('.select2_1').length > 0) {
        $('.select2_1').select2({
            minimumResultsForSearch: 8,
            width: '100%'
        });
        //$('[data-control="select2"]').select2({
        //    dropdownParent: $('#AccessModal')
        //});
        //$('.select2').select2({
        //    dropdownParent: $('#AccessModal')
        //});
    }

    // Multi Select 

    if ($('.multi-select').length) {
        //$('.multi-select').multiselect({
        //    enableFiltering: true,
        //    enableHTML: true,
        //    numberDisplayed: 1,
        //    buttonClass: '',
        //    buttonContainer: '<div class="btn-group multiselect-input" />',
        //    nonSelectedText: lang == 'ar' ? "لم يتم الإختيار" : "None selected",
        //    nSelectedText: lang == 'ar' ? "عنصر" : "selected",
        //    allSelectedText: lang == 'ar' ? "تم إختيار الكل" : "All selected",
        //    filterPlaceholder: lang == 'ar' ? "بحث" : "Search",
        //    selectAllText: lang == 'ar' ? " إختيار الكل" : " Select all",
        //    maxHeight: 250,
        //    includeSelectAllOption: false,
        //    templates: {
        //        button: '<span class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> &nbsp;<b class="fa fa-caret-down"></b></span>',
        //        ul: '<ul class="multiselect-container dropdown-menu"></ul>',
        //        filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
        //        filterClearBtn: '<span class="input-group-btn"><a class="btn btn-default btn-white btn-grey multiselect-clear-filter" ><i class="fa fa-times-circle red2"></i></button></span>',
        //        li: '<li><a tabindex="0"><label class="" ><input type="checkbox" class="ace ace-checkbox-2" /></label></a></li>',
        //        divider: '<li class="multiselect-item divider"></li>',
        //        liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
        //    }
        //});
    }

    // Bootstrap Tooltip

    if ($('[data-toggle="tooltip"]').length > 0) {
        $('[data-toggle="tooltip"]').tooltip();
    }

    // Color Picker

    if ($('[data-rel="colorpicker"]').length) {
        $('[data-rel="colorpicker"]').colorpicker({
            useHashPrefix: false,
            align: 'left'
        });
    }

    // FileUpload

    if ($('.dropzone').length) {
        setuploadtools();
    }

    // Toggle Button

    if ($('.btn-toggle').length > 0) {
        $('.btn-toggle').click(function () {
            $(this).find('.btn').toggleClass('active');
            if ($(this).find('.btn-success').size() > 0) {
                $(this).find('.btn').toggleClass('btn-success');
            }
        });
    }

    // Datetimepicker

    if ($('.datepicker').length > 0) {
        //$('.datepicker').datepicker({
        //    autoclose: true,
        //    todayHighlight: true,
        //    format: 'yyyy/mm/dd'//'dd/mm/yyyy'
        //}).on('changeDate', function (ev) {
        //    $(this).attr("class", "datepicker has-value");
        //});
    }

    if ($('.date-islamic').length > 0) {

        var calendar = $.calendars.instance('ummalqura');
        $('.date-islamic').calendarsPicker({ calendar: calendar }).on('changeDate', function (ev) {
            $(this).attr("class", "date-islamic has-value");
        });;

    }


    // Permissions Check All

    if ($('[data-rel="check-all"]').length) {
        $('[data-rel="check-all"]').change(function () {
            var input = 'input[type="checkbox"][id*="' + $(this).data("target") + '"]';
            $(input).prop('checked', $(this).is(':checked'));
        });

    }

    // Permissions Check All

    if ($('[data-rel="check-row"]').length) {
        $('[data-rel="check-row"]').change(function () {
            var input = 'input[type="checkbox"][id*="' + $(this).data("target") + '"]';
            $(input).prop('checked', $(this).is(':checked'));
        });

    }

    // Permissions Check All

    if ($('[data-rel="scroll-table"]').length) {
        setScrollTable();
    }

    // Form Wizard

    if ($('.wizard-card').length) {
        setFormWizard();
    }

    // Left Sidebar Scroll

    if ($('.slimscroll').length > 0) {
        $('.slimscroll').slimScroll({
            height: 'auto',
            width: '100%',
            position: 'right',
            size: "7px",
            color: '#ccc',
            wheelStep: 10,
            touchScrollStep: 100
        });
        var hei = $(window).height() - 60;
        $('.slimscroll').height(hei);
        $('.sidebar .slimScrollDiv').height(hei);

        $(window).resize(function () {
            var hei = $(window).height() - 60;
            $('.slimscroll').height(hei);
            $('.sidebar .slimScrollDiv').height(hei);
        });
    }

    if ($('[data-rel="slimScroll"]').length) {
        $('[data-rel="slimScroll"]').each(function () {
            var scrol = $(this);
            scrolposition = 'right';
            var maxheight = 250;
            if (scrol.data("height")) {
                maxheight = parseInt(scrol.data("height"), 0);
            }
            if (scrol.data("align")) {
                scrolposition = scrol.data("align");
            }
            scrol.slimScroll({
                position: scrolposition,
                height: maxheight + 'px'
            });
        });
    }


    $(".numeric").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+X
            (e.keyCode == 88 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $(".decimal").keydown(function (e) {
        isNumberKey(e, $(this));
    });

    $.validator.addMethod("date",
        function (value, element, params) {
            if (this.optional(element)) {
                return true;
            }
            var ok = true;
            try {
                var momentDate = moment(value, 'YYYY/MM/DD');
            }
            catch (err) {
                ok = false;
            }
            return ok;
        });



    $(document).on("blur", "input, textarea", function (b) {
        $(this).val() ? $(this).addClass("has-value") : $(this).removeClass("has-value")
    });

    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    $.fn.modal.Constructor.prototype._enforceFocus = function () { };
}

function isNumberKey(e, element) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: Ctrl+C
        (e.keyCode == 67 && e.ctrlKey === true) ||
        // Allow: Ctrl+X
        (e.keyCode == 88 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;      
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

        e.preventDefault();
    }
    else {
        var len = element.val().length;
        var index = element.val().indexOf('.');
        if (index > 0 && e.charCode == 46) {
            e.preventDefault();
        }
        if (index > 0) {
            var CharAfterdot = (len + 1) - index;
            if (CharAfterdot > 3) {
                e.preventDefault();
            }
        }
    }
    return;
}

function FireUserDistricts() {

    // Multi Select 

    if ($('.multi-select').length) {
        $('.multi-select').multiselect({
            enableFiltering: true,
            enableHTML: true,
            numberDisplayed: 1,
            buttonClass: 'btn btn-default',
            buttonContainer: '<div class="btn-group multiselect-input" />',
            nonSelectedText: lang == 'ar' ? "لم يتم الإختيار" : "None selected",
            nSelectedText: lang == 'ar' ? "عنصر" : "selected",
            allSelectedText: lang == 'ar' ? "تم إختيار الكل" : "All selected",
            filterPlaceholder: lang == 'ar' ? "بحث" : "Search",
            selectAllText: lang == 'ar' ? " إختيار الكل" : " Select all",
            maxHeight: 250,
            includeSelectAllOption: true,
            templates: {
                button: '<span class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> &nbsp;<b class="fa fa-caret-down"></b></span>',
                ul: '<ul class="multiselect-container dropdown-menu"></ul>',
                filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
                filterClearBtn: '<span class="input-group-btn"><a class="btn btn-default btn-white btn-grey multiselect-clear-filter" ><i class="fa fa-times-circle red2"></i></button></span>',
                li: '<li><a tabindex="0"><label class="" ><input type="checkbox" class="ace ace-checkbox-2" /></label></a></li>',
                divider: '<li class="multiselect-item divider"></li>',
                liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
            }
        });
    }
}

function FireSearchFilter() {
    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    SetInputControls();
    $('#FilterModal').modal({ backdrop: "static", keyboard: false });
}

function FireAccess() {
    debugger;
    fileUploadFileName = $('[data-rel="imgControl"]').val();
    console.log("sdfsdfsdfsd12443")
    //   $("form").removeData("validator");
    //   $("form").removeData("unobtrusiveValidation");
    SetInputControls();
    $('#AccessModal').modal('show');
   // $('#AccessModal').modal({ backdrop: "static", keyboard: false });
    Inital_DateCalender();
    InitalSelect2V2();
    $('.dateCtrlParentDiv').each(function (e) {
        var $input = $(this);
        // debugger;
        convertDtFromGerToHijri($input.val(), $input.attr("hijriCalendar"));
    });
    InitAutoComplate();

    try {
        KTMenu.createInstances();
    } catch (e) {
        console.log(" errorr : KTMenu.createInstances")
    }

    try {
        //  debugger;
       /* setTimeout(function () { KTApp.initCountUp(); }, 1000);*/
        // setInterval(function () { KTApp.initCountUp(); }, 1000);

    } catch (e) {
        console.log(" errorr : KTApp.initCountUp()")

    }
}
function FireAccessSubModeal() {
    debugger
    $('#AccessModal').modal('show');
    $('#AccessSubModalattach').modal('show');
}
function FireAccessSub() {
    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    SetInputControls();
    $('#AccessSubModal').modal({ backdrop: "static", keyboard: false });
}

function CloseModel(data) {
    debugger;
    swal.fire({
        title: data.title,
        text: data.message,
        type: data.status,
        icon: data.status,
        confirmButtonClass: data.btnClass,
        confirmButtonText: data.btnText,
        showConfirmButton: 0,
        timer: 3000
    });
    if (data.close) {
        try {
            $(".close").click();
            var btnmodel = document.getElementById("BtnClose");
            btnmodel.click();
        } catch (e) {

        }
        try {
            $('#AccessModal').modal('hide');

        } catch (e) {

        }
        RefreshAjaxDt();
    }
}
function ajax_success_ByDiv(data) {
    debugger;
    swal.fire({
        title: data.title,
        text: data.message,
        icon: data.status,
        type: data.status,
        confirmButtonClass: data.btnClass,
        confirmButtonText: data.btnText,
        showConfirmButton: 0,
        timer: 3000
    });
    if (data.close) {
        try {
            $(this).parent("div").hide("slow");
            //$(".close").click();
            //var btnmodel = document.getElementById("BtnClose");
            //btnmodel.click();
        } catch (e) {

        }
        RefreshAjaxDt();
    }
}
function CloseSubModel(data) {
    $("#BtnSubClose").trigger("click");
}

function FinishLogin(data) {
    debugger;
    var re = data;
    if (re.status == "warning") {
        var userInfo = re.data;
        $.ajax({
            url: "/Account/login_ChangePass",//?UserName=" + userInfo.userName+ "&TempPassword=" + userInfo.tempPassword,
            type: "POST",
            // dataType: "json",
            data: {
                UserName: userInfo.userName,
                TempPassword: userInfo.tempPassword,
                ReturnUrl: userInfo.returnUrl
            },
            success: function (result) {

                $('#AccessSection').html(result);
                $('#AccessModal').modal('show');
            },
            error: function (result) {
                alert("عذرا ، هناك خطأ في النداء")
            }
        });
    }
    else {
        swal.fire({
            title: data.title,
            text: data.message,
            icon: data.status,
            type: data.status,
            confirmButtonClass: data.btnclass,
            confirmButtonText: data.btntext,
            showConfirmButton: 0,
            timer: 3000
        });
        if (data.url) {
            window.location.href = data.url;

        }
        else {
            try {
                grecaptcha.reset();
            } catch (e) {
                console.log("grecaptcha not reconize")
            }
        }
    }
}

function setScrollTable() {
    var language;
    if (lang == 'ar') {
        language = {
            "sSearch": "<span>بحث:</span> _INPUT_",
            "oPaginate": { "sFirst": "الأول", "sLast": "الأخير", "sNext": "التالي", "sPrevious": "السابق" },
            "sLengthMenu": "<span class='lengthLabel'>حجم الصفحة:</span><span class='lenghtMenu'> _MENU_</span>",
            "sInfo": '', //"عرض _START_ إلى _END_ من إجمالي _TOTAL_ سجل",
            "sInfoEmpty": "لا يوجد سجلات",
            "sProcessing": "جاري التحميل...",
            "sLoadingRecords": "جاري التحميل...",
            "sZeroRecords": "لا يوجد سجلات",
            "sInfoFiltered": "(تصفية من _MAX_ سجل)"
        };
    }
    else {
        language = {
            "sSearch": "<span>Search:</span> _INPUT_",
            "oPaginate": { "sFirst": "First", "sLast": "Last", "sNext": "Next", "sPrevious": "Previous" },
            "sLengthMenu": "<span class='lengthLabel'>Show:</span><span class='lenghtMenu'> _MENU_</span>",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
            "sInfoEmpty": "No data available",
            "sProcessing": "Loading ...",
            "sLoadingRecords": "Loading ...",
            "sZeroRecords": "No data available",
            "sInfoFiltered": "(filtered from _MAX_ total entries)"
        };
    }

    var scrollDt = $('[data-rel="scroll-table"]').DataTable({
        "scrollY": "305px",
        "scrollCollapse": true,
        "paging": false,
        "ordering": false,
        "searching": false,
        "oLanguage": language,
        "sDom": "<'row'<'col-sm-12 col-xs-12 scroll'f>r>t",
    });

    $('#AccessModal').on('shown.bs.modal', function () {
        scrollDt.columns.adjust();
    });

}

function getTableLanguage(lang) {
    var language;
    if (lang == 'ar') {
        language = {
            "sSearch": "<span>بحث:</span> _INPUT_",
            "oPaginate": '', //{ "sFirst": "الأول", "sLast": "الأخير", "sNext": "التالي", "sPrevious": "السابق" },
            "sLengthMenu": "<span class='lengthLabel'>حجم الصفحة:</span><span class='lenghtMenu'> _MENU_</span>",
            "sInfo": '', //"عرض _START_ إلى _END_ من إجمالي _TOTAL_ سجل",
            "sInfoEmpty": "لا يوجد سجلات",
            "sProcessing": "جاري التحميل...",
            "sLoadingRecords": "جاري التحميل...",
            "sZeroRecords": "لا يوجد سجلات",
            "sInfoFiltered": "(تصفية من _MAX_ سجل)"
        };
    }
    else {
        language = {
            "sSearch": "<span>Search:</span> _INPUT_",
            "oPaginate": '', //{ "sFirst": "First", "sLast": "Last", "sNext": "Next", "sPrevious": "Previous" },
            "sLengthMenu": "<span class='lengthLabel'>Show:</span><span class='lenghtMenu'> _MENU_</span>",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
            "sInfoEmpty": "No data available",
            "sProcessing": "Loading ...",
            "sLoadingRecords": "Loading ...",
            "sZeroRecords": "No data available",
            "sInfoFiltered": "(filtered from _MAX_ total entries)"
        };
    }
    return language;
}

function setTable(setting, tableId) {
    if (dt)
        dt.destroy();
    lang = setting.language;

    setting.bFilter = true;
    var language = getTableLanguage(setting.language);


    if (tableId == undefined || tableId == null || tableId == '') {
        tableId = '[data-rel="dashbord"]'
    } else {
        tableId = '#' + tableId
    }

    //  debugger;
    if (!setting.JsonData) {
        setting.JsonData = {};
    }
    if (!setting.initComplete) {
        setting.initComplete = function (settings, json) {


        }
    }

    if (setting.RowCallback) {
        dt = $(tableId).DataTable({
            "processing": false,
            "serverSide": true,
            "orderMulti": false,
            
            //"searching": false,
            "aaSorting": setting.sorting,
            "sDom": "<'row'<'col-sm-6 col-xs-12'l><'col-sm-6 col-xs-12'f>r>t<'row'<'col-sm-12 col-xs-12'i><'col-sm-12 col-xs-12'p>>",
            "searching": true, // Enable search bar in the view
            "ajax": {
                "url": setting.url,
                "type": "POST",
                "datatype": "json",
                "data": setting.JsonData
            },
            "columns": setting.columns,
            "oLanguage": language,
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $('td', nRow).css('background-color', aData.ColorCode);
            },
            "initComplete": setting.initComplete
        });
    }
    else {
        dt = $(tableId).DataTable({
            "processing": false,
            "serverSide": true,
            "orderMulti": false,
            "aaSorting": setting.sorting,
            "oLanguage": language,
           
            //"searching": false,
            "bFilter": setting.bFilter,
          //  "bFilter": setting.bFilter,
          //  "dom": setting.don,//"lfrti",
        //    "bLengthChange": setting.bLengthChange,// false, //thought this line could hide the LengthMenu
       //     "bInfo": setting.bInfo,// false,
         //   "sDom": "<'row'<'col-sm-6 col-xs-12'l><'col-sm-6 col-x-12'f>r>t<'row'<'col-sm-6 col-xs-12'i><'col-sm-6 col-xs-12'p>>",

            "sDom": "<'row'<'col-sm-6 align-items-end col-x-12'f><'col-sm-2 col-xs-12'l>r>t<'row'<'col-sm-12 col-xs-12'i><'col-sm-12 col-xs-12'p>>",

            "searching": true, // Enable search bar in the view
            "ajax": {
                "url": setting.url,
                "type": "POST",
                "datatype": "json",
                "data": setting.JsonData
            },
            "columns": setting.columns,
            "initComplete": setting.initComplete,
            "fnDrawCallback": setting.fnDrawCallback

        });
        Inital_format_Money_number();
    }

    return dt;
}

function RefreshAjaxDt() {
    // debugger;
    if (dt)
        dt.ajax.reload(null, false);
}

function RefreshDataDt() {
    dt.data.reload(null, false);
}

function setuploadtools() {

    if (!fileUploadFileType) {
        fileUploadFileType = ["image\/gif", "image\/GIF", "image\/jpeg", "image\/jpg", "image\/JPG", "image\/png", "image\/PNG"];
    }
    //uploadSettings.hintText = fileUploadText;
    //uploadSettings.uploadScript = fileUploadUrl;
    //uploadSettings.imageName = fileUploadFileName;
    //uploadSettings.path = fileUploadPath;
    //uploadSettings.fileType = fileUploadFileType;
    //uploadSettings.hintIco = fileUploadFileIcon;
    //uploadSettings.toolId = '[data-rel="upload"]';
    //uploadSettings.contentId = '[data-rel="imgContent"]';
    //uploadSettings.intputId = '[data-rel="imgControl"]';
    setUploadControl();
}
function setUploadControl() {
    $(".dz_file").each(function (e) {
        var $dz = $(this);
        var id = this.id;
        var url = $dz.attr("dz-url");
        var name = $dz.attr("dz-name");
        var paramName = $dz.attr("dz-paramName");
        var maxFiles = $dz.attr("dz-maxFiles");
        debugger;
        myDropzone = new Dropzone("#" + id, {
            url: url,//"https://keenthemes.com/scripts/void.php", // Set the url for your upload script location
            paramName: paramName,// "file", // The name that will be used to transfer the file
            maxFiles: maxFiles,
            maxFilesize: 10, // MB
            addRemoveLinks: true,
            accept: function (file, done) {
                if (file.name == "wow.jpg") {
                    done("Naha, you don't.");
                } else {
                    done();
                }
            },
            success: function (file, response) {
        debugger;
                //Here you can get your response.

                console.log(response);
                if (response.status == "success") {
                    var fileuploded = file.previewElement.querySelector("[data-dz-name]");
                    fileuploded.innerHTML = response.data + '<input type="hidden" class="dropzonejs_hf" id="df_hf_' + response.data + '" name="' + name+'" />';
                  //  var btndelete = file.previewElement.querySelector(".dz-remove");
                   // btndelete.setAttribute("id", 'delete-midia-id-' + serverResponse.midia_id);
                 //   var old_val = $dz.find(".dropzonejs_hf").val();
                  //  $dz.find(".dropzonejs_hf").val(old_val + response.data)
                }
                else {

                }
               
            }
        });
        
    });
 
}
var pos;
function SetMap(Currentlat, Currentlng, Currentplace, Currentzoom, PlacesTypes, MarkerTitle) {

    pos = new google.maps.LatLng(Currentlat, Currentlng);

    var txtlat = $("#Lat");
    var txtlng = $("#Lng");
    var mapOptions = {
        zoom: Currentzoom,
        center: pos
    };

    txtlat.val(pos.lat());
    txtlng.val(pos.lng());

    mymap = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    var infowindow = new google.maps.InfoWindow({
        content: ''
    });

    marker = new google.maps.Marker({
        position: pos,
        map: mymap,
        title: MarkerTitle,
        draggable: true,
        animation: google.maps.Animation.DROP
    });
    //console.log(Currentplace);

    // Create the search box and link it to the UI element.       
    var input = document.getElementById('mapSearchInput');

    mymap.controls[google.maps.ControlPosition.TOP_RIGHT].push(input);

    var autocomplete = new google.maps.places.Autocomplete(input);

    // Bind the map's bounds (viewport) property to the autocomplete object,
    // so that the autocomplete requests use the current map bounds for the
    // bounds option in the request.
    autocomplete.bindTo('bounds', mymap);

    autocomplete.setTypes(PlacesTypes);

    autocomplete.addListener('place_changed', function () {
        infowindow.close();
        marker.setVisible(false);
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            // User entered the name of a Place that was not suggested and
            // pressed the Enter key, or the Place Details request failed.
            $("#PlaceID").val('');
            window.alert("No details available for input: '" + place.name + "'");
            return;
        }

        mymap.setCenter(place.geometry.location);
        mymap.setZoom(Currentzoom);

        marker.setPosition(place.geometry.location);
        marker.setVisible(true);

        txtlat.val(marker.getPosition().lat());
        txtlng.val(marker.getPosition().lng());

        getPlaceDetails(marker.getPosition(), Currentplace);
        mymap.setCenter(place.geometry.location);

        var address = '';
        if (place.address_components) {
            address = [
                (place.address_components[0] && place.address_components[0].short_name || ''),
                (place.address_components[1] && place.address_components[1].short_name || ''),
                (place.address_components[2] && place.address_components[2].short_name || '')
            ].join(' ');
        }

        infowindow.setContent(address);
        infowindow.open(mymap, marker);

    });

    google.maps.event.addListener(marker, 'click', function () {

        infowindow.open(mymap, marker);

    });

    google.maps.event.addListener(marker, 'dragend', function () {

        txtlat.val(marker.getPosition().lat());
        txtlng.val(marker.getPosition().lng());

        getPlaceDetails(marker.getPosition(), Currentplace);

        mymap.setCenter(marker.getPosition());

    });

    google.maps.event.addListener(mymap, 'click', function (event) {

        var service = new google.maps.places.PlacesService(mymap);

        getPlaceDetails(event.latLng, Currentplace);

        txtlat.val(event.latLng.lat());
        txtlng.val(event.latLng.lng());
        marker.setPosition(event.latLng);
        marker.setVisible(true);
        mymap.setCenter(event.latLng);

    });

    $('#AccessModal').on('shown.bs.modal', function () {
        google.maps.event.trigger(mymap, "resize");
        mymap.setCenter(pos);
    });

    if ($('a[data-toggle="tab"]').length) {
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            google.maps.event.trigger(mymap, "resize");
            mymap.setCenter(pos);
        });
    }
}

function SetMarker(Currentlat, Currentlng, Currentzoom) {
    pos = new google.maps.LatLng(Currentlat, Currentlng);
    $("#Lat").val(Currentlat);
    $("#Lng").val(Currentlng);
    if ($("#PlaceID").length)
        $("#PlaceID").val('');
    marker.setPosition(pos);
    marker.setVisible(true);
    mymap.setZoom(Currentzoom);
    mymap.setCenter(pos);

}

function getPlaceDetails(latLng, placetype) {
    var placeDetails = [];
    var geocoder = new google.maps.Geocoder();

    geocoder.geocode({
        'latLng': latLng
    }, function (results, status) {
        var address = '';
        console.log(results);
        for (var i = 0; i < results.length; i++) {
            var types = 'types:' + results[i].types;

            address += 'type:' + results[i].types + '\nname:' + results[i].formatted_address + '\nplaveId:' + results[i].place_id + '\n\n';
            if (types.toLowerCase().indexOf("street") >= 0 || types.toLowerCase().indexOf("route") >= 0) {
                placeDetails.push({
                    "type": 'street',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
            if (types.toLowerCase().indexOf("country") >= 0) {
                placeDetails.push({
                    "type": 'country',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
            if (types.toLowerCase().indexOf("sublocality") >= 0 || types.toLowerCase().indexOf("neighborhood") >= 0 || types.toLowerCase().indexOf("administrative_area_level_3") >= 0) {
                placeDetails.push({
                    "type": 'district',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
            else if (types.toLowerCase().indexOf("locality") >= 0) {
                placeDetails.push({
                    "type": 'city',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
        }
        var isMatched = false;

        for (var i = 0; i < placeDetails.length; i++) {
            if (placeDetails[i].type == placetype) {
                if ($("#PlaceID").length)
                    $("#PlaceID").val(placeDetails[i].place_id);
                isMatched = true;
            }
        }
        if (!isMatched) {
            if ($("#PlaceID").length)
                $("#PlaceID").val('');
        }
    });


    return placeDetails;
}

function getPlaceDetails(latLng, placetype) {
    var placeDetails = [];
    var geocoder = new google.maps.Geocoder();

    geocoder.geocode({
        'latLng': latLng
    }, function (results, status) {
        var address = '';
        console.log(results);
        for (var i = 0; i < results.length; i++) {
            var types = 'types:' + results[i].types;

            address += 'type:' + results[i].types + '\nname:' + results[i].formatted_address + '\nplaveId:' + results[i].place_id + '\n\n';
            if (types.toLowerCase().indexOf("street") >= 0 || types.toLowerCase().indexOf("route") >= 0) {
                placeDetails.push({
                    "type": 'street',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
            if (types.toLowerCase().indexOf("country") >= 0) {
                placeDetails.push({
                    "type": 'country',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
            if (types.toLowerCase().indexOf("sublocality") >= 0 || types.toLowerCase().indexOf("neighborhood") >= 0 || types.toLowerCase().indexOf("administrative_area_level_3") >= 0) {
                placeDetails.push({
                    "type": 'district',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
            else if (types.toLowerCase().indexOf("locality") >= 0) {
                placeDetails.push({
                    "type": 'city',
                    "name": results[i].formatted_address,
                    "place_id": results[i].place_id
                });
            }
        }
        var isMatched = false;

        for (var i = 0; i < placeDetails.length; i++) {
            if (placeDetails[i].type == placetype) {
                if ($("#PlaceID").length)
                    $("#PlaceID").val(placeDetails[i].place_id);
                isMatched = true;
            }
        }
        if (!isMatched) {
            if ($("#PlaceID").length)
                $("#PlaceID").val('');
        }
    });


    return placeDetails;
}
function InitalSelect2V2() {
    try {
        var $select = $('.select2_V_1');
        $select.select2({
            width: null,
            containerCssClass: ':all:',
            dir: 'rtl',
            tags: true,
            createTag: function (params) {
                var term = $.trim(params.term);
                return {
                    id: term,
                    text: term,
                    newTag: "true"
                }
            }
        }).on('select2:select', function (evt) {
            if (evt.params.data.newTag == "true") {

            }
        });
        $select.attr('dir', 'rtl');
    } catch (e) {
        console.log("select2 is not definde :" + e)
    }
}
function InitAutoComplate() {
    try {
        $('#search').autocomplete({
            source: function (request, response) {
                var autocompleteUrl = '../../Admin/Users/AutoCompleteUSer/';
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    data: {
                        name: $('#search').val()
                    },
                    success: function (json) {
                        debugger;
                        if (json.status == "success") {
                            var dd = json.data;

                            // call autocomplete callback method with results
                            //console.log(json[0]["name"])
                            //console.log(json[0]["mobile"])
                            //console.log(json.d[0]["mail"])
                            //console.log(json.d[0]["mobile"])
                            response($.map(dd, function (data, id) {

                                return {
                                    //label: data.Role,
                                    //value: data.ItemId
                                    label: data.name,
                                    value: data.mail,
                                    value2: data.mobile,
                                    name_ar: data.name_ar,
                                    name_en: data.name_en,
                                    //    value3 = data.userPrincipalName


                                };

                            }));
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                //  alert('you have selected ' + ui.item.label + ' ID: ' + ui.item.value);
                //   $('#search').val(ui.item.label);

                debugger;

                var str = ui.item.value.split("@@")[0];

                // var x = ui.item.value;
                // x = x.substring(0, x.lastIndexOf(y));
                //$(".ArabicUserName").val(ui.item.label);
                $(".ArabicUserName").val(ui.item.name_ar);
                $(".EnglishUserName").val(ui.item.name_en);
                $(".Email").val(ui.item.value);
                $(".Mobile").val(ui.item.value2);
                $(".UserName").val(str);
                return false;
            }
        });
    } catch (e) {
        console.log("autocomplete is not definde :" + e)

    }

}






function Inital_DateCalender() {
    try {
        $(".datetimepicker").daterangepicker({
            timePicker: true,
            timePicker24Hour: true,
            singleDatePicker: true,
            showDropdowns: true,
            minYear: 1901,
            locale: {
                format: 'YYYY/MM/DD H:mm'
            },
            //   maxYear: parseInt(moment().format("YYYY"), 10)
        }, function (start, end, label) {
            //var years = moment().diff(start, "years");
            //alert("You are " + years + " years old!");
        }
        );
        //$('.datetimepicker').datetimepicker("remove");
        //$('.datetimepicker').datetimepicker({ format: "yyyy/mm/dd hh:ii", todayBtn: true, autoclose:true });
    } catch (e) {
        console.log("error on daterangepicker : " + e);
    }
    try {
        //$('.dateCtrlParentDiv.is-calendarsPicker')
        $('.dateCtrlParentDiv').calendarsPicker($.extend({
            calendar: $.calendars.instance(),
            onSelect: function (date) {
                $(this).addClass("has-value");
                convertDtFromGerToHijri(date, $(this).attr("hijriCalendar"));
            },
            showTrigger: '#calImg',
            dateFormat: 'yyyy/mm/dd',
        }));
    } catch (e) {
        console.log("error on calendarsPicker : " + e);
    }
    try {


        //$('.dateCtrlParentDivHijri.is-calendarsPicker')
        $('.dateCtrlParentDivHijri').calendarsPicker($.extend({
            calendar: $.calendars.instance('ummalqura', 'ar'),
            onSelect: function (date) {
                $(this).addClass("has-value");
                convertDtFromHijriToGer(date, $(this).attr("gregorianCalendar"));
            },
            showTrigger: '#calImg',
            dateFormat: 'yyyy/mm/dd',
        },
            $.calendarsPicker.regionalOptions['ar'])
        );
        //debugger;

        $.each($('.dateCtrlParentDiv'), function (i, v) {
            // debugger;
            $(v).prop('autocomplete', 'off')
            if (!isNaN(new Date($(v).val()))) {

                var dateVal = $(v).val().split(" ")[0];
                if ($(v).val().split(" ")[1] == "" || $(v).val().split(" ")[1] == undefined) {
                    formatedDate = dateVal;
                }
                else {
                    dateArr = dateVal.split('/');
                    formatedDate = dateArr[2] + '/' + dateArr[0] + '/' + dateArr[1];
                }


                $(v).calendarsPicker('setDate', formatedDate);
                // convertDtFromGerToHijriCustome(new Date(formatedDate), $(v).attr("hijriCalendar"));
            }

        });
    } catch (e) {
        console.log("error on calendarsPicker : " + e);
    }
}

$(function () {
    Inital_DateCalender();
    Inital_format_Money_number();

});
$(document).ajaxComplete(function () {
    Inital_format_Money_number();
})
function Inital_TimePicker() {
    $('.timePickerCtrlDivEn').clockpicker({
        'default': 'now',
        vibrate: true,
        placement: "top",
        align: "right",
        autoclose: false,
        twelvehour: true,
    })
}
$(function () {
    //  Inital_TimePicker();
});

function Inital_format_Money_number() {
    $(".format_no").each(function () {
        // debugger;
        var $no = $(this);
        var no_val = $no.text();
        if (parseFloat(no_val).toString() == "NaN") {

        }
        else {
            $no.text(numberWithCommas(no_val));

        }
        $no.removeClass("format_no");

    });
    // var val = parseInt($('#value').text());
    //Use the code in the answer above to replace the commas.
    // val = numberWithCommas(val);
    // $('#value').text(val);

}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
