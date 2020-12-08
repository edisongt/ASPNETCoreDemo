﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ASPNETCore5Demo.Models
{
    /// <summary>
    /// 透過 DB First 流程建立 EF Core 實體資料模型
    /// </summary>
    public partial class ContosoUniversityContext : DbContext
    {
        public ContosoUniversityContext()
        {
        }

        public ContosoUniversityContext(DbContextOptions<ContosoUniversityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructor { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<OfficeAssignment> OfficeAssignment { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<VwCourseStudentCount> VwCourseStudentCount { get; set; }
        public virtual DbSet<VwCourseStudents> VwCourseStudents { get; set; }
        public virtual DbSet<VwDepartmentCourseCount> VwDepartmentCourseCount { get; set; }

        public override int SaveChanges()
        {
            var entities = this.ChangeTracker.Entries();
            foreach (var entry in entities)
            {
                Console.WriteLine("Entity Name: {0}", entry.Entity.GetType().FullName);
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var entities = this.ChangeTracker.Entries();
            foreach (var entry in entities)
            {
                Console.WriteLine("Entity Name: {0}", entry.Entity.GetType().FullName);
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.DepartmentId, "IX_DepartmentID");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("DepartmentID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_dbo.Course_dbo.Department_DepartmentID");
            });

            modelBuilder.Entity<CourseInstructor>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.InstructorId })
                    .HasName("PK_dbo.CourseInstructor");

                entity.HasIndex(e => e.CourseId, "IX_CourseID");

                entity.HasIndex(e => e.InstructorId, "IX_InstructorID");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseInstructor)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_dbo.CourseInstructor_dbo.Course_CourseID");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.CourseInstructor)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_dbo.CourseInstructor_dbo.Instructor_InstructorID");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.InstructorId, "IX_InstructorID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Budget).HasColumnType("money");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Department)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_dbo.Department_dbo.Instructor_InstructorID");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(e => e.CourseId, "IX_CourseID");

                entity.HasIndex(e => e.StudentId, "IX_StudentID");

                entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_dbo.Enrollment_dbo.Course_CourseID");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_dbo.Enrollment_dbo.Person_StudentID");
            });

            modelBuilder.Entity<OfficeAssignment>(entity =>
            {
                entity.HasKey(e => e.InstructorId)
                    .HasName("PK_dbo.OfficeAssignment");

                entity.HasIndex(e => e.InstructorId, "IX_InstructorID");

                entity.Property(e => e.InstructorId)
                    .ValueGeneratedNever()
                    .HasColumnName("InstructorID");

                entity.Property(e => e.Location).HasMaxLength(50);

                entity.HasOne(d => d.Instructor)
                    .WithOne(p => p.OfficeAssignment)
                    .HasForeignKey<OfficeAssignment>(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.OfficeAssignment_dbo.Instructor_InstructorID");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Discriminator)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('Instructor')");

                entity.Property(e => e.EnrollmentDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VwCourseStudentCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCourseStudentCount");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<VwCourseStudents>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCourseStudents");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.CourseTitle).HasMaxLength(50);

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentName).HasMaxLength(101);
            });

            modelBuilder.Entity<VwDepartmentCourseCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwDepartmentCourseCount");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}