using UnityEngine;

public class ScoreService : MonoBehaviour, IScoreService
{
    private IFileHandler fileHandler;

    public void Init(IFileHandler fileHandler)
    {
        this.fileHandler = fileHandler;
    }

    public string GetPersonalBest(int currentHole)
    {
        string fileName = $"PB_Hole{currentHole}";
        string content = fileHandler.ReadFromFile(fileName);

        if (!string.IsNullOrEmpty(content))
        {
            return content;
        }
        else
        {
            Debug.LogWarning($"Personal best file for hole {currentHole} not found. Setting default.");
            return "None";
        }
    }

    public string UpdatePersonalBest(int currentHole, int currentPar)
    {
        // Leemos el récord
        string currentBestString = GetPersonalBest(currentHole);
        int currentBest = int.MaxValue;
        if (int.TryParse(currentBestString, out int parsed))
        {
            currentBest = parsed;
        }

        // Actualizamos si hay mejora
        if (currentPar < currentBest)
        {
            string newBest = currentPar.ToString();
            fileHandler.WriteInFile($"PB_Hole{currentHole}", newBest);
            Debug.Log($"New personal best for hole {currentHole}: {newBest}");
            return newBest;
        }
        return currentBestString;
    }
}
