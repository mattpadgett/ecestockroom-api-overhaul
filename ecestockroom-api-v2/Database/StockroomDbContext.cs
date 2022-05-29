using ecestockroom_api_v2.Contracts.Configuration;
using ecestockroom_api_v2.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Database;

public partial class StockroomDbContext : DbContext
{
    private readonly IPostgresDbSettings _postgresDbSettings;

    public StockroomDbContext(IPostgresDbSettings postgresDbSettings)
    {
        _postgresDbSettings = postgresDbSettings;
    }

    public StockroomDbContext(DbContextOptions<StockroomDbContext> options, IPostgresDbSettings postgresDbSettings)
        : base(options)
    {
        _postgresDbSettings = postgresDbSettings;
    }

    public virtual DbSet<Classification> Classifications { get; set; } = null!;
    public virtual DbSet<Major> Majors { get; set; } = null!;
    public virtual DbSet<Permission> Permissions { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Status> Statuses { get; set; } = null!;
    public virtual DbSet<TokenFamily> TokenFamilies { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_postgresDbSettings.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classification>(entity =>
        {
            entity.ToTable("classification", "core");

            entity.HasIndex(e => e.Name, "classification_name_key")
                .IsUnique();

            entity.Property(e => e.ClassificationId)
                .HasColumnName("classification_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.Classifications)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classification_status_id_fkey");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.ToTable("major", "core");

            entity.HasIndex(e => e.Name, "major_name_key")
                .IsUnique();

            entity.Property(e => e.MajorId)
                .HasColumnName("major_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.Majors)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("major_status_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permission", "security");

            entity.HasIndex(e => e.Key, "permission_key_key")
                .IsUnique();

            entity.HasIndex(e => e.Name, "permission_name_key")
                .IsUnique();

            entity.Property(e => e.PermissionId)
                .HasColumnName("permission_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");

            entity.Property(e => e.Key)
                .HasMaxLength(50)
                .HasColumnName("key");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role", "security");

            entity.HasIndex(e => e.Name, "role_name_key")
                .IsUnique();

            entity.Property(e => e.RoleId)
                .HasColumnName("role_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.DefaultFlag).HasColumnName("default_flag");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.PermissionIds).HasColumnName("permission_ids");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("status", "core");

            entity.HasIndex(e => e.Name, "status_name_key")
                .IsUnique();

            entity.Property(e => e.StatusId)
                .HasColumnName("status_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TokenFamily>(entity =>
        {
            entity.ToTable("token_family", "security");

            entity.Property(e => e.TokenFamilyId)
                .HasColumnName("token_family_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.AuthorizationToken)
                .HasMaxLength(8192)
                .HasColumnName("authorization_token");

            entity.Property(e => e.CreationUtc)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_utc");

            entity.Property(e => e.RefreshToken)
                .HasMaxLength(4096)
                .HasColumnName("refresh_token");

            entity.Property(e => e.CreationReason)
                .HasMaxLength(100)
                .HasColumnName("creation_reason");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.ValidFlag).HasColumnName("valid_flag");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user", "core");

            entity.HasIndex(e => e.TechId, "user_tech_id_key")
                .IsUnique();

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.ClassificationId).HasColumnName("classification_id");

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(320)
                .HasColumnName("email_address");

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");

            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");

            entity.Property(e => e.MajorId).HasColumnName("major_id");

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(1024)
                .HasColumnName("password_hash");

            entity.Property(e => e.RoleIds).HasColumnName("role_ids");

            entity.Property(e => e.PermissionIds).HasColumnName("permission_ids");

            entity.Property(e => e.PreferredName)
                .HasMaxLength(100)
                .HasColumnName("preferred_name");

            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.Property(e => e.TechId)
                .HasMaxLength(8)
                .HasColumnName("tech_id");

            entity.HasOne(d => d.Classification)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.ClassificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_classification_id_fkey");

            entity.HasOne(d => d.Major)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_major_id_fkey");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_status_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}