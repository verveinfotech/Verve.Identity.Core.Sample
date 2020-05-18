using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using Verve.Identity.Core.Model;
using Verve.Identity.Core.Sample.ApplicationDbContext;

namespace Verve.Identity.Core.Tests
{
    public class MockDbContext 
    {
        public static TestApplicationDbContext Create()
        {
            var builder = new DbContextOptionsBuilder<TestApplicationDbContext>().UseInMemoryDatabase("testDb");
            return new TestApplicationDbContext(builder.Options);
        }

        public static void SeedRoles(TestApplicationDbContext mockDbContext)
        {
            mockDbContext.Roles.Add(new VerveRole
            {
                Id = StaticGuids.One,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator"
            });

            mockDbContext.Roles.Add(new VerveRole
            {
                Id = StaticGuids.Two,
                Name = "User",
                NormalizedName = "USER",
                Description = "User"
            });

            mockDbContext.SaveChanges();
        }
    }
}