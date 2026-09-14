using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class clsReportExecutionLog
{
    public string ReportName { get; set; }
    
    public int UserId { get; set; }

    public string ServerID { get; set; }
    
    public string Parameters { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public long DurationMs { get; set; }

    public int RowCount { get; set; }

    public string Status { get; set; }

    public string ErrorMessage { get; set; }
}