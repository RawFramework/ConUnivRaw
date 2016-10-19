define(['jquery', 'knockout', 'underscore', 'moment'], function ($, ko, _, moment) {
    if ($("#refresh").val() == 'yes') { location.reload(); } else { $('#refresh').val('yes'); }
        
    var modelCourse = [
    ];

    ko.bindingHandlers.datetimepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {

            var options = {
                pickTime: false,
                defaultDate: CourseAppViewModel.dateSelected()
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

    var CourseAppViewModel = {
        Courses: ko.observableArray(
            modelCourse
        ),

        add : function (element) {
            var that = this;
            
            
            that.Courses.push(element);
        },

        remove: function () {
            var self = this;
            $.post("/Course/Delete", { id: this.Id }, function (success) {
                if (Boolean(success)) {
                    CourseAppViewModel.Courses.remove(self);
                }
            });
        },

        rootElement: "",

        dateSelected: ko.observable()


    };

    $(document).ready(function () {
        //if a root element is defined, then use it
        if(CourseAppViewModel.rootElement)
            ko.applyBindings(CourseAppViewModel ); 
        else
            ko.applyBindings(CourseAppViewModel,document.getElementById(CourseAppViewModel.rootElement)); 
    });
    return CourseAppViewModel;

});
