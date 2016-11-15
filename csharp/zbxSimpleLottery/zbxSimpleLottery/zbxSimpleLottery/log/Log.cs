using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*********************************************************
 * 开发人员：王可
 * 创建时间：2016-11-10 10:59:32
 * 最近更新：$updateTime$
 * 描述说明：
 * 
 * 
 *
 * *******************************************************/
using log4net;
namespace zbxSimpleLottery.log
{
    public class Log
    {
       // private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  
        private static readonly log4net.ILog LogInfo = log4net.LogManager.GetLogger("loginfo");

        private static readonly log4net.ILog LogError = log4net.LogManager.GetLogger("logerror");

        private static readonly log4net.ILog LogWarning = log4net.LogManager.GetLogger("logwarn");

        private static readonly log4net.ILog LogDebug = log4net.LogManager.GetLogger("logdebug");

        /// <summary>
        /// 写入操作日志
        /// </summary>
        /// <param name="info">操作信息</param>
        public static void Info(string info)
        {
            if (LogInfo.IsInfoEnabled)
            {
                LogInfo.Info(info);
            }
        }
        /// <summary>
        /// 写入调试日志
        /// </summary>
        /// <param name="info">日志内容</param>
        /// <param name="ex">异常信息</param>
        public static void Debug(string info, Exception ex)
        {
            LogDebug.Debug("调试：" + info, ex);
        }
        /// <summary>
        /// 写入警告日志
        /// </summary>
        /// <param name="info">日志内容</param>
        /// <param name="ex">异常信息</param>
        public static void Warn(string info, Exception ex)
        {
            LogWarning.Warn("警告:" + info, ex);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="info">日志内容</param>
        /// <param name="ex">异常信息</param>
        public static void Error(string info, Exception ex)
        {
            LogError.Error("错误:"+info, ex);
        }

        //public static void Info(string str,Exception ex=null) {
        //    log.Info(str, ex);
        //}
        //public static void Error(string str, Exception ex = null)
        //{
        //    log.Error(str, ex);
        //}
        //public static void Debug(string str, Exception ex = null)
        //{
        //    log.Debug(str, ex);
        //}
        //public static void Warn(string str, Exception ex = null)
        //{
        //    log.Warn(str, ex);
        //}
    }
}
