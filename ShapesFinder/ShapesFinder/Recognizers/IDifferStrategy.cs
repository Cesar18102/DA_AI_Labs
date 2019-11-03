using System.Collections.Generic;

namespace ShapesFinder.Recognizers
{
    public interface IDifferStrategy
    {
        IEnumerable<IRawData> DifferShape(IRawData data);
    }
}
