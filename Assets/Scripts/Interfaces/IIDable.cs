namespace Assets.Scripts.Interfaces
{
    public interface IIDable
    {
        public int ID { get; }

        public void AssignID(int id);
    }
}
