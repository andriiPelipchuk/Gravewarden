using System;

public static class EventManager
{
    public static event Action<int> EnemyHasDied;
    public static event Action PlayerHasDied;

    public static void TriggerEnemyHasDied(int amount) => EnemyHasDied?.Invoke(amount);
    public static void TriggerPlayerHasDied() => PlayerHasDied?.Invoke();

}