using System;
using System.Collections.Generic;

namespace Golden_votes.Entities;

public class Vote
{
  public int ID { get; set; }
  public string Name { get; set; }
  
  public DateTime StartPeriod { get; set;}

  public DateTime EndPeriod { get; set;}

  public List<Question> Questions { get; set; }

  public int QuestionsCount
  {
    get
    {
      return Questions.Count;
    }
  }

}
