namespace TodoApp.Services.Interfaces
{
    public interface IPasswordService
    {
        public string HashedPassword(string password);
        public Boolean VerifyPassowrd(string password, string passwordHash);
    }
}
