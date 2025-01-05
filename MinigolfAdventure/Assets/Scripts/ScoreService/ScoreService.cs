using System;
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

    public void SaveFinalScores(int hole, int playerScore, int aiScore)
    {
        // El archivo donde guardaremos el histórico
        string scoresFileName = "AllScores.txt";

        // Leemos lo que hubiera (si existe) para no pisarlo
        string existingData = fileHandler.ReadFromFile(scoresFileName);

        // Si no hay nada, inicializamos a vacío
        if (string.IsNullOrEmpty(existingData))
        {
            existingData = "";
        }

        // Creamos la línea de registro
        // Puedes darle el formato que prefieras (CSV, JSON, etc.)
        string newLine = $"Hole {hole} -> Player: {playerScore}, AI: {aiScore}\n";

        // Anexamos la nueva línea
        existingData += newLine;

        // Escribimos todo de nuevo al fichero
        fileHandler.WriteInFile(scoresFileName, existingData);

        Debug.Log($"Final scores appended to {scoresFileName}: {newLine}");
    }

    public string[] GetAllScoresHistory()
    {
        string scoresFileName = "AllScores.txt";
        string existingData = fileHandler.ReadFromFile(scoresFileName);

        if (string.IsNullOrEmpty(existingData))
        {
            // Si no hay archivo o está vacío
            return new string[] { "No Scores Yet" };
        }

        // Dividimos el contenido por saltos de línea
        // Eliminando entradas vacías en caso de haber saltos extra
        string[] lines = existingData.Split(
            new[] { '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

        return lines;
    }
}
