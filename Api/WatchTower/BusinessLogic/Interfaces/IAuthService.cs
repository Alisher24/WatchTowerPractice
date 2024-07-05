using WatchTower.Server.Models;

namespace WatchTower.Server.BusinessLogic.Interface
{
    public interface IAuthService
    {
        public Task<User> Login(string name, string password);
        public Task<User> Register(User user);
    }
}
