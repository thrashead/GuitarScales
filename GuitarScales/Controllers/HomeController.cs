using System.Web.Mvc;

namespace GuitarScales.Controllers
{
	public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Session["giris"] = "yonet";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
