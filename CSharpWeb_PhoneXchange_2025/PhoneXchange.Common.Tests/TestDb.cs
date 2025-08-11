using System;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Web.Data;

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
