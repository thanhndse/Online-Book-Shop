using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopWithAuthen.Service.OtherServices
{
    public interface ILogService
    {
        void LogException(string message);
    }
    public sealed class LogService : ILogService
    {
        private LogService()
        {

        }

        private static readonly Lazy<LogService> instance = new Lazy<LogService>(()=>new LogService());
        public static LogService GetInstance
        {
            get
            {
                return instance.Value;
            }
        }
        public void LogException(string message)
        {
            string fileName = string.Format("{0}_{1}.log", "Exception", DateTime.Now.ToString("dd-MM-yyyy"));
            string logFilePath = string.Format(@"{0}-{1}", AppDomain.CurrentDomain.BaseDirectory+ "Logging\\", fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            string tmpString = null;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(logFilePath);
                tmpString = reader.ReadToEnd();
            }
            catch
            {

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            File.Delete(logFilePath);
            using (StreamWriter writer = new StreamWriter(logFilePath,true))
            {
                if (tmpString == null)
                {
                    writer.Write(sb.ToString());
                }
                else
                {
                    writer.Write(sb.ToString() + Environment.NewLine + tmpString);
                }
                writer.Flush();
            }
        }
    }
}
