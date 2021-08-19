namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IAuthorization
    {
        bool Readonly { get; set; }
        bool IsAdmin { get; set; }
    }
}
