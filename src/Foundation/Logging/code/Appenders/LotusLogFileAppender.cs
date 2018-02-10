using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using log4net.Appender;
using log4net.spi;
using Lotus.Foundation.Kernel.Structures.Collections;
using Sitecore.Configuration;
using ConfigReader = log4net.Appender.ConfigReader;

namespace Lotus.Foundation.Logging.Appenders
{
  public class LotusLogFileAppender : FileAppender
  {
    private StaticDictionary<string, string> AppenderVariables = new StaticDictionary<string, string>();
    
    private DateTime m_currentDate;
    private string m_originalFileName;

    public LotusLogFileAppender()
    {
      this.m_currentDate = DateTime.Now;
    }

    public override string File
    {
      get
      {
        return base.File;
      }
      set
      {
        if (this.m_originalFileName == null)
        {
          var str = value;
          var variables = ConfigReader.GetVariables();
          foreach (var key in variables.Keys)
          {
            var oldValue = "$(" + key + ")";
            str = str.Replace(oldValue, variables[key]);
          }
          this.m_originalFileName = this.MapPath(str.Trim());
        }
        base.File = this.m_originalFileName;
      }
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      var now = DateTime.Now;
      if (this.m_currentDate.Day != now.Day || this.m_currentDate.Month != now.Month || this.m_currentDate.Year != now.Year)
      {
        lock (this)
        {
          this.CloseFile();
          this.m_currentDate = DateTime.Now;
          this.OpenFile(string.Empty, false);
        }
      }
      base.Append(loggingEvent);
    }

    protected override void OpenFile(string fileName, bool append)
    {
      fileName = this.m_originalFileName;
      fileName = fileName.Replace("{date}", this.m_currentDate.ToString("yyyyMMdd"));
      fileName = fileName.Replace("{time}", this.m_currentDate.ToString("HHmmss"));
      fileName = fileName.Replace("{processid}", LotusLogFileAppender.GetCurrentProcessId().ToString());
      foreach (var key in AppenderVariables.Keys)
      {
        var oldValue = "$(" + key + ")";
        fileName = fileName.Replace(oldValue, AppenderVariables[key]);
      }
      if (System.IO.File.Exists(fileName))
        fileName = this.GetTimedFileName(fileName);
      base.OpenFile(fileName, append);
    }

    private string GetTimedFileName(string fileName)
    {
      var num = fileName.LastIndexOf('.');
      if (num < 0)
        return fileName;
      return fileName.Substring(0, num) + (object) '.' + this.m_currentDate.ToString("HHmmss") + fileName.Substring(num);
    }

    private bool IsLocked(string fileName)
    {
      if (!System.IO.File.Exists(fileName))
        return false;
      try
      {
        var fileStream = System.IO.File.OpenWrite(fileName);
        fileStream.Close();
        return false;
      }
      catch (Exception ex)
      {
        var message = ex.Message;
        return true;
      }
    }

    public static string MakePath(string part1, string part2, char separator)
    {
      if (string.IsNullOrEmpty(part1))
      {
        return part2 ?? string.Empty;
      }
      if (string.IsNullOrEmpty(part2))
      {
        return part1;
      }
      if (part1[part1.Length - 1] == separator)
        part1 = part1.Substring(0, part1.Length - 1);
      if (part2[0] == separator)
        part2 = part2.Substring(1);
      return part1 + (object) separator + part2;
    }

    private string MapPath(string fileName)
    {
      if (fileName == "" || fileName.IndexOf(":/", StringComparison.Ordinal) >= 0 || fileName.IndexOf("://", StringComparison.Ordinal) >= 0)
        return fileName;
      int index = fileName.IndexOfAny(new char[2]
      {
        '\\',
        '/'
      });
      if (index >= 0 && (int) fileName[index] == 92)
        return fileName.Replace('/', '\\');
      fileName = fileName.Replace('\\', '/');
      if (HttpContext.Current != null)
        return HttpContext.Current.Server.MapPath(fileName);
      if (fileName[0] == '/')
        return MakePath(State.HttpRuntime.AppDomainAppPath, fileName.Replace('/', '\\'), '\\');
      return fileName;
    }

    public void SetVariable(string key, string value)
    {
      if (!string.IsNullOrEmpty(value))
        AppenderVariables[key] = value;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern int GetCurrentProcessId();
  }
}