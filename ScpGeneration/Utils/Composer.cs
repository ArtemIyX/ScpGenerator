﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.GraphBasedGenerator.Grid2D.Drawing;

namespace ScpGeneration.Utils;

interface IComposer
{
    public bool SavePng<TRoom>(LayoutGrid2D<TRoom> layout, DungeonDrawerOptions drawerOptions, string path);
}

internal class Composer : IComposer
{
    public bool SavePng<TRoom>(LayoutGrid2D<TRoom> layout, DungeonDrawerOptions drawerOptions, string path)
    {
        DungeonDrawer<TRoom> drawer = new DungeonDrawer<TRoom>();
        Bitmap? bitmap = drawer.DrawLayout(layout, drawerOptions);
        if (bitmap is null) return false;
        bitmap.Save(path);
        return true;
    }
}