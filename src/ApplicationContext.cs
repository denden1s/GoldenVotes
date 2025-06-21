using System;
using Microsoft.EntityFrameworkCore;
using Golden_votes.Entities;
using Golden_votes.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;

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
  public DbSet<Answer> Answers { get; set; }

  public DbSet<Vote> Votes { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Server=" + ip + "\\SQLEXPRESS;" +
        "Database=golden_votes;Trusted_Connection=True;TrustServerCertificate=true;");
    //Server=localhost\SQLEXPRESS01;Database=master;Trusted_Connection=True;
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().HasKey(user => user.ID);
    modelBuilder.Entity<User>().HasMany(user => user.Answers);

    modelBuilder.Entity<Answer>().HasKey(Answer => Answer.ID);
    modelBuilder.Entity<Answer>().HasMany(Answer => Answer.Users);

    modelBuilder.Entity<Vote>().HasKey(vote => vote.ID);

    modelBuilder.Entity<Vote>().HasMany(vote => vote.Answers);

  }

  public static void GenerateContent()
  {
    // TODO: realize generating content like carsharing

    // Users
    List<User> users = new List<User>();
    users.Add(new User("di", "Pass@123"));
    users.Add(new User("ivan_ivanov", "Qwerty123!"));
    users.Add(new User("kris_tin", "pass_word"));
    users.Add(new User("mark", "markelov"));
    users.Add(new User("admin", "admin", User.UserRole.kAdmin));

    //Votes
    List<Answer> answers1 = new List<Answer>();
    answers1.Add(new Answer("C#",
                            new List<User>() { users[0], users[1], users[2] })
               );
    answers1.Add(new Answer("Python", new List<User>() { users[3] }));
    answers1.Add(new Answer("Pascal"));
    Vote vote1 = new Vote("Какой язык программирования вы предпочитаете для бэкенда?",
                          answers1,
                          DateTime.Now.AddDays(45));

    List<Answer> answers2 = new List<Answer>();
    answers2.Add(new Answer("React",
                            new List<User>() { users[0] })
               );
    answers2.Add(new Answer("Angular", new List<User>() { users[3] }));
    answers2.Add(new Answer("Vue", new List<User>() { users[2] }));
    Vote vote2 = new Vote("Какой фреймворк вам нравится для веб-разработки?",
                          answers2,
                          DateTime.Now.AddDays(35));

    List<Vote> votes = new List<Vote> { vote1, vote2 };

    using (ApplicationContext db = new ApplicationContext())
    {
      db.Users.AddRange(users);
      db.Votes.AddRange(votes);
      db.SaveChanges();
    }
  }
  public static void GenerateAdmin()
  {
    using (ApplicationContext db = new ApplicationContext())
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

  public static List<User> LoadUsers(User.UserRole role = User.UserRole.kBaseUser)
  {
    using (ApplicationContext db = new ApplicationContext())
      return db.Users.Where(user => user.Role == role).ToList();
  }

  public static bool DeleteUser(string user_id)
  {
    using (ApplicationContext db = new ApplicationContext())
    {
      User user = db.Users.Where(u => u.ID == user_id).FirstOrDefault();
      if (user == null)
        return false;

      db.Users.Remove(user);
      db.SaveChanges();
    }
    return true;
  } 
 
  public static bool AddUser(User user)
  {
    using (ApplicationContext db = new ApplicationContext())
    {
      if (db.Users.Where(u => u.ID == user.ID).FirstOrDefault() != null)
        return false;

      db.Users.Add(user);
      db.SaveChanges();
    }
    return true;
  }  
}
