using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Gates;

namespace ScpGeneration.Rooms.Descriptions.Heavy;

public class HeavyVerticalGates : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new GateVertical().Get()
            ]);
    }
}