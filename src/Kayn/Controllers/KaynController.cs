using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kayn.Controllers
{
    public class KaynController : Controller
    {
        protected string Theme => Request.Cookies["site_theme"] ?? "dark";
        
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewData["site_theme"] = Theme;
            base.OnActionExecuted(context);
        }
    }
}