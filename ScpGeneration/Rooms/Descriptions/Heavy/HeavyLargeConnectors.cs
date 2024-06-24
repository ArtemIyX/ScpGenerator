using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Gates;
using ScpGeneration.Rooms.Templates.Basic.Hubs;

namespace ScpGeneration.Rooms.Descriptions.Heavy;

public class HeavyLargeConnectors : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new HubA().Get()
            ]);
    }
}