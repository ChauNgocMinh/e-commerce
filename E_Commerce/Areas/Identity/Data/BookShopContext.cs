using E_Commerce.Areas.Identity.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Identity.Data;

public partial class BookShopContext : IdentityDbContext<ApplicationUser>
{
    public BookShopContext()
    {
    }

    public BookShopContext(DbContextOptions<BookShopContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Bill> Bills { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-V6B0S12C\\SQLEXPRESS;Initial Catalog=BookShop;Persist Security Info=True;User ID=sa;Password=sa;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("BOOK");
            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Category)
            .HasMaxLength(50)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.Picture)
                .HasMaxLength(4000)
                .HasColumnName("PICTURE");
            entity.Property(e => e.PublishingCompany)
                .HasMaxLength(50)
                .HasColumnName("PUBLISHING_COMPANY");
            entity.Property(e => e.Namebook)
                .HasMaxLength(50)
                .HasColumnName("NAMEBOOK");
            entity.Property(e => e.Price).HasColumnName("PRICE");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Review)
                .HasMaxLength(4000)
                .HasColumnName("REVIEW");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Email });

            entity.ToTable("BILL");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL");
            entity.Property(e => e.BuyingDate)
                .HasColumnType("datetime")
                .HasColumnName("BUYING_DATE");
            entity.Property(e => e.IdCartItem)
                .HasMaxLength(10)
                .HasColumnName("ID_CART_ITEM");

        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Email }).HasName("PK_CART_ITEM_1");

            entity.ToTable("CART_ITEM");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.Number)
               .HasColumnType("int")
               .HasColumnName("NUMBER");
            entity.Property(e => e.IdBook)
                .HasMaxLength(10)
                .HasColumnName("ID_BOOK");
            entity.Property(e => e.Status).HasColumnName("STATUS");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.IdBook)
                .HasConstraintName("FK_CART_ITEM_BOOK");
        });

        SeedRoles(modelBuilder);
        OnModelCreatingPartial(modelBuilder);

    }


    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData
            (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
            );

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public DbSet<BookModel>? BookModel { get; set; }
    public DbSet<CartItemModel>? CartItemModel { get; set; }

}
