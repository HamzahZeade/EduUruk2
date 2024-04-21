// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function (e) {
    let fv;
    fv = FormValidation.formValidation(document.getElementById('form-register'), {
        fields: {
            'txtEmail': {
                validators: {
                    notEmpty: {
                        message: 'يجب إدخال البريد الإلكتروني',
                    },
                },
            },
        },
        plugins: {
            declarative: new FormValidation.plugins.Declarative({
                html5Input: true,
            }),
            internationalTelephoneInput: new FormValidation.plugins.InternationalTelephoneInput({
                field: 'mobile1',
                message: 'يجب كتابة رقم الجوال بالشكل الصحيح.',
                utilsScript: 'https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.3/js/utils.js',
                initialCountry: "sa",
                preferredCountries: ["sa", "ae", "kw", "bh", 'qa', 'om'],
            }),
            submitButton: new FormValidation.plugins.SubmitButton(),
            defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5(),
        },
    });

});

