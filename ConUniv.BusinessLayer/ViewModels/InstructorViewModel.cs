using ConUniv.Models;
using DataAccess.Scaffold.Attributes;
using DataAccess.Scaffold.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConUniv.BusinessLayer.ViewModels
{
    [NeedletailViewModel]
    public class InstructorViewModel : ViewModelAutoLoadAndSaveAsync
    {

        [HasOne("Office", "Id", "OfficeAssignment", "InstructorID")]
        [HasManyNtoN("Courses", "Id", "CourseInstructor", "InstructorId", "CourseID", "Course", "Id")]
        public Instructor Instructor { get; set; }

        public OfficeAssignment Office { get; set; }

        public IList<Course> Courses { get; set; }


        public override async Task FillDataAsync(object primaryKey)
        {
            await base.FillDataAsync(primaryKey);
            if (this.Instructor.Id == Guid.Empty)
                this.Instructor.HireDate = DateTime.Now;
        }

    }
}
