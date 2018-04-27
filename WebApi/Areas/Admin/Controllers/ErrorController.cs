using System.Web.Mvc;

namespace WebApi.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Admin/Error
        public ActionResult Index()
        {
            return View();
        }
    }
}