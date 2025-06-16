using System;
using Microsoft.EntityFrameworkCore;
using Golden_votes.Entities;
using Golden_votes.Utils;
using Microsoft.IdentityModel.Tokens;


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
        " Database = golden_votes; User ID = sa; Password = Qwerty123; TrustServerCertificate=true;");
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().HasKey(user => user.ID);
    modelBuilder.Entity<User>().HasMany(user => user.Questions);

    modelBuilder.Entity<Question>().HasKey(question => question.ID);
    modelBuilder.Entity<Question>().HasMany(question => question.Users);

    modelBuilder.Entity<Vote>().HasKey(vote => vote.ID);

    modelBuilder.Entity<Vote>().HasMany(vote => vote.Questions);

  }
  public static void GenerateAdmin()
  {
    using(ApplicationContext db = new ApplicationContext())
    {
      // TODO: delete in release
      db.Database.EnsureDeleted();

      db.Database.EnsureCreated();
      if (db.Users.IsNullOrEmpty())
      {
        db.Users.Add(new User("root", "root", User.UserRole.kAdmin));
        db.SaveChanges();
      }
    }
  }
}
