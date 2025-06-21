using System;
using System.Collections.Generic;

namespace Golden_votes.Entities;

public class Vote
{
  public int ID { get; set; }
  public string Name { get; set; }

  public DateTime StartPeriod { get; set; }

  public DateTime EndPeriod { get; set; }

  public List<Answer> Answers { get; set; }

  public int AnswersCount
  {
    get
    {
      return Answers.Count;
    }
  }

  public Vote(string name, List<Answer> answers, DateTime end, DateTime? start = null)
  {
    Name = name;
    Answers = answers;
    StartPeriod = start ?? DateTime.Now;
    EndPeriod = end;
  }
}
