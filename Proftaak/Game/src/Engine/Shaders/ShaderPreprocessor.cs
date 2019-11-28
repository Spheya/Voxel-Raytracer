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
        public static string Execute(string[] definitions, string[] code, string rootLocation)
        {
            if (code.Length == 0)
                return "";

            StringBuilder result = new StringBuilder();

            result.AppendLine(code[0]);

            if (definitions != null)
                foreach (string def in definitions)
                    result.AppendLine("#define " + def);

            foreach (string line in code.Skip(1))
            {
                if (line.StartsWith("#include"))
                {
                    string[] segments = line.Split('\"');
                    if (segments.Length != 3)
                        throw new ShaderPreprocessorException("Syntax error on include statement");

                    string fileName = segments[1];
                    string filePath = rootLocation + fileName;

                    string[] fileContent;
                    try
                    {
                        fileContent = File.ReadAllLines(filePath);
                    }
                    catch (Exception e) {
                        throw new ShaderPreprocessorException("Error in include statement: File not found");
                    }

                    string data = Execute(null, fileContent, Path.GetDirectoryName(filePath));

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
