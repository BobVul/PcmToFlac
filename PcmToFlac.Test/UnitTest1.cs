using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace PcmToFlac.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMono()
        {
            File.WriteAllBytes(@"bensound-littleidea-48k-s16le-mono.flac", PcmToFlac.Convert(File.ReadAllBytes(@"bensound-littleidea-48k-s16le-mono.raw"), 1));
        }

        [TestMethod]
        public void TestStereo()
        {
            File.WriteAllBytes(@"bensound-littleidea-48k-s16le-stereo.flac", PcmToFlac.Convert(File.ReadAllBytes(@"bensound-littleidea-48k-s16le-stereo.raw"), 2));
        }
    }
}
