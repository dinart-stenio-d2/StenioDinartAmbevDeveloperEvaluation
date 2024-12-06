using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(s => s.Id);
           
            entity.Property(s => s.Id)
            .ValueGeneratedNever();

            entity.Property(s => s.SaleNumber)
                .HasMaxLength(10); 

            entity.Property(s => s.SaleDate)
                .IsRequired(); 

            entity.Property(s => s.Customer)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(s => s.Branch)
               .HasMaxLength(50);

            entity.Property(s => s.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 

            entity.Property(s => s.IsCancelled)
                .IsRequired();
        });


        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(si => si.Id); 
            
            entity.Property(s => s.Id)
           .ValueGeneratedNever();

            entity.Property(si => si.Product)
                .IsRequired()
                .HasMaxLength(100); 

            entity.Property(si => si.Quantity)
                .IsRequired(); 

            entity.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 

            entity.Property(si => si.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 

            entity.Property(si => si.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 

            entity.HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

    
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.WebApi")
        );

        builder.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM"));



        return new DefaultContext(builder.Options);
    }
}