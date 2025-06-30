using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Linq;
using System.Collections.Generic;

using Golden_votes.Entities;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class VoteCreateWindow : Window
{
  private AdminWindow _parentWindow;
  private int _answersCount = 1;
  public VoteCreateWindow(AdminWindow win)
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    _parentWindow = win;
  }
  private void CloseWindow()
  {
    _parentWindow.Show();
    this.Hide();
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e) => CloseWindow();

  private async void OnSubmitVote(object? sender, RoutedEventArgs e)
  {
    List<Answer> answers;
    Vote vote;
    if (AnswersPanel.Children.Count < 2)
    {
      InfoMessageBox.Show(this, "Ошибка", "В голосовании должно быть минимум два варианта ответа");
      return;
    }
    if (AnswerBox.Text.IsNullOrEmpty())
    {
      InfoMessageBox.Show(this, "Ошибка", "Название голосования не установлено");
      return;
    }

    foreach (TextBox answer in AnswersPanel.Children)
    {
      if (answer.Text.IsNullOrEmpty())
      {
        InfoMessageBox.Show(this, "Ошибка", "Не все варианты ответов установлены");
        return;
      }
    }

    if (EndDate.SelectedDate == null)
    {
      InfoMessageBox.Show(this, "Ошибка", "Не установлена дата окончания голосования");
      return;
    }
    else if (EndDate.SelectedDate.Value.DateTime < DateTime.Now)
    {
      InfoMessageBox.Show(this, "Ошибка", "Дата окончания не может находиться в прошлом");
      return;
    }

    if (StartDate.SelectedDate != null &&
       (StartDate.SelectedDate.Value.DateTime.Day < DateTime.Now.Day ||
        StartDate.SelectedDate.Value.DateTime > EndDate.SelectedDate.Value.DateTime))
    {
      InfoMessageBox.Show(this, "Ошибка", "Дата начала голосования установлена некорректно");
      return;
    }

    answers = new List<Answer>();
    foreach (TextBox answer in AnswersPanel.Children)
    {
      if (answers.Where(ans => ans.Name == answer.Text).Any())
      {
        InfoMessageBox.Show(this, "Ошибка", "Ответы повторяются");
        return;
      }
      answers.Add(new Answer(answer.Text));
    }
    vote = (StartDate.SelectedDate != null) ? new Vote(AnswerBox.Text, answers,
                                                       EndDate.SelectedDate.Value.DateTime,
                                                       StartDate.SelectedDate.Value.DateTime) :
                                              new Vote(AnswerBox.Text, answers, EndDate.SelectedDate.Value.DateTime);
    var vote_add = await VariantMessageBox.Show(this, "Предупреждение",
                                                $"Вы действительно хотите добавить голосование '{AnswerBox.Text}'?");
    if (!vote_add)
    {
      InfoMessageBox.Show(this, "Информация", "Добавление голосования было отменено");
      return;
    }
  
    if (!ApplicationContext.AddVote(vote))
    {
      InfoMessageBox.Show(this, "Ошибка", "Добавление голосования прошло неудачно - такое голосование уже есть в системе");
      return;
    }

    InfoMessageBox.Show(this, "Информация", "Добавление голосования прошло успешно", () =>
    {
      CloseWindow();
    });
  }

  private void OnAddTextBoxClick(object? sender, RoutedEventArgs e)
  {
    var textBox = new TextBox
    {
      Watermark = $"Вариант {_answersCount}",
      Margin = new Thickness(0, 0, 0, 5)
    };
    AnswersPanel.Children.Add(textBox);
    _answersCount++;
  }

  private void OnDelTextBoxClick(object? sender, RoutedEventArgs e)
  {
    if (AnswersPanel.Children.Count > 0)
    {
      AnswersPanel.Children.RemoveAt(AnswersPanel.Children.Count - 1);
      _answersCount--;
    }
  }
}
