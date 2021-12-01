namespace Modo.Clients.Models
{
    public record KeyValue
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public KeyValue(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
