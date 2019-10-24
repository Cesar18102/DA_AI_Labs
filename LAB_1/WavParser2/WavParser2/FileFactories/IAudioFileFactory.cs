using System;
using System.Collections.Generic;
using System.Text;
using WavParser2.Files;

namespace WavParser2.FileFactories
{
    public interface IAudioFileFactory
    {
        AudioFile ParseAudioFile(string path);
    }
}
