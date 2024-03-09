using Microsoft.AspNetCore.Mvc;

namespace Commercio.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Internal()
        {
            return View();

        }
    }
}
