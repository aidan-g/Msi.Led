using System;
using System.Collections.Generic;
using System.CommandLine.Parser;
using System.CommandLine.Parser.Parameters;
using System.Runtime.InteropServices;
using Msi.Led.Core;
using System.Linq;

namespace Msi.Led.Controller
{
    public static class Program
    {
        public static Color DEFAULT_COLOR = new Color()
        {
            Red = 255,
            Green = 0,
            Blue = 0
        };

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GetCommandLine();

        private static void GetCommandLine(out string commandLine)
        {
            commandLine = Marshal.PtrToStringUni(GetCommandLine());
        }

        public static int Main(string[] args)
        {
            return Main(GetColor());
        }

        public static int Main(Color color)
        {
            Platform.Initialize();
            try
            {
                var count = Platform.GetGPUCount();
                for (var index = 0; index < count; index++)
                {
                    Platform.SetIlluminationParm(index, 0, 0);
                    Platform.SetIlluminationParmColor_RGB(index, State.ON, Light.ALL, 0, 0, 0, 4, 0, 0, color, false);
                }
            }
            finally
            {
                Platform.Unload();
            }
            return 0;
        }

        private static Color GetColor()
        {
            var commandLine = default(string);
            GetCommandLine(out commandLine);
            if (string.IsNullOrEmpty(commandLine))
            {
                return DEFAULT_COLOR;
            }
            var arguments = commandLine.Split(new[] { "Msi.Led.Controller.exe", "\"" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (string.IsNullOrEmpty(arguments))
            {
                return DEFAULT_COLOR;
            }
            var parser = new CommandLineParser();
            var parameters = parser.Parse(arguments);
            return new Color()
            {
                Red = GetParameterOrZero(parameters.Parameters, "r"),
                Green = GetParameterOrZero(parameters.Parameters, "g"),
                Blue = GetParameterOrZero(parameters.Parameters, "b")
            };
        }

        private static byte GetParameterOrZero(IDictionary<string, Parameter> parameters, string name)
        {
            var parameter = default(Parameter);
            if (!parameters.TryGetValue(name, out parameter))
            {
                return 0;
            }
            if (parameter.Kind != ParameterKind.Number)
            {
                return 0;
            }
            return Convert.ToByte((parameter as NumberParameter).Value);
        }
    }
}
