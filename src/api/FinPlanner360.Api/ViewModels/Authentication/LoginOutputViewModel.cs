namespace FinPlanner360.Api.ViewModels.Authentication
{
    public class LoginOutputViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
    }
}
