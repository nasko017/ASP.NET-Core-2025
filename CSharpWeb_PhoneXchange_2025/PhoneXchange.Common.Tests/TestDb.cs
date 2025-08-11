using Microsoft.EntityFrameworkCore;
using PhoneXchange.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Common.Tests
{
    public class TestDb
    {
        public static ApplicationDbContext NewContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"PhoneXchange_Tests_{Guid.NewGuid()}")
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
