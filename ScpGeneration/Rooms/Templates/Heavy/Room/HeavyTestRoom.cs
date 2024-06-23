using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Heavy.Room;

public class HeavyTestRoom : IRoomTemplate
{
    public RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? outline = PolygonGrid2D.GetRectangle(6, 3);
        //var doors = new SimpleDoorModeGrid2D(doorLength: 1, cornerDistance: 1);
        ManualDoorModeGrid2D doors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(0, 1), new Vector2Int(0, 2)),
                new DoorGrid2D(new Vector2Int(6, 1), new Vector2Int(6, 2))
            ]
        );

        return new RoomTemplateGrid2D(outline, doors, this.GetType().Name,
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}