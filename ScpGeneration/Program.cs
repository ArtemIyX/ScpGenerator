using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Grid2D.Drawing;
using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.GraphBasedGenerator.Common;
using System.Diagnostics;
using Edgar.Graphs;
using ScpGeneration.Generators;
using ScpGeneration.Utils;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            IComposer composer = new Composer();

            int seed = 0;
            seed = new Random().Next(int.MaxValue - 1);
            Generator<int> gen = new HeavyGenerator(seed);
            var layout = gen.GenerateLayout();
            composer.SavePng(layout,
                new DungeonDrawerOptions()
                {
                    Height = 2000,
                    Width = 2000,
                    ShowRoomNames = true,
                    EnableShading = false,
                },
                "layout.png");
            composer.SaveJson(layout, "layout.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}