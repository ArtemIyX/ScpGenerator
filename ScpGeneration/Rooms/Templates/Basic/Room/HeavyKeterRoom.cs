using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Room;

public class HeavyEuclidRoom : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? outline = PolygonGrid2D.GetRectangle(10, 7);
        IDoorModeGrid2D doors = new ManualDoorModeGrid2D([
            new DoorGrid2D(new Vector2Int(0, 5), new Vector2Int(0, 6))
        ]);

        return new RoomTemplateGrid2D(outline, doors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}