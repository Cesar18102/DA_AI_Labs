using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WavParser2.Files
{
    public class WavAudioFile : AudioFile
    {
        public WavAudioFile() { }
        public WavAudioFile(short channelsCount, int sampleRate, int byteRate, short blockAlign, short bitsPerSample, byte[] data) : base(channelsCount, sampleRate, byteRate, blockAlign, bitsPerSample, data) { }

        public override void ParseFile(string path)
        {
            using (Stream str = new StreamReader(path).BaseStream)
            {
                str.Seek(8, SeekOrigin.Begin);

                if (Encoding.ASCII.GetString(str.Read(4)) != "WAVE")
                    throw new WrongFileFormatException();

                str.Seek(22, SeekOrigin.Begin);
                ChannelsCount = BitConverter.ToInt16(str.Read(2), 0);
                SampleRate = BitConverter.ToInt32(str.Read(4), 0);
                ByteRate = BitConverter.ToInt32(str.Read(4), 0);
                BlockAlign = BitConverter.ToInt16(str.Read(2), 0);
                BitsPerSample = BitConverter.ToInt16(str.Read(2), 0);

                str.Seek(40, SeekOrigin.Begin);
                Data = str.Read(BitConverter.ToInt32(str.Read(4), 0));
            }
        }

        public override void ConstructFile(string path)
        {
            using (FileStream fileStream = File.Create(path))
            {
                fileStream.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
                fileStream.Write(BitConverter.GetBytes(Data.Length + 36), 0, 4);
                fileStream.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);
                fileStream.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
                fileStream.Write(BitConverter.GetBytes(16), 0, 4);
                fileStream.Write(BitConverter.GetBytes(1), 0, 2);
                fileStream.Write(BitConverter.GetBytes(ChannelsCount), 0, 2);
                fileStream.Write(BitConverter.GetBytes(SampleRate), 0, 4);
                fileStream.Write(BitConverter.GetBytes(ByteRate), 0, 4);
                fileStream.Write(BitConverter.GetBytes(BlockAlign), 0, 2);
                fileStream.Write(BitConverter.GetBytes(BitsPerSample), 0, 2);
                fileStream.Write(Encoding.ASCII.GetBytes("data "), 0, 4);
                fileStream.Write(BitConverter.GetBytes(Data.Length), 0, 4);
                fileStream.Write(Data, 0, Data.Length);
            }
        }

        public override IAudioFile Sum(AudioFile A)
        {
            WavAudioFile R = new WavAudioFile();
            Sum(A, R);
            return R;
        }
    }
}
