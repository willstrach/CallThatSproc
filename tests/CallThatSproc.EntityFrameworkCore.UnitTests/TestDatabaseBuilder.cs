using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CallThatSproc.EntityFrameworkCore.UnitTests
{
    internal static class TestDatabaseBuilder
    {
        internal class Entity
        {
            public int Id { get; set; }
        }

        internal class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
            public DbSet<Entity> DbSet { get; set; }
        }

        // This is a workaround to get a database context without a real database
        // Future updates to EF Core may break this
        internal static TestDbContext CreateDbContext()
        {
            // To use the sql extension methods, we need a relational provider
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TestDbContext>().UseSqlite();
            return new TestDbContext(dbContextOptionsBuilder.Options);
        }

        internal static DatabaseFacade CreateDatabaseFacade() => new DatabaseFacade(CreateDbContext());
        internal static DbSet<Entity> CreateDbSet() => CreateDbContext().DbSet;
    }
}