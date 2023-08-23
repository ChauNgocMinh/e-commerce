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

            entity.Property(e => e.Namebook)
                .HasMaxLength(50)
                .HasColumnName("NAMEBOOK");

            entity.Property(e => e.Picture).HasColumnName("PICTURE");

            entity.Property(e => e.Price).HasColumnName("SALES");
            entity.Property(e => e.Price).HasColumnName("PRICE");

            entity.Property(e => e.PublishingCompany)
                .HasMaxLength(50)
                .HasColumnName("PUBLISHING_COMPANY");

            entity.Property(e => e.Review).HasColumnName("REVIEW");

            entity.Property(e => e.Status).HasColumnName("STATUS");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Email })
                .HasName("PK_CART_BUY");

            entity.ToTable("BILL");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("ADDRESS");

            entity.Property(e => e.BuyingDate)
                .HasColumnType("datetime")
                .HasColumnName("BUYING_DATE");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");

            entity.Property(e => e.Note).HasColumnName("NOTE");

            entity.Property(e => e.Pay).HasColumnName("PAY");

            entity.Property(e => e.PaymentMethods)
                .HasMaxLength(20)
                .HasColumnName("PAYMENT_METHODS");

            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("PHONE");

            entity.Property(e => e.Status).HasColumnName("STATUS");

            entity.Property(e => e.Total).HasColumnName("TOTAL");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Email })
                .HasName("PK_CART_ITEM_1");

            entity.ToTable("CART_ITEM");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL");

            entity.Property(e => e.IdBill)
                .HasMaxLength(10)
                .HasColumnName("ID_BILL");

            entity.Property(e => e.IdBook)
                .HasMaxLength(10)
                .HasColumnName("ID_BOOK");

            entity.Property(e => e.Number).HasColumnName("NUMBER");

            entity.Property(e => e.Status).HasColumnName("STATUS");

            entity.HasOne(d => d.IdBookNavigation)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.IdBook)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CART_ITEM_BOOK");

            entity.HasOne(d => d.Bill)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => new { d.IdBill, d.Email })
                .HasConstraintName("FK_CART_ITEM_BILL");
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
    public DbSet<BillModel>? BillModel { get; set; }

}
