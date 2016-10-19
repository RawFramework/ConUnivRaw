using ConUniv.Models;
using DataAccess.Scaffold.Attributes;
using DataAccess.Scaffold.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConUniv.BusinessLayer.ViewModels
{
    [NeedletailViewModel]
    public class StudentViewModel : ViewModelAutoLoadAndSaveAsync
    {

        [HasManyNtoN("Courses", "Id", "Enrollment", "StudentId", "CourseID", "Course", "Id")]
        public Student Student { get; set; }

        public IList<Course> Courses { get; set; }


        public override async Task FillDataAsync(object primaryKey)
        {
            await base.FillDataAsync(primaryKey);
            if (this.Student.Id == Guid.Empty)
                this.Student.EnrollmentDate = DateTime.Now;
        }
    }
}
