using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates;

public interface IRoomTemplate
{
    public RoomTemplateGrid2D Get();
}