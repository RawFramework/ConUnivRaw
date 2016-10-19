define(['jquery', 'knockout', 'underscore', 'moment'], function ($, ko, _, moment) {
    if ($("#refresh").val() == 'yes') { location.reload(); } else { $('#refresh').val('yes'); }
        
    var modelDepartment = [
    ];

    ko.bindingHandlers.datetimepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {

            var options = {
                pickTime: false,
                defaultDate: DepartmentAppViewModel.dateSelected()
            };

            $(element).parent().datetimepicker(options);

            ko.utils.registerEventHandler($(element).parent(), "change.dp", function (event) {
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    var thedate = $(element).parent().data("DateTimePicker").getDate();
                    value(moment(thedate).toDate());
                }
            });
        },
        update: function (element, valueAccessor) {
            var widget = $(element).parent().data("DateTimePicker");
            //when the view model is updated, update the widget
            var thedate = new Date(parseInt(ko.utils.unwrapObservable(valueAccessor()).toString().substr(6)));
            widget.setDate(thedate);
        }
    };

    var DepartmentAppViewModel = {
        Departments: ko.observableArray(
            modelDepartment
        ),

        add : function (element) {
            var that = this;

            
            var date = element.Department ? element.Department.StartDate : element.StartDate;
            that.dateSelected = ko.observable(date);
            that.Departments.push(element);
        },

        remove: function () {
            var self = this;
            $.post("/Department/Delete", { id: this.Id }, function (success) {
                if (Boolean(success)) {
                    DepartmentAppViewModel.Departments.remove(self);
                }
            });
        },

        rootElement: "",

        dateSelected: ko.observable()


    };

    $(document).ready(function () {
        //if a root element is defined, then use it
        if(DepartmentAppViewModel.rootElement)
            ko.applyBindings(DepartmentAppViewModel ); 
        else
            ko.applyBindings(DepartmentAppViewModel,document.getElementById(DepartmentAppViewModel.rootElement)); 
    });
    return DepartmentAppViewModel;

});
