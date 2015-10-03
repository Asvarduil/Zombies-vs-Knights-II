public interface IHealthStat
{
    int HP { get; }
    int MaxHP { get; }
    bool IsDead { get; }

    void Heal(int amount);
    void TakeDamage(int amount);
}
