using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Verve.Identity.Core.Model;
using Verve.Identity.Core.Service;
using Verve.Identity.Core.Service.Abstraction;
using Verve.Identity.Core.Sample.ApplicationDbContext;
using Verve.Identity.Core.Sample.Entity;
using Verve.Identity.Core.Sample.Web.Service;
using Xunit;

namespace Verve.Identity.Core.Tests.Service
{
    public class ApplicationUserServiceShould
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IVerveRoleStore<VerveRole>> _mockVerveRoleStore;
        private readonly TestApplicationDbContext _mockTestApplicationDbContext;
        private readonly Mock<IdentityErrorDescriber> _mockIdentityErrorDescriber;
        private readonly Mock<ILogger<VerveIdentityService<TestApplicationDbContext, UserAccount, VerveRole>>> _mockLogger;

        public ApplicationUserServiceShould()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockVerveRoleStore = this._mockRepository.Create<IVerveRoleStore<VerveRole>>();
            _mockTestApplicationDbContext = MockDbContext.Create();
            _mockIdentityErrorDescriber = this._mockRepository.Create<IdentityErrorDescriber>();
            _mockLogger = this._mockRepository.Create<ILogger<VerveIdentityService<TestApplicationDbContext, UserAccount, VerveRole>>>();
        }

        private ApplicationUserService CreateService()
        {
            return new ApplicationUserService(
                _mockVerveRoleStore.Object,
                _mockTestApplicationDbContext,
                new IdentityErrorDescriber(), 
                NullLogger<ApplicationUserService>.Instance);
        }

        [Fact]
        public async Task CreateUser_WithUserRole()
        {
            // Arrange
            var service = this.CreateService();
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var userAccount = new UserAccount
            {
                AccessFailedCount = 0,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                EmailConfirmed = false,
                LockoutEnabled = true,
                Name = "Aziz Test",
                NormalizedEmail = "TEST@TEST.COM",
                PhoneNumber = "01234567890",
                NormalizedUserName = "TESTUSER",
                UserName = "testUser",
                Status = 0,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "hashedpawword",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                Id = userId,
            };

            _mockVerveRoleStore.Setup(x =>
                    x.FindByRoleNameAsync("User", It.Is<CancellationToken>(c => c == CancellationToken.None)))
                .ReturnsAsync(new VerveRole
                {
                    Id = roleId,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Description",
                });

            // Act
            var userResult = await service.CreateAsync(userAccount, CancellationToken.None);

            // Assert
            Assert.True(userResult.Succeeded);

            var dbAccount = await service.FindByIdAsync(userId.ToString(), CancellationToken.None);

            Assert.NotNull(dbAccount);

            var dbRole = _mockTestApplicationDbContext.UserRoleMappings.FirstOrDefault(x => x.UserId == userId);

            Assert.NotNull(dbRole);

            Assert.Equal(roleId, dbRole.RoleId);

            _mockRepository.VerifyAll();
        }
    }
}
