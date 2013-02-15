using System.IO;

namespace ForexTickAnalyzer
{
    public class LogDataWriter
    {
        private readonly StreamWriter writer;

        public LogDataWriter(string name)
        {
            writer = File.CreateText(name);
            writer.AutoFlush = true;
        }

        public void WriteLog(string content)
        {
            writer.WriteLine(content);
        }

        public void Close()
        {
            writer.Close();
        }
    }
}