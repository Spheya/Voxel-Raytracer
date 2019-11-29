using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.src.Engine.Shaders;

namespace Game.Engine.Shaders
{
    static class ShaderPreprocessor
    {
        public static string Execute(string file)
        {
            return Include(file, new List<string>());
        }

        private static string Include(string file, List<string> included)
        {
            StringBuilder result = new StringBuilder();

            string[] code;
            try
            {
                code = File.ReadAllLines(file);
            }
            catch (Exception)
            {
                throw new ShaderPreprocessorException("Error in include statement: File \"" + file + "\" not found");
            }

            if (code[0] == "#pragma once")
            {
                if (included.Contains(Path.GetFullPath(file)))
                    return "";

                included.Add(Path.GetFullPath(file));
            }

            foreach (string line in code)
            {
                if (line.StartsWith("#include"))
                {
                    string[] segments = line.Split('\"');
                    if (segments.Length != 3)
                        throw new ShaderPreprocessorException("Syntax error on include statement");

                    string data = Include(Path.GetDirectoryName(file) + '\\' + segments[1], included);

                    result.AppendLine(data);
                }
                else
                {
                    result.AppendLine(line);
                }
            }

            return result.ToString();
        }
    }
}
