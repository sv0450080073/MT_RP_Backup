using Reprocess.Model.Metrics.Dto;
using Reprocess.Service.Metrics.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reprocess.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        // GET: Student
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public JsonResult GetStudent()
        {           
            try
            {
                var result = _studentService.GetStudents();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStudentById(int id)
        {
            try
            {
                if(id > 0 )
                {
                    var result = _studentService.GetStudentById(id);
                    return Json(new { IsSuccess = result, JsonRequestBehavior.AllowGet });
                }
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
        }
        public JsonResult InsertStudent(StudentEntity student)
        {
            try
            {     
                if(_studentService.IsValidStudentData(student))
                {
                    var result = _studentService.InsertStudent(student);
                    return Json(new { IsSuccess = result, JsonRequestBehavior.AllowGet });
              
                }
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPut]
        public JsonResult UpdateStudent(StudentEntity student)
        {
            try
            {
                if (_studentService.IsValidStudentData(student))
                {
                    var result = _studentService.UpdateStudent(student);
                    return Json(new { IsSuccess = result, JsonRequestBehavior.AllowGet });
                }
                return Json( new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpDelete]
        public JsonResult DeleteStudentById(int id)
        {
            try
            {
                if(_studentService.IsExistStudentInDB(id))
                {
                    var result = _studentService.DeleteStudent(id);
                    return Json(new { IsSuccess = result, JsonRequestBehavior.AllowGet });                   
                }
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
            }
        }
    }
}