using System;
using System.Runtime.InteropServices;

namespace Msi.Led.Core
{
    public class Platform
    {
        [DllImport(@"lib\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_Initialize();

        [DllImport(@"lib\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_Unload();

        [DllImport(@"lib\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_GetGPUCounts(out long count);

        [DllImport(@"lib\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_SetIlluminationParm(int index, int attribute, int value);

        [DllImport(@"lib\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_SetIlluminationParmColor_RGB(int index, int cmd, int led1, int led2, int ontime, int offtime, int time, int darktime, int bright, int r, int g, int b, bool one = false);

        public static void Initialize()
        {
            if (!NDA_Initialize())
            {
                throw new PlatformException("Failed to initialize.");
            }
        }

        public static void Unload()
        {
            if (!NDA_Unload())
            {
                throw new PlatformException("Failed to unload.");
            }
        }

        public static int GetGPUCount()
        {
            var count = default(long);
            if (!NDA_GetGPUCounts(out count))
            {
                throw new PlatformException("Failed to determine GPU count.");
            }
            return Convert.ToInt32(count);
        }

        public static void SetIlluminationParm(int index, int attribute, int value)
        {
            if (!NDA_SetIlluminationParm(index, attribute, value))
            {
                throw new PlatformException("Failed to set illumination parameter.");
            }
        }

        public static void SetIlluminationParmColor_RGB(int index, int cmd, int led1, int led2, int ontime, int offtime, int time, int darktime, int bright, Color color, bool one = false)
        {
            if (!NDA_SetIlluminationParmColor_RGB(index, cmd, led1, led2, ontime, offtime, time, darktime, bright, color.Red, color.Green, color.Blue, one))
            {
                throw new PlatformException("Failed to set illumination parameter.");
            }
        }
    }

    public class PlatformException : Exception
    {
        public PlatformException(string message) : base(message)
        {

        }
    }
}
