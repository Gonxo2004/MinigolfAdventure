public interface IScoreService
{
    /// <summary>
    /// Devuelve el récord personal para el hoyo indicado, o "None" si no existe.
    /// </summary>
    string GetPersonalBest(int currentHole);

    /// <summary>
    /// Actualiza el récord personal si 'currentPar' es mejor y lo retorna; 
    /// si no hay mejora, retorna el anterior.
    /// </summary>
    string UpdatePersonalBest(int currentHole, int currentPar);
}
