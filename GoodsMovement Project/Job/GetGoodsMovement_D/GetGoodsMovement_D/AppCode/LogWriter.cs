/*
 * 2008/03/24 Tom 增加 Mutex; 修改 Logger name
 * 2008/03/27 Tom 設定 ILog 初值 = null
 * 
 */ 

using log4net ;
using log4net.Config ;
using System ;
using System.Threading ;


public class LogWriter 
{
    private static ILog m_log = null;  // updated by Tom 2008/03/27
    private static Mutex mut = new Mutex(false,"Log4_IDL_Leaving_418HF");

    private static void init()
    {
        // BasicConfigurator.Configure()
        // DOMConfigurator.Configure(New System.IO.FileInfo("log.config.xml"))

        log4net.Config.XmlConfigurator.Configure();
        m_log = LogManager.GetLogger("Logger_Idl_Leave");

        // DOMConfigurator.Configure(); // DOMConfigurator is obsolete. Use XmlConfigurator instead of DOMConfigurator
    }


    public static void writeInfo(string vs_msg)
    {
        mut.WaitOne();

        if (m_log == null)
        {
            init() ;
        }                    
        m_log.Info(vs_msg)  ;
        mut.ReleaseMutex();

    }

    public static void  writeDebug(string vs_msg )
    {
        mut.WaitOne();
        if (m_log == null)
        {
            init();
        }
        m_log.Debug(vs_msg) ;
        mut.ReleaseMutex();
    }

    public static void  writeError(string vs_msg )
    {
        mut.WaitOne();
        if (m_log == null)
        {
            init();
        }
        m_log.Error(vs_msg) ;
        mut.ReleaseMutex();
    }

    public static void  writeEx( string vs_msg, Exception v_ex  )
    {

        mut.WaitOne();
        if (m_log == null)
        {
            init() ;
        }
        m_log.Error(vs_msg, v_ex) ;
        mut.ReleaseMutex();
    }

}


