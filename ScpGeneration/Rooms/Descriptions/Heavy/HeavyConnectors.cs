using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Connectors;

namespace ScpGeneration.Rooms.Descriptions.Heavy;

public class HeavyConnectors : IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new ConnectorA().Get(),
                new ConnectorB().Get()
            ]);
    }
}