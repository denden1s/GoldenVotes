using System;

namespace Golden_votes.Entities;

public class User
{
  public enum UserRole
  {
    kBaseUser,
    kAdmin
  };
  public string Login { get; set; }
  public string Password { get; set; }
  public UserRole Role { get; set; }

  public User(string login, string password, UserRole role = UserRole.kBaseUser)
  {
    Login = login;
    Password = password;
    Role = role;
  }
}
