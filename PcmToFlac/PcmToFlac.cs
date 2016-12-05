using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.RAW;
using CSCore.DSP;
using CUETools.Codecs;
using CUETools.Codecs.FLAKE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcmToFlac
{
    public class PcmToFlac
    {
        public static byte[] Convert(byte[] input, int channels)
        {
            using (var dest = new MemoryStream()) {
                using (var resampledSource = new MemoryStream())
                {
                    using (var source = new RawDataReader(new MemoryStream(input, false), new WaveFormat(48000, 16, channels)))
                    {
                        source
                            .ToMono()
                            .ChangeSampleRate(16000)
                            .ToSampleSource()
                            .ToWaveSource(16)
                            .WriteToStream(resampledSource);
                    }
                    resampledSource.Seek(0, SeekOrigin.Begin);

                    // https://bitbucket.org/josephcooney/cloudspeech/src/8619cf699541ed2cd3f0ce52208a5f8a273fdc37/CloudSpeech/SpeechToText.cs?at=default&fileviewer=file-view-default
                    var reader = new WAVReader(null, resampledSource, new AudioPCMConfig(16, 1, 16000));

                    var buf = new AudioBuffer(reader, 0x10000);
                    var flakeWriter = new FlakeWriter(null, dest, reader.PCM);
                    flakeWriter.CompressionLevel = 11;
                    while (reader.Read(buf, -1) != 0)
                    {
                        flakeWriter.Write(buf);
                    }
                    flakeWriter.Close();
                    return dest.ToArray();
                }
            }
        }
    }
}
