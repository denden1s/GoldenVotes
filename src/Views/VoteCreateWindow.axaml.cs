using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Golden_votes.Entities;
using Golden_votes.Utils;
using Microsoft.IdentityModel.Tokens;

namespace Golden_votes.Views;

public partial class VoteCreateWindow : Window
{
  private AdminWindow parentWindow;
  private int answers_count = 1;
  public VoteCreateWindow(AdminWindow win)
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    parentWindow = win;
  }
  private void CloseWindow()
  {
    parentWindow.Show();
    this.Hide();
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
  {
    CloseWindow();
  }

  private async void OnSubmitVote(object? sender, RoutedEventArgs e)
  {
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


    List<Answer> answers = new List<Answer>();
    foreach (TextBox answer in AnswersPanel.Children)
    {
      if (answers.Where(ans => ans.Name == answer.Text).Count() != 0)
      {
        InfoMessageBox.Show(this, "Ошибка", "Ответы повторяются");
        return;
      }
      answers.Add(new Answer(answer.Text));
    }
      Vote vote = (StartDate.SelectedDate != null) ? new Vote(AnswerBox.Text, answers, EndDate.SelectedDate.Value.DateTime, StartDate.SelectedDate.Value.DateTime) :
                                                     new Vote(AnswerBox.Text, answers, EndDate.SelectedDate.Value.DateTime);
    var vote_add = await VariantMessageBox.Show(this, "Предупреждение", $"Вы действительно хотите добавить голосование '{AnswerBox.Text}'?");
    if (vote_add == false)
    {
      InfoMessageBox.Show(this, "Информация", "Добавление голосования было отменено");
      return;
    }
    // if () // TODO: ask about adding because cant remove it, after question add + show message + close form
    // else no clear and show message about
    if (ApplicationContext.AddVote(vote) == false)
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
      Watermark = $"Вариант {answers_count}",
      Margin = new Thickness(0, 0, 0, 5)
    };
    AnswersPanel.Children.Add(textBox);
    answers_count++;
  }
  
  private void OnDelTextBoxClick(object? sender, RoutedEventArgs e)
  {
    if (AnswersPanel.Children.Count > 0)
    {
      AnswersPanel.Children.RemoveAt(AnswersPanel.Children.Count - 1);
      answers_count--;
    }
  }
}