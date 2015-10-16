using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace PhotoShare.Web.Models
{
    public class PhotoContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UploadedFile> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UploadedFile>().HasKey(file => file.Name);
            modelBuilder.Entity<Photo>()
                .HasKey(photo => photo.Id)
                .HasRequired(photo => photo.File).WithRequiredPrincipal()
                .WillCascadeOnDelete();
            modelBuilder.Entity<Photo>()
                .Property(photo => photo.Timestamp).HasColumnType("datetime");
        }
    }


    public class Photo
    {
        public Guid Id { get; set; }
        public UploadedFile File { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class UploadedFile
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}