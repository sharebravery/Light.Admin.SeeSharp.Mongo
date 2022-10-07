namespace Light.Admin.Mongo.IServices
{
    public interface IAccountService
    {
        public Task<User> ValidateUser(string account, string password);

        public Task<string> GetToken(User user);
    }
}
