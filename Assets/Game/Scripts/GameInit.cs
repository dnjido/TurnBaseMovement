using System;

public class GameInit // Сообщает другим скриптам, что игра началась
{
    public delegate void StartDelegate();
    public event StartDelegate StartEvent;

    public void Init() =>
        StartEvent?.Invoke();
}

public static class GameCheck
{
    public static GameInit game { get; private set; }

    static GameCheck() =>
        game = new GameInit();

    public static void Start() =>
        game.Init();
}
