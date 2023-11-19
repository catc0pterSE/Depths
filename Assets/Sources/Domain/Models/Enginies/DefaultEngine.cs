namespace Sources.Domain.Models.Enginies
{
    public class Engine : IEngine
    {
        public Engine(int speed)
        {
            Speed = speed;
        }

        public int Speed { get; }
    }
}