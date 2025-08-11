namespace PhoneXchange.Common.Tests
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using PhoneXchange.Data.Models;

    public static class UserManagerMock
    {
        public static Mock<UserManager<ApplicationUser>> Create()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mock.SetupAllProperties();
            return mock;
        }
    }
}
