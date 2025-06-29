namespace TodoApp.Db
{
    public class DatabaseSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? UserCollection { get; set; }
        public string? TodoCollection { get; set; }
    }
}
