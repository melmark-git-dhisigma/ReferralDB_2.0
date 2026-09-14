using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public static class ReportLogger
{
    private static readonly object _lock = new object();

    public static void Save(clsReportExecutionLog log)
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath("~/ErrorLog");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName =
                "ReportLog_" + DateTime.Now.ToString("yyyy_MM") + ".csv";

            string fullPath = Path.Combine(path, fileName);

            bool fileExists = File.Exists(fullPath);

            lock (_lock)
            {
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    if (!fileExists)
                    {
                        sw.WriteLine(
                            "ReportName,UserId,ServerID,StartTime,EndTime,DurationMs,RowCount,Status,Parameters,ErrorMessage");
                    }

                    sw.WriteLine(
                        Escape(log.ReportName) + "," +
                        Escape(log.UserId.ToString()) + "," +
                        Escape(log.ServerID) + "," +
                        log.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," +
                        log.EndTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," +
                        log.DurationMs + "," +
                        log.RowCount + "," +
                        Escape(log.Status) + "," +
                        Escape(log.Parameters) + "," +
                        Escape(log.ErrorMessage));
                }
            }
        }
        catch
        {
            // Never let logging break report execution
        }
    }

    private static string Escape(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

}