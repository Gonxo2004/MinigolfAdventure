using UnityEngine;
using System.IO;

public class FileHandler : MonoBehaviour, IFileHandler
{
    public void WriteInFile(string fileName, string content)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, content);
        Debug.Log($"File written to: {path}");
    }

    public string ReadFromFile(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            Debug.Log($"File content: {content}");
            return content;
        }
        else
        {
            Debug.LogWarning($"File not found: {path}");
            return null;
        }
    }
}
