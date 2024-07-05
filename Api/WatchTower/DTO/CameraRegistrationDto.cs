namespace WatchTower.Server.DTO
{
    public class CameraRegistrationDto
    {
        public required string Ip {  get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required int UserId { get; set;}
    }
}
