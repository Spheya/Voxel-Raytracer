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
        static ShaderPreprocessor()
        {

        }

        public static string Execute(string definitions, string[] code, string rootLocation)
        {
            string result = "";

            foreach (string line in code)
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
                        fileContent = File.ReadAllLines(filePath + fileName);
                    }
                    catch (Exception e) {
                        throw new ShaderPreprocessorException("Error in include statement: File not found");
                    }

                    string data = Execute(definitions, fileContent, Path.GetDirectoryName(filePath + fileName));

                    result += data;
                    result += '\n';
                }
                else
                {
                    result += line;
                    result += '\n';
                }
            }

            return result;
        }
    }
}
