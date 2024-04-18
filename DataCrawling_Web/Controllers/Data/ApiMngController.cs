using DataCrawling_Web.Service.Api;
using System.Web.Mvc;
using System.Windows.Forms.Design;

namespace DataCrawling_Web.Controllers.Data
{
    public class ApiMngController : Controller
    {
        ApiService service;

        public ApiMngController()
        {
            service = new ApiService();
        }


        [Route("Data/ApiMng/List")]
        public ActionResult List()
        {
            return View("~/Views/Data/ApiMng/List.cshtml");
        }

        [Route("Data/ApiMng/Scrap")]
        public ActionResult Scrap()
        {
            var data=service.GenerateSampleData(100);
            return View("~/Views/Data/ApiMng/Scrap.cshtml", data);
        }

        [Route("Data/ApiMng/AutoWeb")]
        public ActionResult AutoWeb()
        {
            return View("~/Views/Data/ApiMng/AutoWeb.cshtml");
        }

        [Route("Data/ApiMng/AutoWork")]
        public ActionResult AutoWork()
        {
            return View("~/Views/Data/ApiMng/AutoWork.cshtml");
        }
    }
}