using Reprocess.Model.Metrics;
using Reprocess.Model.Metrics.Dto;
using Reprocess.Service.Metrics.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reprocess.Web.Controllers
{
    public class MetricsReprocessController : Controller
    {
        private readonly IReprocessService _mtService;
        public MetricsReprocessController(IReprocessService mtService)
        {
            _mtService = mtService;
        }
        // GET: MetricsReprocess
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost] //TrackSearchData searchParams
        public JsonResult TrackFile(TrackSearchData searchParams)
        {
            try
            {
                searchParams.Files = "\\DIStorage1\\Data\\ASCII\\202202\\FRANCESCA_20573@ftp\\2d25e93f-cfaa-48a0-9758-544a3594b50c_21637_202202_20574_20573_856_004030_1252_inbox_323059005646752926_inbox.ascii";
                searchParams.FlowId = 1; //CIP
                if (_mtService.CheckInputTrackData(searchParams))
                {
                    var result = _mtService.GetTrackDataGrids(searchParams);
                    return Json(result, JsonRequestBehavior.AllowGet);
                    //files = _reprocessService.GetFiles(searchParams.Files);
                    //var filePath = _reprocessService.ParseFileName(files[0]);
                    //string status = _reprocessService.GetIntegrationStatusInbox(filePath[2], filePath[1], filePath[4]);
                }
                return Json(new TrackDataGrid(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //todo log
                return Json(new TrackDataGrid(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost] //TrackSearchData searchParams
        public JsonResult TrackStatusFile(List<TrackSearchData> trackDataList)
        {
            try
            {
                if(trackDataList.Any())
                {
                    var result = _mtService.GetTrackDataGridsV1(trackDataList);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //todo log
                return Json(new List<TrackSearchData>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost] //TrackSearchData searchParams
        public JsonResult ReprocessFiles(ReprocessSearchData reprocessSearchData)
        {

            try
            {
                ///check count list input    


                return null;
                //trackDataList.FirstOrDefault().FlowId = 1; //CIP            
                //var result = _mtService.GetTrackDataGridsV1(trackDataList);
                //return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //todo log
                return Json(new List<TrackSearchData>(), JsonRequestBehavior.AllowGet);
            }
        }


    }
}