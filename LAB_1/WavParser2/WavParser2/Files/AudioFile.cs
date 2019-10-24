using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WavParser2.Files
{
    public abstract class AudioFile : IAudioFile
    {
        public short ChannelsCount { get; protected set; }
        public int SampleRate { get; protected set; }
        public int ByteRate { get; protected set; }
        public short BlockAlign { get; protected set; }
        public short BitsPerSample { get; protected set; }

        public byte[] Data { get; protected set; }

        public AudioFile() { }
        public AudioFile(short channelsCount, int sampleRate, int byteRate, short blockAlign, short bitsPerSample, byte[] data)
        {
            ChannelsCount = channelsCount;
            SampleRate = sampleRate;
            ByteRate = byteRate;
            BlockAlign = blockAlign;
            BitsPerSample = bitsPerSample;
            Data = data;
        }

        public abstract IAudioFile Sum(AudioFile A);
        public void Sum(AudioFile A, AudioFile R)
        {
            if (this.ChannelsCount != A.ChannelsCount || this.SampleRate != A.SampleRate || this.ByteRate != A.ByteRate || this.BlockAlign != A.BlockAlign || this.BitsPerSample != A.BitsPerSample)
                throw new IncompatibleFilesException();

            byte[] sum = new byte[Math.Max(this.Data.Length, A.Data.Length)];
            for (int i = 0; i < sum.Length; i++)
                sum[i] += (byte)(2 * (i < this.Data.Length ? this.Data[i] : 0) + (i < A.Data.Length ? A.Data[i] : 0));

            R.ChannelsCount = this.ChannelsCount;
            R.SampleRate = this.SampleRate;
            R.ByteRate = this.ByteRate;
            R.BlockAlign = this.BlockAlign;
            R.BitsPerSample = this.BitsPerSample;
            R.Data = sum;
        }

        public override string ToString() => "Channels Count = " + ChannelsCount + "\nSample Rate = " + SampleRate + "\nBits Per Sample = " + BitsPerSample;

        public abstract void ParseFile(string path);
        public abstract void ConstructFile(string path);
    }

    public class IncompatibleFilesException : Exception { }
    public class WrongFileFormatException : Exception { }

    public static class Extensions
    {
        public static byte[] Read(this Stream str, int count)
        {
            byte[] buffer = new byte[count];
            str.Read(buffer, 0, count);
            return buffer;
        }
    }
}
