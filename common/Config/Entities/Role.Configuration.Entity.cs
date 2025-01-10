using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Entity.Configuration{

    public class RoleConfiguration : IEntityTypeConfiguration<Role>{
        public void Configure(EntityTypeBuilder<Role> builder){
            builder.ToTable("Role");
            builder.HasKey(e => e.Id);//Primary key

            //Property configuration
            builder.Property(e => e.Name)//Name
                .HasMaxLength(40)
                .IsRequired();
            builder.Property(e => e.Description)
                .HasMaxLength(200);
            
            //Constrains
            builder.HasIndex(e => e.Name)
                .IsUnique()//Name is unique
                .HasDatabaseName("IX_ROLE_NAME");

            //Relationship Configuration: Many to Many
            builder.HasMany(r => r.Permissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}