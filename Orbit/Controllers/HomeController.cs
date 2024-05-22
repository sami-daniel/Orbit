using Microsoft.AspNetCore.Mvc;

namespace Orbit.Controllers
{
    public class HomeController : Controller
    {
        //Route: {url}/Home/Index OR {url}/Home/
        public IActionResult Index()
        {
            ViewBag.Title = "Bedrock Default Page"; /*Colocando nova propriedade chamada Title<dynamic>
                                                     dentro da ViewBag (usado para title da pagina de _Layout)*/

            return View(); //~/Views/Home/Index.cshtml
        }
    }
}
