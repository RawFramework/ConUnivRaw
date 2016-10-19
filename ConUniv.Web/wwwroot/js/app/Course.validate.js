define(['jquery', 'bootstrapValidator', 'moment', 'bootstrapDateTimePicker'], function ($) {
    var initValidator = function () {
        $('#').datetimepicker({
            pickTime: false,
            useMinutes: false,               
            useSeconds: false,               
            useCurrent: false
        });

        $(".course_form").bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                
                Id: {
                     message: 'The Id is not valid',
                     validators: {
                        notEmpty: {
                            
                          }
                     }
                   },

                Title: {
                     message: 'The Title is not valid',
                     validators: {
                        notEmpty: {
                            max: 50
                          },
max: {
                            
                          }
                     }
                   },

                Credits: {
                     message: 'The Credits is not valid',
                     validators: {
                        numeric: {
                            
                          },
notEmpty: {
                            
                          }
                     }
                   },

                DepartmentID: {
                     message: 'The DepartmentID is not valid',
                     validators: {
                        notEmpty: {
                            
                          }
                     }
                   },
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();

            var $form = $(e.target);


            

            var jsonObj = viewModel.Courses()[0];
            
            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                dataType: "json",
                data: JSON.stringify(jsonObj),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                  window.history.back();
                }
            });
        });
    };

    var initViewModel = function (model) {
        viewModel = model;
    };

    $('#')
        .on('dp.change dp.show', function (e) {
            // Revalidate the date when user change it
            $('.user_form').bootstrapValidator('revalidateField', '');
        });
    
    var formValidator = {        
        initViewModel: initViewModel,
        initValidator: initValidator
    };
    

    return formValidator;
});
