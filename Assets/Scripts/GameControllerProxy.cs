using UnityEngine;

// Protection proxy: stands in for the real IGameController (GameManager) and
// guards access to it. Callers go through the proxy; it validates each request,
// logs it, then delegates to the real controller. Note it is a plain C# class,
// NOT a MonoBehaviour -- it lives entirely in code.
public class GameControllerProxy : IGameController
{
    private readonly IGameController real; // the real subject being protected
    private bool gameEnded = false;        // proxy-side state used to guard access

    public GameControllerProxy(IGameController real)
    {
        this.real = real;
    }

    public void AddScore(float points)
    {
        // Guard 1: no scoring once the game is over.
        if (gameEnded)
        {
            Debug.LogWarning("[Proxy] Ignoring AddScore - game has already ended.");
            return;
        }

        // Guard 2: reject nonsensical score values instead of trusting the caller.
        if (points <= 0f)
        {
            Debug.LogWarning($"[Proxy] Rejected invalid score value: {points}");
            return;
        }

        Debug.Log($"[Proxy] AddScore({points})");
        real.AddScore(points); // access granted -> delegate to the real controller
    }

    public void EndGame()
    {
        // Guard: collapse duplicate end-game requests into a single real call.
        if (gameEnded)
        {
            Debug.Log("[Proxy] EndGame already handled - ignoring duplicate.");
            return;
        }

        gameEnded = true;
        Debug.Log("[Proxy] EndGame()");
        real.EndGame(); // access granted -> delegate to the real controller
    }
}
