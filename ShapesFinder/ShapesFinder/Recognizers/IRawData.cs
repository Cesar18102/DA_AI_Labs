using System;
using System.Collections.Generic;

namespace ShapesFinder.Recognizers
{
    public interface IRawData : IDisposable
    {
        bool IsEmpty(int x, int y);

        int Width { get; }
        int Height { get; }

        int X { get; }
        int Y { get; }

        void Erase(IEnumerable<(int x, int y)> pts);
        IRawData GetCropped(IEnumerable<(int x, int y)> pts);

        void Save(string filename);
    }
}
