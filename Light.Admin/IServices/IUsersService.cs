using Light.Admin.CSharp.Models;

namespace Light.Admin.IServices
{
    public interface IUsersService
    {
        Task<User[]> FindAsync();
    }
}
