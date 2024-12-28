namespace FinPlanner360.Api.ViewModels.User;

public class LoginOutputViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string AccessToken { get; set; }
}