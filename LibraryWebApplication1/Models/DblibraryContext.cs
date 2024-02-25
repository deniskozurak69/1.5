using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication1.Models;

public partial class DblibraryContext : DbContext
{
    public DblibraryContext()
    {
    }

    public DblibraryContext(DbContextOptions<DblibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<SearchRequest> SearchRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-11NQTPR\\SQLEXPRESS; Database=DBLibrary; Trusted_Connection=True;  Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Article");

            entity.Property(e => e.ArticleId)
                .ValueGeneratedNever()
                .HasColumnName("articleId");
            entity.Property(e => e.ArticleName)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("articleName");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.AuthorUsername)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("authorUsername");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("category");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.PublishDate).HasColumnName("publishDate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("status");

            entity.HasOne(d => d.Author).WithMany(p => p.Articles)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Article_User");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_Category");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("categoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId)
                .ValueGeneratedNever()
                .HasColumnName("commentId");
            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.AuthorUsername)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("authorUsername");
            entity.Property(e => e.PublishDate).HasColumnName("publishDate");

            entity.HasOne(d => d.Article).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_Comment_Article");
        });

        modelBuilder.Entity<SearchRequest>(entity =>
        {
            entity.ToTable("SearchRequest");

            entity.Property(e => e.SearchRequestId)
                .ValueGeneratedNever()
                .HasColumnName("searchRequestId");
            entity.Property(e => e.RequestedAuthor)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("requestedAuthor");
            entity.Property(e => e.RequestedCategory)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("requestedCategory");
            entity.Property(e => e.RequestedDate).HasColumnName("requestedDate");
            entity.Property(e => e.RequestedName)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("requestedName");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.SearchRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_SearchRequest_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
