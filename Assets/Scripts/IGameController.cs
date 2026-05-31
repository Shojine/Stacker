// "Subject" interface for controlling the game. Implemented by BOTH the real
// GameManager and the GameControllerProxy, so callers can't tell which one they
// hold -- that interchangeability is what makes it a Proxy.
public interface IGameController
{
    void AddScore(float points);
    void EndGame();
}
