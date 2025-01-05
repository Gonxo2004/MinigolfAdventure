public interface IScoreService
{
    string GetPersonalBest(int currentHole);
    string UpdatePersonalBest(int currentHole, int currentPar);
    void SaveFinalScores(int hole, int playerScore, int aiScore);

}
