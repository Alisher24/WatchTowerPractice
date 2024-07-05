using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchTower.Server.BusinessLogic;

namespace WatchTower.Server.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private WatchTowerDbContext _db;
        
        public HomeController(WatchTowerDbContext context)
        {
            _db = context;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*public ActionResult Login()
        {

        }

        public ActionResult Logout()
        {

        }*/
    }
}
