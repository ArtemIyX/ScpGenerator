using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Descriptions;

public interface IRoomDescription
{
    public RoomDescriptionGrid2D Get();
}