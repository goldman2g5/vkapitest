namespace vkapitest.Services
{
    public class UserService : IUserService
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("root") && password.Equals("root");
        }
    }
}
