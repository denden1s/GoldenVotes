using System;
using System.Collections.Generic;
using Golden_votes.Utils;

namespace Golden_votes.Entities;

public class User
{
  private const string id_salt = "golden_votes";
  public enum UserRole
  {
    kBaseUser,
    kAdmin
  };

  public string ID { get; set; }
  public string Login { get; set; }
  public string Password { get; set; }
  public UserRole Role { get; set; }

  public List<Answer> Answers { get; set; }

  public User() { }
  public User(string login, string password, UserRole role = UserRole.kBaseUser)
  {
    Encryption enc = new Encryption();
    ID = enc.Hash(login + id_salt);
    Login = enc.Encrypt(login);
    Password = enc.Hash(login + password);
    Role = role;
  }
}
