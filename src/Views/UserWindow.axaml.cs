using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Golden_votes.Entities;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class UserWindow : Window
{
  private Chart pieChart;
  private User user;
  private List<Vote> votes;

  public UserWindow(User user)
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    VotesList.Height = this.Height * 0.8;
    PieStat.Width = this.Width * 0.8;
    PieStat.Height = this.Height * 0.6;
    pieChart = new Chart(ref PieStat);
    this.user = user;
    this.user.LoadAnswers();
    VoteButton.IsEnabled = false;
    votes = ApplicationContext.LoadActualVotes();
    foreach (var vote in votes)
      vote.LoadAnswers();
    LoadVotesInListBox();
    TestAnswers();
    TestAnswers();
    TestAnswers();
    TestAnswers();
    TestAnswers();
    TestAnswers();
    TestAnswers();
  }

  private void TestAnswers()
  {
    var textBox = new RadioButton
    {
      GroupName = "Answer",
      Content = "test",
      Margin = new Thickness(5, 0, 0, 5)
    };
    AnswersPanel.Children.Add(textBox);
  }

  private void LoadVotesInListBox()
  {
    VotesList.Items.Clear();
    foreach (var vote in votes)
    {
      int len = Settings.VoteNameLength < vote.Name.Length ? Settings.VoteNameLength : vote.Name.Length;
      string name = len == Settings.VoteNameLength ? vote.Name.Substring(0, Settings.VoteNameLength) + "..." :
                                            vote.Name;
      VotesList.Items.Add(name);
    }
    VoteButton.IsEnabled = votes.Count > 0 && VotesList.SelectedItem != null;
  }


  private void VotesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
    if (VotesList.SelectedItem is string selected)
    {
      string name = selected.Contains("...") ? selected.Substring(0, Settings.VoteNameLength) : selected;
      var vote = votes.Where(vote => vote.Name.Contains(name)).FirstOrDefault();
      if (vote != null)
      {
        VoteQuestion.Text = vote.Name;
        LoadPieChart(vote.Answers);
        EndDate.Text = $"Дата завершения голосования: {vote.EndPeriod.ToShortDateString()}";
        //VoteButton.IsEnabled = based on activity in this vote
      }
    }
  }

  private void LoadPieChart(List<Answer> answers)
  {
    if (pieChart == null || answers.Count() == 0)
      return;

    pieChart.LoadData(answers);
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
  {
    Environment.Exit(0);
  }
}