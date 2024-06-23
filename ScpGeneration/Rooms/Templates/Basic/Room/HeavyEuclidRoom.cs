using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Room;

public class HeavyKeterRoom : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? outline = PolygonGrid2D.GetRectangle(10, 7);
        SimpleDoorModeGrid2D doors = new SimpleDoorModeGrid2D(doorLength: 1, cornerDistance: 1);

        return new RoomTemplateGrid2D(outline, doors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}