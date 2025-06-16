using System;
using System.Collections.Generic;

namespace Golden_votes.Entities;

public class Question
{
  public int ID { get; set; }

  public string Name { get; set; }

  public List<User> Users { get; set; }
}
