using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Room;

namespace ScpGeneration.Rooms.Descriptions.Heavy.SCP;

public class HeavySafe : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new HeavySafeRoom().Get()
            ]);
    }
}