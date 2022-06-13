using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reprocess.Model.Metrics.Dto;
using Reprocess.Service.Metrics.IService;

namespace Reprocess.Web.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        private readonly IStudentService _studentService;
        private readonly IReprocessService _mtService;
        public EmployeeController(IStudentService studentService, IReprocessService mtService)
        {
            _studentService = studentService;
            _mtService = mtService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost] //TrackSearchData searchParams
        public JsonResult TrackStatusFile(List<TrackSearchData> trackDataList)
        {
            try
            {
                ///check count list input        
                trackDataList.FirstOrDefault().FlowId = 1; //CIP            
                var result = _mtService.GetTrackDataGridsV1(trackDataList);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //todo log
                return Json(new List<TrackSearchData>(), JsonRequestBehavior.AllowGet);
            }
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
                if (id > 0)
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
                if (_studentService.IsValidStudentData(student))
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
                return Json(new { IsSuccess = false, JsonRequestBehavior.AllowGet });
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
                if (_studentService.IsExistStudentInDB(id))
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