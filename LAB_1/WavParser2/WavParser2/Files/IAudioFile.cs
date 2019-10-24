using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavParser2.Files
{
    public interface IAudioFile
    {
        void ParseFile(string path);
        void ConstructFile(string path);
    }
}
