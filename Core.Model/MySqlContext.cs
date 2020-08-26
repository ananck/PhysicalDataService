using Microsoft.EntityFrameworkCore;

namespace Core.Model
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Trade> Trade { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("manufacturer");


                entity.Property(e => e.EnglishName)
                    .HasColumnName("englishName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ShortName)
            .HasColumnName("shortName")
            .HasMaxLength(100)
            .IsUnicode(false);
            });

            modelBuilder.Entity<Trade>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("trade");

                entity.Property(e => e.MaufactureId).HasColumnName("maufactureId");

                entity.Property(e => e.Kind)
                  .HasColumnName("kind")
                  .HasMaxLength(100)
                  .IsUnicode(false);
                entity.Property(e => e.Mark)
          .HasColumnName("mark")
          .HasMaxLength(100)
          .IsUnicode(false);
                entity.Property(e => e.TradeName)
          .HasColumnName("tradeName")
          .HasMaxLength(100)
          .IsUnicode(false);
                entity.Property(e => e.MaterialIdentity)
      .HasColumnName("materialIdentity")
      .HasMaxLength(100)
      .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}