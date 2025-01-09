using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Entity.Configuration{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>{
        public void Configure(EntityTypeBuilder<Permission> builder){
            //Permission
            builder.ToTable("Permissions");
            builder.HasKey(e => e.Id); //Primary key

            //Property configuration
            builder.Property(e => e.Name)// Name
                .HasMaxLength(40)
                .IsRequired(); 
            builder.Property(e => e.Description)// Description
                .HasMaxLength(200);

            // Constraint
            builder.HasIndex(e => e.Name)
                .IsUnique()//name is unique
                .HasDatabaseName("IX_Permission_Name");
        }
    }
}