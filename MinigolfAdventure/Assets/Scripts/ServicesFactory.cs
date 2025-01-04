using UnityEngine;

public class ServicesFactory : MonoBehaviour
{
    [Header("Prefabs / Scene References")]
    public FileHandler fileHandlerPrefab;
    public ScoreService scoreServicePrefab;

    private IFileHandler fileHandlerInstance;
    private IScoreService scoreServiceInstance;

    void Awake()
    {
        // Instanciar el FileHandler
        if (fileHandlerPrefab != null)
        {
            var handlerObj = Instantiate(fileHandlerPrefab);
            fileHandlerInstance = handlerObj.GetComponent<IFileHandler>();
        }

        // Instanciar el ScoreService y asignarle FileHandler
        if (scoreServicePrefab != null)
        {
            var scoreObj = Instantiate(scoreServicePrefab);
            scoreServiceInstance = scoreObj.GetComponent<IScoreService>();

            if (scoreServiceInstance is ScoreService concreteScoreService)
            {
                concreteScoreService.Init(fileHandlerInstance);
            }
        }
    }

    public IFileHandler GetFileHandler()
    {
        return fileHandlerInstance;
    }

    public IScoreService GetScoreService()
    {
        return scoreServiceInstance;
    }
}
