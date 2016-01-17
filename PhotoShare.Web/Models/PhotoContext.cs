using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PhotoShare.Web.Models;


namespace PhotoShare.Web.Models
{
    public class PhotoContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UploadedFile> Files { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UploadedFile>().HasKey(file => file.Name);

            modelBuilder.Entity<Photo>().HasRequired(photo => photo.File).WithRequiredPrincipal().WillCascadeOnDelete();
            modelBuilder.Entity<Photo>().Property(photo => photo.Timestamp).HasColumnType("datetime");

            modelBuilder.Entity<UserRating>().HasRequired(rating => rating.Photo).WithMany(p => p.Ratings).WillCascadeOnDelete(true);
            modelBuilder.Entity<UserRating>().HasRequired(rating => rating.User).WithMany().WillCascadeOnDelete(false);

        }
    }
}