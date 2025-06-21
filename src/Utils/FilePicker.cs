using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace Golden_votes.Utils;

public class FileService
{
  private readonly Window _targetWindow;

  public FileService(Window targetWindow)
  {
      _targetWindow = targetWindow;
  }

  public async Task<string?> OpenFilePickerAsync()
  {
    // Получаем сервис для работы с файлами
    var storageProvider = _targetWindow.StorageProvider;

    // Настраиваем параметры диалога
    var options = new FilePickerOpenOptions
    {
      Title = "Выберите файл",
      AllowMultiple = false, // Разрешаем выбор только одного файла
      FileTypeFilter = null // Отключаем фильтрацию по типам файлов
    };

    // Открываем диалог выбора файла
    var files = await storageProvider.OpenFilePickerAsync(options);

    // Возвращаем путь к первому выбранному файлу (или null, если файл не выбран)
    return files?.FirstOrDefault()?.Path?.LocalPath;
  }
}