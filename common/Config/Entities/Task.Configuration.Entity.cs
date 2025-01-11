
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Entity.Configuration{

    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>{
        public void Configure(EntityTypeBuilder<TaskEntity> builder){
            builder.ToTable("Tasks");
            builder.HasKey(t => new {t.id});//Primary key

            //Property configuration
            builder.Property(t => t.tittle)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(t => t.description)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(t => t.status)
                .HasMaxLength(20)
                .IsRequired()
                .HasConversion(
                    v => v,
                    v => v 
                );

             builder.Property(t => t.updated_at)
                .IsRequired(false);

            builder.Property(t => t.created_at)
                .IsRequired();
            //Relationship task -> user
            builder.HasOne(t => t.user)
                .WithMany()
                .HasForeignKey("userId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}