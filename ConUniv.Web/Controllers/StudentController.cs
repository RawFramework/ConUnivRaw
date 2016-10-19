using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using ConUniv.Repositories;
using ConUniv.Models;
using ConUniv.BusinessLayer.ViewModels;


namespace ConUniv.Controllers
{

    //Basic Controller template, avoid adding bussiness logic code here, use the Repository instead
    public class StudentController : Controller
    {
        public Dictionary<string, Func<Student, IComparable>> DynamicOrdering = new Dictionary<string, Func<Student, IComparable>>
        {
            {"Id",opt => opt.Id},
            {"LastName",opt => opt.LastName},
            {"FirstName",opt => opt.FirstName},
            {"EnrollmentDate",opt => opt.EnrollmentDate},
            {"Discriminator",opt => opt.Discriminator}
        };

        
        StudentRepository studentRepository;

        public StudentController() {

            //Remove this line if you want to use dependency injection 
            
        studentRepository = new StudentRepository();
        }

        //
        // GET: /Student/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Student/Details/id

        public ActionResult Details(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]StudentViewModel model)
        {

            model.Student.Id = Guid.NewGuid();
            
             await model.SaveAsync();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Student") });
        }

        //
        // GET: /Student/Edit/id

        public ActionResult Edit(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // POST: /Student/Edit/id

        [HttpPost]
        public async Task<ActionResult> Edit([FromBody]StudentViewModel model)
        {
            await model.SaveAsync();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Student") });
        }


        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public async Task<bool> Delete(Guid id)
        {
            var result = await studentRepository.DeleteAsync(where: new {Id = id});
            return result;
        }

        #region student data access

        [HttpPost]
        public JsonResult GetStudents()
        {
            var students = studentRepository.GetAll();

            var result = new {
                students = students
            };

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetStudent(Guid id)
        {
            
            
            var student = new StudentViewModel();
            await student.FillDataAsync(primaryKey: id);

            var result = new
            {
                student = student
            };

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> AddCourse(Guid studentId, Guid courseId)
        {
            using (var eRepo = new EnrollmentRepository())
            {
                await eRepo.InsertAsync(new Enrollment { Id = Guid.NewGuid(), CourseID = courseId, StudentID = studentId, Grade = 1 });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCourse(Guid ciId)
        {
            using (var icRepo = new EnrollmentRepository())
            {
                await icRepo.DeleteAsync(where: new { CourseId = ciId });
            }
            return Json(new { success = true });
        }



        [HttpPost]
        public async Task<JsonResult> GetAllStudent(int page_num, int rows_per_page)
        {
            var list = new List<dynamic>();
            var sortField = this.Request.Form["sorting[0][field]"];
            var sort = this.Request.Form["sorting[0][order]"];
            
            var totalRows = await studentRepository.ExecuteScalarAsync<int>("SELECT Count(Id) FROM student", new Dictionary<string, object>());
            var registers = await studentRepository.GetManyAsync(
                new {},
                new {},
                Needletail.DataAccess.Engines.FilterType.AND,
                page_num - 1,
                rows_per_page
            );

            registers = (sort == "descending") ?
                registers.OrderByDescending(DynamicOrdering[sortField]) :
                registers.OrderBy(DynamicOrdering[sortField]);

            foreach (var row in registers)
            {
                var objId = row.Id.ToString();
                list.Add(new
                {   
                    Id = row.Id,
                    LastName = row.LastName,
                    FirstName = row.FirstName,
                    EnrollmentDate = row.EnrollmentDate,
                    Discriminator = row.Discriminator,
                    edit = string.Format("<a href = \"/student/edit/{0}\">Edit</a>", objId),
                    details = string.Format("<a href = \"/student/Details/{0}\">Details</a>", objId),
                    delete = string.Format("<a href = \"/student/delete/{0}\">Delete</a>", objId)
                });
            }
 
            return Json(new { total_rows = totalRows, page_data = list });;
        }

        //By default the Id is a Guid, change it to whatever you need
        public async Task<JsonResult> GetTextForAutocomplete(Guid id, string idField, string showField)
        {
            var found = (await studentRepository.GetManyAsync(
                where: string.Format("{0} = @id", idField),
                orderBy: "",
                args: new Dictionary<string, object> { { "id", id } },
                topN: null)).FirstOrDefault();
            if(found == null)
                return Json(new { name = "" });    
            
            var show = found.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == showField.ToLower());            
            return Json(new { name = show.GetValue(found) });
        }

        public async Task<JsonResult> Search(string query, string searchField, string idField, string showField, string order)
        {
            query = query.Trim();
            if (!query.StartsWith("%"))
                query = "%" + query;
            if (!query.EndsWith("%"))
                query += "%";
            var found = await studentRepository.GetManyAsync(
                where: string.Format("{0} like @query", searchField),
                orderBy: order + " DESC",
                args: new Dictionary<string, object> { { "query", query } },
                topN: null);

            if (found == null || found.Count() == 0)
                return Json(new List<object> { new { id = "00000000-0000-0000-0000-000000000000", name = string.Format("{0}: No results found", query.Replace("%","")) } });

            //create the object to return
            var allFound = new List<object>();
            var id = found.First().GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == idField.ToLower());
            var show = found.First().GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == showField.ToLower());
            if (id == null || show == null)
                throw new Exception("Id field or Show fields doest not belog to the entity");
            foreach (var f in found)
            { 
                allFound.Add(new { id = id.GetValue(f), name = show.GetValue(f) });
            }

            return Json(allFound);
        }

        #endregion
    }
}
