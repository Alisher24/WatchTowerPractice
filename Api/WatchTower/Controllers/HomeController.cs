using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchTower.Database;

namespace WatchTower.Controllers
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
