using Microsoft.Extensions.Configuration;

namespace Assignment.Common
{
    public static class AppSettings
    {
        public static string BaseUrlV1
        {
            get
            {
                var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
                var baseUrlV1 = config.GetSection("BaseUrls").GetSection("V1").Value;
                return baseUrlV1 ?? string.Empty;
            }
        }
    }
}
