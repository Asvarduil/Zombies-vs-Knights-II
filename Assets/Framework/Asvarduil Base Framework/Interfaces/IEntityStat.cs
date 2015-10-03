public interface IEntityStat
{
    int Current { get; set; }
    int Maximum { get; set; }

    void RaiseMax(int amount);
}
