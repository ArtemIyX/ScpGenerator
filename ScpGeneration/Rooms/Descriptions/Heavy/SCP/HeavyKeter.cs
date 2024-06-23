using Edgar.GraphBasedGenerator.Grid2D;
using ScpGeneration.Rooms.Templates.Basic.Room;

namespace ScpGeneration.Rooms.Descriptions.Heavy.SCP;

public class HeavyKeter: IRoomDescription
{
    public RoomDescriptionGrid2D Get()
    {
        return new RoomDescriptionGrid2D(
            isCorridor: false,
            roomTemplates:
            [
                new HeavyKeterRoom().Get()
            ]);
    }
}