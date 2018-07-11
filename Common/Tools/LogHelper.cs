using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;
using Common;

public sealed class LogHelper
{
    private static LogHelper _instance = new LogHelper();
    public static Mutex LogLock = new Mutex();
    public static Mutex BackLogLock = new Mutex();
    public static bool PrintToConsole = true;
    
    static LogHelper(){
        load();
    }

    public static void load() {
        //dirName.Clear();
    }

    public static String DNAME(String K) {
        //if (String.IsNullOrEmpty(K))
            return "UNDER";
        //if (!dirName.ContainsKey(K))
            //return "UNDER";
        //return dirName.Item(K);
    }


    /// <summary>
    /// 寫入log
    /// </summary>
    /// <param name="Forder">分類名稱</param>
    /// <param name="Message"></param>
    public static void doLog(string Forder,string Message){
        if (PrintToConsole) 
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "[" + Forder + "]=>" + Message);

        String FileName = AppDomain.CurrentDomain.BaseDirectory + @"\logs\\" + Forder + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        FileWrite(FileName, Message);
    }

    /// <summary>
    /// 寫入log
    /// </summary>
    /// <param name="Path">實體路徑</param>
    /// <param name="Forder">分類名稱</param>
    /// <param name="Message"></param>
    public static void doLog(string Path, string Forder, string Message)
    {
        String FileName = Path + "logs\\" + Forder + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        FileWrite(FileName, Message);
    }

    public static void FileWrite(string FilePath, string Message,bool timestemp = true)
    {
        try
        {
            LogLock.WaitOne();

            if (!Directory.Exists(Path.GetDirectoryName(FilePath))){
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            }

            if (!File.Exists(FilePath))
            {

                using (StreamWriter f = File.CreateText(FilePath))
                {
                    f.Flush();
                    f.Close();
                }
            }
            using (StreamWriter sw = new StreamWriter(FilePath, true))
            {
                if (timestemp)
                    Message = DateTime.Now.ToString("HH:mm:ss fff") + " : " + Message;

                sw.WriteLine(Message);
                sw.Flush();
                sw.Close();
            }
        }
        catch// (Exception ex)
        {
            
        }
        finally
        {
            LogLock.ReleaseMutex();
        }
    }
    public static void FileOverWrite(string FilePath, string Message, bool timestemp = true)
    {
        try
        {
            LogLock.WaitOne();

            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            }

            if (!File.Exists(FilePath))
            {

                using (StreamWriter f = File.CreateText(FilePath))
                {
                    f.Flush();
                    f.Close();
                }
            }

            System.IO.File.WriteAllText(FilePath, Message, Encoding.UTF8);
    
        }
        catch// (Exception ex)
        {

        }
        finally
        {
            LogLock.ReleaseMutex();
        }
    }
}

