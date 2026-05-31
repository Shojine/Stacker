public interface IGameObserver
{
    void OnGameEnd();
    void OnScoreChanged(float newScore);
}

public interface ICubeObserver
{
    void OnCubeFellOff(Cube cube);
}
