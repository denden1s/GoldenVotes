using System.Text.Json;
using System.IO;

namespace Golden_votes.Utils;

public class DBServer
{
  private string ip = "localhost";
  private string configPath = ".settings.json";

  public string IP { get { return ip; } }

  public void Load()
  {
    string json = File.ReadAllText(configPath);
    ip = JsonSerializer.Deserialize<string>(json)!;
  }

  public void Setup(string newAddress)
  {
    var options = new JsonSerializerOptions { WriteIndented = true };
    string json = JsonSerializer.Serialize(newAddress, options);
    ip = newAddress;
    File.WriteAllText(configPath, json);
  }

    public static bool IsValidIP(string ip)
    {
      if (string.IsNullOrWhiteSpace(ip))
        return false;

      string[] parts = ip.Split('.');
      if (parts.Length != 4)
        return false;

      foreach (string part in parts)
      {
        // Reject empty parts, non-numeric, or leading zeros
        if (string.IsNullOrEmpty(part) || 
          part.Length > 1 && part[0] == '0' || 
          !int.TryParse(part, out int num))
          return false;
        
        // Validate number range
        if (num < 0 || num > 255)
          return false;
      }
      
      return true;
    }
}
