namespace ScpGeneration.Data;

public class RoomId
{
    public RoomId(int id, string name)
    {
        Id = id;
        name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public override string ToString()
    {
        return $"[{Id}] {Name}";
    }
}