using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests;

public class DatabaseContext : DbContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<Animal>(new AnimalConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Animal> Animals { get; set; }
}

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int NumberOfLegs { get; set; }
}

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.Property(animal => animal.Id).HasColumnName("ID");
        builder.Property(animal => animal.Name).HasColumnName("AnimalName");
        builder.Property(animal => animal.NumberOfLegs).HasColumnName("NumberOfLegs");
        builder.HasKey(x => x.Id);
    }
}
