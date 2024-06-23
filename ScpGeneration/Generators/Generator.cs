using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Generators;

public abstract class Generator<TRoom>
{
    public Random Random { get; private set; }

    public Generator()
    {
        Random = new Random();
    }

    public Generator(int seed)
    {
        Random = new Random(seed);
    }

    public LayoutGrid2D<TRoom> GenerateLayout()
    {
        LevelDescriptionGrid2D<TRoom> levelDescription = GetLevelDescription();
        GraphBasedGeneratorGrid2D<TRoom> generator = new GraphBasedGeneratorGrid2D<TRoom>(levelDescription);
        generator.InjectRandomGenerator(Random);
        LayoutGrid2D<TRoom>? layout = generator.GenerateLayout();
        return layout;
    }

    public abstract LevelDescriptionGrid2D<TRoom> GetLevelDescription();
}