using System.Collections.Generic;

namespace ShapesFinder.Recognizers.Strategies
{
    public class InnerTraversalStrategy : IDifferStrategy
    {
        public IEnumerable<IRawData> DifferShape(IRawData data)
        {
            return new List<IRawData>();
        }
    }
}
