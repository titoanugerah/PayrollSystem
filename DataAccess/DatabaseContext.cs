using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Payroll.DataAccess
{
    public class DatabaseContext:DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerFactory logger;
        public DatabaseContext(IConfiguration _configuration, ILoggerFactory _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }
    }

}
