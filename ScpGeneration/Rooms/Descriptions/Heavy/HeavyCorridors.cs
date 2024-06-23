using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Heavy.Corridors;

namespace ScpGeneration.Rooms.Descriptions.Heavy;

public class HeavyCorridors : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: true,
            roomTemplates:
            [
                new HeavyCorridorA().Get()
            ]);
    }
}