using System;
using System.Collections.Generic;

namespace Golden_votes.Entities;

public class Answer
{
  public int ID { get; set; }

  public string Name { get; set; }

  public List<User> Users { get; set; }

  public Answer(string name, List<User>? users = null)
  {
    Name = name;
    if (users != null)
    {
      Users = users;
    }
  }
}
