public interface IFileHandler
{
    void WriteInFile(string fileName, string content);
    string ReadFromFile(string fileName);
}
