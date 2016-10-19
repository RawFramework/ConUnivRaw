using ConUniv.Models;
using ConUniv.Repositories;
using DataAccess.Scaffold.Attributes;
using DataAccess.Scaffold.ViewModels;
using System;
using System.Threading.Tasks;

namespace ConUniv.BusinessLayer.ViewModels
{
    [NeedletailViewModel]
    public class DepartmentViewModel : ViewModelAutoLoadAndSaveAsync
    {

        public string InstructorIDName { get; set; }

        public string StartDate { get; set; }

        [Autocomplete("InstructorID", "Instructor", "Id", "FirstName", "FirstName", "FirstName")]
        public Department Department { get; set; }

        public override async Task FillDataAsync(object primaryKey)
        {
            await base.FillDataAsync(primaryKey);
            if (this.Department.Id == Guid.Empty)
                this.Department.StartDate = DateTime.Now;
            await FillOtherData();
        }

        private async Task FillOtherData()
        {
            if (this.Department.InstructorID.HasValue)
            {
                using (var iRepo = new InstructorRepository())
                {
                    var inst = await iRepo.GetSingleAsync(where: new { Id = this.Department.InstructorID });
                    if (inst != null)
                        InstructorIDName = inst.FirstName + " " + inst.LastName;
                }
            }

            this.StartDate = this.Department.StartDate.ToString("MM-dd-yyyy");
        }

    }
}
