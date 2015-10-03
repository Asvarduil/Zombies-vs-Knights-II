public interface IResourceStat
{
    int Resource { get; }
    int Cap { get; }
    bool IsExhausted { get; }

    void GainResource(int amount);
    void Consume(int amount);
}
