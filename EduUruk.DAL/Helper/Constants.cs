//using Microsoft.Extensions.Configuration;

//namespace EduUruk.DAL.Helper
//{
//    public static class Constants
//    {


//        public static IConfiguration Configuration { get; set; }

//        //public const string ApiSecretKey = "v9y$B&E)"; // todo: get this from somewhere secure
//        //private const string SecretKey = "1ZDQ4NjA3My04OGMxLTRlMjEtYjczYi1hYjVjMGFiZDZmODgiLCJpYXQiOjE1MjY2ODMzOTksInJvbCI6ImFwaV9hY2Nlc3MiLC"; // todo: get this from somewhere secure
//        //public static readonly SymmetricSecurityKey _signingKey = new(Encoding.ASCII.GetBytes(SecretKey));

//        public static string GetConnectionString() => Configuration.GetConnectionString("DefaultConnection");
//        //public static string GetLiteConnectionString() => Configuration.GetConnectionString("LiteConnection");
//        //public static string GetStorageConnectionString() => Configuration.GetConnectionString("AzureDbStorage");
//    }
//}
namespace EduUruk.DAL.Helper;
public static class Constants
{
    private static string _connectionString;

    public static void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static string GetConnectionString() => _connectionString;
}