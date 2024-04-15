using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Data
{
    public class ApiMngController : Controller
    {
        [Route("Data/ApiMng/List")]
        public ActionResult List()
        {
            return View("~/Views/Data/ApiMng/List.cshtml");
        }

        [Route("Data/ApiMng/Scrap")]
        public ActionResult Scrap()
        {
            return View("~/Views/Data/ApiMng/Scrap.cshtml");
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