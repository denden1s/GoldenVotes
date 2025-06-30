using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using System.Collections.Generic;

using Golden_votes.Entities;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class UserWindow : Window
{
  private Chart _pieChart;
  private Vote _selectedVote;
  private User _user;
  private List<Vote> _votes;
  private List<RadioButton> _radioAnswers;

  public UserWindow(User user)
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    VotesList.Height = Height * 0.8;
    PieStat.Width = Width * 0.8;
    PieStat.Height = Height * 0.6;
    _pieChart = new Chart(ref PieStat);
    _user = user;
    _user.LoadAnswers();
    VoteButton.IsEnabled = false;
    _votes = ApplicationContext.LoadActualVotes();
    _selectedVote = new Vote();
    _radioAnswers = new List<RadioButton>();
    foreach (var vote in _votes)
      vote.LoadAnswers();

    LoadVotesInListBox();
  }

  private void LoadRadioButtons(List<Answer> answers)
  {
    AnswersPanel.Children.Clear();
    _radioAnswers.Clear();
    bool isVoted = _user.Answers.Where(answer => answer.VoteID == answers.First().VoteID).Any();
    foreach (var answer in answers)
    {
      var radioButton = new RadioButton
      {
        GroupName = "Answer",
        Content = answer.Name,
        Margin = new Thickness(5, 0, 0, 5)
      };
      if (_user.Answers.Where(ans => ans.Name == answer.Name && ans.VoteID == answer.VoteID).Any())
        radioButton.IsChecked = true;

      AnswersPanel.Children.Add(radioButton);
      _radioAnswers.Add(radioButton);
    }
    AnswersPanel.IsEnabled = VoteButton.IsEnabled = !isVoted;
  }

  private void LoadVotesInListBox()
  {
    int len;
    string name;
    VotesList.Items.Clear();
    foreach (var vote in _votes)
    {
      len = Settings.kVoteNameLength < vote.Name.Length ? Settings.kVoteNameLength : vote.Name.Length;
      name = len == Settings.kVoteNameLength ? vote.Name.Substring(0, Settings.kVoteNameLength) + "..." :
                                               vote.Name;
      VotesList.Items.Add(name);
    }
    VoteButton.IsEnabled = _votes.Any() && VotesList.SelectedItem != null;
  }

  private void VotesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
    string name;
    if (VotesList.SelectedItem is string selected)
    {
      name = selected.Contains("...") ? selected.Substring(0, Settings.kVoteNameLength) : selected;
      var vote = _votes.Where(vote => vote.Name.Contains(name)).FirstOrDefault();
      if (vote != null)
      {
        _selectedVote = vote;
        VoteQuestion.Text = vote.Name;
        LoadPieChart(vote.Answers);
        LoadRadioButtons(vote.Answers);
        EndDate.Text = $"Дата завершения голосования: {vote.EndPeriod.ToShortDateString()}";
      }
    }
  }

  private async void OnVoteButtonClick(object? sender, RoutedEventArgs e)
  {
    Answer answer;
    string answer_name = _radioAnswers.Where(rb => rb.IsChecked == true).FirstOrDefault().Content.ToString();
    var result = await VariantMessageBox.Show(this, "Предупреждение",
      $"Вы действительно хотите проголосовать за ответ {answer_name}");
    if (!result)
    {
      InfoMessageBox.Show(this, "Информация", "Голос отменен");
      return;
    }
    answer = _selectedVote.Answers.Where(answer => answer.Name == answer_name).FirstOrDefault();
    ApplicationContext.UpdateAnswer(answer, _user);
    answer.Users.Add(_user);
    _user.LoadAnswers();
    AnswersPanel.IsEnabled = false;
    LoadPieChart(_selectedVote.Answers);
    VoteButton.IsEnabled = false;
  }

  private void LoadPieChart(List<Answer> answers)
  {
    if (_pieChart == null || answers.Count() == 0)
      return;

    _pieChart.LoadData(answers);
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e) =>  Environment.Exit(0);
}
