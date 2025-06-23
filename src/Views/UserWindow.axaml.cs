using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Golden_votes.Entities;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class UserWindow : Window
{
  private Chart pieChart;
  private Vote selected_vote;
  private User user;
  private List<Vote> votes;
  private List<RadioButton> radio_answers;


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
    selected_vote = new Vote();
    radio_answers = new List<RadioButton>();
    foreach (var vote in votes)
      vote.LoadAnswers();
    LoadVotesInListBox();
  }

  private void LoadRadioButtons(List<Answer> answers)
  {
    AnswersPanel.Children.Clear();
    radio_answers.Clear();
    bool isVoted = user.Answers.Where(answer => answer.VoteID == answers.First().VoteID).Count() != 0;
    foreach (var answer in answers)
    {
      var radioButton = new RadioButton
      {
        GroupName = "Answer",
        Content = answer.Name,
        Margin = new Thickness(5, 0, 0, 5)
      };
      if (user.Answers.Where(ans => ans.Name == answer.Name && ans.VoteID == answer.VoteID).Count() != 0)
        radioButton.IsChecked = true;

      AnswersPanel.Children.Add(radioButton);
      radio_answers.Add(radioButton);
    }
    AnswersPanel.IsEnabled = VoteButton.IsEnabled = !isVoted;
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
        selected_vote = vote;
        VoteQuestion.Text = vote.Name;
        LoadPieChart(vote.Answers);
        LoadRadioButtons(vote.Answers);
        EndDate.Text = $"Дата завершения голосования: {vote.EndPeriod.ToShortDateString()}";
      }
    }
  }

  private async void OnVoteButtonClick(object? sender, RoutedEventArgs e)
  {
    string answer_name = radio_answers.Where(rb => rb.IsChecked == true).FirstOrDefault().Content.ToString();
    var result = await VariantMessageBox.Show(this, "Предупреждение",
      $"Вы действительно хотите проголосовать за ответ {answer_name}");
    if (result == false)
    {
      InfoMessageBox.Show(this, "Информация", "Голос отменен");
      return;
    }
    Answer answer = selected_vote.Answers.Where(answer => answer.Name == answer_name).FirstOrDefault();
    ApplicationContext.UpdateAnswer(answer, user);
    answer.Users.Add(user);
    user.LoadAnswers();
    AnswersPanel.IsEnabled = false;
    LoadPieChart(selected_vote.Answers);
    VoteButton.IsEnabled = false;
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