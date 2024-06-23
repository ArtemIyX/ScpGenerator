using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates;

public abstract class RoomTemplate
{
    public string GetName()
    {
        return this.GetType().Name;
    }
    public abstract RoomTemplateGrid2D Get();
}