using System.Collections.Generic;

using Golden_votes.Utils;

namespace Golden_votes.Entities;

public class User
{
  private const string _kIDSalt = "golden_votes";
  private List<Vote> _passedVotes;
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
  public User(string login)
  {
    Encryption enc = new Encryption();
    ID = enc.Hash(login + _kIDSalt);
  }

  public User(string login, string password, UserRole role = UserRole.kBaseUser)
  {
    Encryption enc = new Encryption();
    _passedVotes = new List<Vote>();
    ID = enc.Hash(login + _kIDSalt);
    Login = enc.Encrypt(login);
    Password = enc.Hash(login + password);
    Role = role;
  }
  public void LoadAnswers()
  {
    Answers = ApplicationContext.LoadAnswers(this);
    foreach (var answer in Answers)
      _passedVotes.AddRange(ApplicationContext.LoadActualVotes(answer));
  }
}
