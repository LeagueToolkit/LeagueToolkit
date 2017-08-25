using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fantome.Libraries.League.IO.ObjectConfig
{
    public class ObjectConfigFile
    {
        public Dictionary<string, Dictionary<string, object>> Entries { get; private set; } = new Dictionary<string, Dictionary<string, object>>();

        public ObjectConfigFile(Dictionary<string, Dictionary<string, object>> entries)
        {
            this.Entries = entries;
        }

        public ObjectConfigFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public ObjectConfigFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(new char[] { '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line[0].Length != 0)
                    {
                        this.Entries.Add(line[0], new Dictionary<string, object>());
                        ReadValues(sr, line[0]);
                    }
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        public void Write(Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (KeyValuePair<string, Dictionary<string, object>> entry in this.Entries)
                {
                    sw.WriteLine(string.Format("[{0}]", entry.Key));
                    foreach (KeyValuePair<string, object> value in entry.Value)
                    {
                        string valueString = "";

                        if (value.Value.GetType() == typeof(Vector3))
                        {
                            Vector3 vector = value.Value as Vector3;
                            valueString = string.Format("{0} {1} {2}", vector.X, vector.Y, vector.Z);
                        }
                        else if (value.Value.GetType() == typeof(float))
                        {
                            valueString = "" + (float)value.Value;
                        }
                        else if (value.Value.GetType() == typeof(uint))
                        {
                            valueString = "" + (uint)value.Value;
                        }
                        else if (value.Value.GetType() == typeof(string))
                        {
                            valueString = value.Value as string;
                        }

                        sw.WriteLine(string.Format("{0}={1}", value.Key, valueString));
                    }
                }
            }
        }

        private void ReadValues(StreamReader sr, string entry)
        {
            string[] line = null;
            object value;
            uint tryParseValue = 0;

            while (sr.Peek() != '[')
            {
                if (!sr.EndOfStream)
                {
                    if ((line = sr.ReadLine().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).Length != 0)
                    {
                        if (line.Length == 2 && line[1].Count(x => x == '.') > 1)
                        {
                            string[] vector = line[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            value = new Vector3(float.Parse(vector[0], CultureInfo.InvariantCulture),
                                float.Parse(vector[1], CultureInfo.InvariantCulture),
                                float.Parse(vector[2], CultureInfo.InvariantCulture));
                        }
                        else if (line.Length == 2 && line[1].Count(x => x == '.') == 1)
                        {
                            value = float.Parse(line[1], CultureInfo.InvariantCulture);
                        }
                        else if (uint.TryParse(line[1], out tryParseValue))
                        {
                            value = tryParseValue;
                        }
                        else
                        {
                            value = line[1];
                        }

                        this.Entries[entry].Add(line[0], value);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
