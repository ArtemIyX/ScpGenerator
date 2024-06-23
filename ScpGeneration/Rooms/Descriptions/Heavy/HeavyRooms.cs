using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Heavy.Room;

namespace ScpGeneration.Rooms.Descriptions.Heavy;

public class HeavyRooms : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new HeavyTestRoom().Get()
            ]);
    }
}