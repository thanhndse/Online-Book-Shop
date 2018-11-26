using System.Web.Mvc;

namespace BookShopWithAuthen.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorHandler
        public ActionResult Index()
        {
            return View("Error");
        }
        public ActionResult NotFound()
        {
            ViewBag.errorMessage = "404 - Page not found";
            ViewBag.errorDetail = "The page you are looking for might have been removed had its name changed or is temporarily unavailable.";
            return View("Error");
        }

        public ActionResult BadRequest()
        {
            ViewBag.errorMessage = "400 - Bad request";
            ViewBag.errorDetail = "Your client has issued a malformed or illegal request";
            return View("Error");
        }
    }
}