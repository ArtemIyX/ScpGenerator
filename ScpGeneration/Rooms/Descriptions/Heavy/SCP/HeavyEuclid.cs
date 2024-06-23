using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Room;

namespace ScpGeneration.Rooms.Descriptions.Heavy.SCP;

public class HeavyEuclid : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new HeavyEuclidRoom().Get()
            ]);
    }
}