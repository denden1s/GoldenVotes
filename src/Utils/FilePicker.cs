using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace Golden_votes.Utils;

public class FileService
{
  private readonly Window _kTargetWindow;

  public FileService(Window targetWindow) => _kTargetWindow = targetWindow;

  public async Task<string?> OpenFilePickerAsync()
  {
    var storageProvider = _kTargetWindow.StorageProvider;
    var options = new FilePickerOpenOptions
    {
      Title = "Выберите файл",
      AllowMultiple = false,
      FileTypeFilter = null
    };
    var files = await storageProvider.OpenFilePickerAsync(options);
    return files?.FirstOrDefault()?.Path?.LocalPath;
  }
}