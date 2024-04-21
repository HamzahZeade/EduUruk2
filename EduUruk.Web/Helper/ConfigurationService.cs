using EduUruk.DAL.Helper;

namespace EduUruk.Web.Helper
{
    public class ConfigurationService : IConfigurationService
    {
        public void SetConnectionString(string connectionString)
        {
            Constants.SetConnectionString(connectionString);
        }
    }

}
