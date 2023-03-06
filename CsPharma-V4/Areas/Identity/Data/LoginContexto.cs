using CsPharma_V4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsPharma_V4.Areas.Identity.Data;

public class LoginContexto : IdentityDbContext<User>
{
    
    //Creamos un DBSet para poder llamar a la tabla User
    public virtual DbSet<User> UserSet { get; set; }

    public LoginContexto(DbContextOptions<LoginContexto> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Añadimos una nueva configuración
        builder.ApplyConfiguration(new UserEntityConfiguration());

        //Añadimos un nuevo esquema
        builder.HasDefaultSchema("dlk_torrecontrol");

        //Cambiamos el nombre de las tablas
        builder.Entity<User>().ToTable("Dlk_cat_acc_empleados");
        builder.Entity<IdentityRole>().ToTable("Dlk_cat_acc_roles");

        
        builder.Entity<IdentityUserRole<string>>().ToTable("Dlk_cat_acc_empleados_roles");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("Dlk_cat_acc_claim_roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("Dlk_cat_acc_claim_empleados");
        builder.Entity<IdentityUserLogin<string>>().ToTable("Dlk_cat_acc_login_empleados");
        builder.Entity<IdentityUserToken<string>>().ToTable("Dlk_cat_acc_token_empleados");
    }
}

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //Aádimos nuevos campos
        builder.Property(usuario => usuario.NombreUsuario).HasMaxLength(255);
        builder.Property(usuario => usuario.ApellidosUsuario).HasMaxLength(255);
        builder.Property(usuario => usuario.Email).HasMaxLength(255);
    }
}