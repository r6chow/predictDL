using System;
using System.Threading;
using System.IO;
using System.Xml;
using log4net;
using log4net.Config;

namespace PitchFXConsole
{
    public class FileParser
    {
        private static ILog log = LogManager.GetLogger("PitchFXConsole");

        public static void Run()
        {
            while (true)
            {
                Thread.Sleep(100);
                var item = QueueManager.Instance.GetFileToParse();
                if (item == null && QueueManager.Instance.IsFinishedDownloading())
                    break;
                else if (item != null)
                {
                    ParsePitchFile(item);
                    ParsePlayerFile(item);
                }
            }
        }

        private static void ParsePitchFile(QueueItem item)
        {
            try
            {
            var xmlString = File.ReadAllText(string.Format("{0}\\inning_all.xml", item.OutputDir));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            doc.ParsePitchXml(item.GameDate, item.GameID, item.OutputDir);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in parsing for url {1}: {0}", ex.ToString(), string.Format("{0}\\inning_all.xml", item.OutputDir)));
            }
        }

        private static void ParsePlayerFile(QueueItem item)
        {
  

            try
            {
                var xmlString = File.ReadAllText(string.Format("{0}\\players.xml", item.OutputDir));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                doc.ParsePlayerXml(item.GameDate, item.GameID, item.OutputDir);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in parsing for url {1}: {0}", ex.ToString(), string.Format("{0}\\players.xml", item.OutputDir)));
            }

        }
    }
}
