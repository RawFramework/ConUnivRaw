define(['jquery', 'bootstrapValidator', 'moment', 'bootstrapDateTimePicker'], function ($) {
    var initValidator = function () {
        $('#StartDate').datetimepicker({
            pickTime: false,
            useMinutes: false,               
            useSeconds: false,               
            useCurrent: false
        });

        $(".department_form").bootstrapValidator({
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

                Name: {
                     message: 'The Name is not valid',
                     validators: {
                        max: {
                            max: 50
                          }
                     }
                   },

                Budget: {
                     message: 'The Budget is not valid',
                     validators: {
                        numeric: {
                            
                          },
notEmpty: {
                            
                          }
                     }
                   },

                StartDate: {
                     message: 'The StartDate is not valid',
                     validators: {
                        date: {
                            
                          }
                     }
                   },

                InstructorID: {
                     message: 'The InstructorID is not valid',
                     validators: {
                        
                     }
                   },
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();

            var $form = $(e.target);

            

            viewModel.Departments()[0].Department.StartDate = viewModel.dateSelected();

            var jsonObj = viewModel.Departments()[0];
            
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

    $('#StartDate')
        .on('dp.change dp.show', function (e) {
            // Revalidate the date when user change it
            $('.user_form').bootstrapValidator('revalidateField', 'StartDate');
        });
    
    var formValidator = {        
        initViewModel: initViewModel,
        initValidator: initValidator
    };
    

    return formValidator;
});
