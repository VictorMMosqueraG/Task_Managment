using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Entity.Configuration{

    public class UserConfiguration : IEntityTypeConfiguration<User>{
        public void Configure(EntityTypeBuilder<User> builder){
            builder.ToTable("User");
            builder.HasKey(u => new {u.Id});//Primary key

            //Property configuration
             builder.Property(u => u.Name)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(100)
                .HasAnnotation("Relational:Validation", "email") 
                .IsRequired();

            builder.Property(u => u.Password)
                .HasMaxLength(200)
                .IsRequired();

             // Constraint
            builder.HasIndex(e => e.Email)
                .IsUnique()//email is unique
                .HasDatabaseName("IX_Email");

            //Relationship user -> role
              builder.HasOne(u => u.role) 
                .WithMany() 
                .HasForeignKey("roleId") 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}