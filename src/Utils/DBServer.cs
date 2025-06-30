using System.IO;
using System.Text.Json;

namespace Golden_votes.Utils;

public class DBServer
{
  private string _ip = "localhost";
  private string _configPath = ".settings.json";
  public string IP { get { return _ip; } }

  public void Load()
  {
    string json = File.ReadAllText(_configPath);
    _ip = JsonSerializer.Deserialize<string>(json)!;
  }

  public void Setup(string newAddress)
  {
    var options = new JsonSerializerOptions { WriteIndented = true };
    string json = JsonSerializer.Serialize(newAddress, options);
    _ip = newAddress;
    File.WriteAllText(_configPath, json);
  }

  public static bool IsValidIP(string ip)
  {
    string[] parts;
    if (string.IsNullOrWhiteSpace(ip))
      return false;

    parts = ip.Split('.');
    if (parts.Length != 4)
      return false;

    foreach (string part in parts)
    {
      if (string.IsNullOrEmpty(part) || 
          part.Length > 1 && part[0] == '0' || 
          !int.TryParse(part, out int num))
        return false;
      
      if (num < 0 || num > 255)
        return false;
    }
    
    return true;
  }
}
