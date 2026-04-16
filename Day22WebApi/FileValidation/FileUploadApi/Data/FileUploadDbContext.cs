using FileUploadApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApi.Data
{
    public class FileUploadDbContext : DbContext
    {
        public FileUploadDbContext(DbContextOptions<FileUploadDbContext> options)
            : base(options)
        {
        }

        public DbSet<BestPractice> BestPractices { get; set; }
        public DbSet<LessonsLearned> LessonsLearned { get; set; }
        public DbSet<MeetingNotes> MeetingNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BestPractice>()
                .Property(x => x.FileName)
                .IsRequired();

            modelBuilder.Entity<BestPractice>()
                .Property(x => x.FilePath)
                .IsRequired();

            modelBuilder.Entity<LessonsLearned>()
                .Property(x => x.FileName)
                .IsRequired();

            modelBuilder.Entity<LessonsLearned>()
                .Property(x => x.FilePath)
                .IsRequired();

            modelBuilder.Entity<MeetingNotes>()
                .Property(x => x.FileName)
                .IsRequired();

            modelBuilder.Entity<MeetingNotes>()
                .Property(x => x.FilePath)
                .IsRequired();
        }
    }
}