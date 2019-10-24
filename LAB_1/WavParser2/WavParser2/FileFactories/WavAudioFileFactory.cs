using System;
using System.Collections.Generic;
using System.Text;
using WavParser2.Files;

namespace WavParser2.FileFactories
{
    public class WavAudioFileFactory : IAudioFileFactory
    {
        public AudioFile ParseAudioFile(string path)
        {
            WavAudioFile file = new WavAudioFile();
            file.ParseFile(path);
            return file;
        }
    }
}
