using System;
using Microsoft.EntityFrameworkCore;
using Golden_votes.Entities;
using Golden_votes.Utils;


namespace Golden_votes;

public class ApplicationContext : DbContext
{
  private string ip;
  public ApplicationContext()
  {
    DBServer server = new DBServer();
    ip = server.IP;
  }
  public DbSet<User> Users { get; set; }
  public DbSet<Question> Questions { get; set; }

  public DbSet<Vote> Votes { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Server = " + ip + "\\SQLEXPRESS, 1433;" +
        " Database = golden_votes; User ID = root; Password = root; ");
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().HasKey(i => i.Login);
    modelBuilder.Entity<Question>().HasKey(i => i.ID);
    modelBuilder.Entity<Vote>().HasKey(i => i.ID);

    modelBuilder.Entity<Question>().HasMany(question => question.Variants);
    modelBuilder.Entity<Vote>().HasMany(vote => vote.Questions);
    modelBuilder.Entity<Vote>().HasMany(vote => vote.Users);
  }
}
