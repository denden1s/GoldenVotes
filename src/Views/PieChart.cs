using System.Collections.Generic;
using Golden_votes.Entities;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Avalonia;
namespace Golden_votes.Views;

public class Chart
{
  private PieChart chart;
  public Chart(ref PieChart chart)
  {
    this.chart = chart;
  }

  public void LoadData(List<Answer> answers)
  {
    var pieSeries = new List<ISeries>();
    foreach (var answer in answers)
    {
      PieSeries<int> series = new PieSeries<int>
    {
      Values = new int[] { answer.Users.Count },
      Name = answer.Name
    };
    pieSeries.Add(series);
    }
    chart.Series = pieSeries;
    chart.CoreChart.Update();
  }
}
