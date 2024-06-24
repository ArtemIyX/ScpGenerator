using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Corridors;

namespace ScpGeneration.Rooms.Descriptions.Heavy;

public class HeavyCorridors : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: true,
            roomTemplates:
            [
                new CorridorA().Get(),
                new CorridorB().Get(),
                new CorridorC().Get(),
                new CorridorD().Get(),
                new CorridorE().Get()
            ]);
    }
}