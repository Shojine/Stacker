// Subject-role interfaces: implemented by classes that PUBLISH events to observers.
public interface IGameSubject
{
    void Subscribe(IGameObserver observer);
    void Unsubscribe(IGameObserver observer);
}

public interface ICubeSubject
{
    void Subscribe(ICubeObserver observer);
    void Unsubscribe(ICubeObserver observer);
}
