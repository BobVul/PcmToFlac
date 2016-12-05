﻿using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.RAW;
using CSCore.DSP;
using FlacBox;
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
                            .WriteToWaveStream(resampledSource);
                    }
                    // unfortunately WriteToWaveStream closes the stream...
                    using (var resampledSourceCopy = new MemoryStream(resampledSource.ToArray()))
                    {
                        resampledSourceCopy.CopyTo(new WaveOverFlacStream(dest, WaveOverFlacStreamMode.Encode, true));
                    }

                    return dest.ToArray();
                }
            }
        }
    }
}
