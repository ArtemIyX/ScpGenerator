using Edgar.Legacy.Core.MapLayouts;

namespace ScpGeneration.Data;

public class Room
{
    public Room(int id, RoomType roomType)
    {
        Id = id;
        RoomType = roomType;
    }
    public int Id { get; set; }
    public RoomType RoomType { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }

        Room other = (Room)obj;
        return Id == other.Id && RoomType == other.RoomType;
    }

    protected bool Equals(Room other)
    {
        return Id == other.Id && RoomType == other.RoomType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, (int)RoomType);
    }

    public override string ToString()
    {
        return this.RoomType.ToString();
    }
}