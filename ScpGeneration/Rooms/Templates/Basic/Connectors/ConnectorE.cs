using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Connectors;

public class ConnectorE : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D corridorOutline = PolygonGrid2D.GetRectangle(8, 3);

        IDoorModeGrid2D corridorDoors = new SimpleDoorModeGrid2D(1, 1);

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}