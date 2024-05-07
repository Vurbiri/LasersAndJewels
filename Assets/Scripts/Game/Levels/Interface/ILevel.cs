
public interface ILevel 
{
    public bool Create(int count, int maxDistance);
    public void Run();
    public bool CheckChain();
    public void Clear();

}
