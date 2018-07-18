using System;
using System.IO;
using System.Net;
using System.Text;
using NLog;

namespace LazyStock.ScheduleServices.Services
{
    /// <summary>
    /// 所有數據爬蟲皆需要實作此類別，進行規範
    /// </summary>
    public abstract class BaseCrawlerServcies
    {
        internal static Logger _ErrorLogger = LogManager.GetLogger("CrawlerErrorLogger");

        /// <summary>
        /// 指定抓取日期
        /// </summary>
        /// <returns></returns>
        public abstract void Init();

        /// <summary>
        /// 確認是否完成
        /// </summary>
        /// <returns></returns>
        public abstract void CheckIsDone();

        /// <summary>
        /// 下載
        /// </summary>
        /// <returns></returns>
        public abstract void Download();

        /// <summary>
        /// 確認是否完成
        /// </summary>
        /// <returns></returns>
        public abstract void ImportDate();
        
        /// <summary>
        /// 上傳至AWS
        /// </summary>
        /// <returns></returns>
        protected abstract void UploadData();


        /// <summary>
        /// 更新LOCALDB
        /// </summary>
        /// <returns></returns>
        protected abstract void UpdateLocalDB();


        /// <summary>
        /// 取得呼叫簡訊商API的回傳結果(x-www-form-urlencoded)
        /// </summary>
        /// <param name="url">API URL路徑</param>
        /// <returns>簡訊商API的回傳結果</returns>
        internal string GetApiInvokeResult(string url, string method = "GET", string postData = null, string tranSn = "")
        {
            string returnValue = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                if (method.ToUpper() == "POST")
                {
                    var paramBytes = Encoding.UTF8.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = paramBytes.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(paramBytes, 0, paramBytes.Length);
                    }
                }

                request.KeepAlive = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            returnValue = sr.ReadToEnd();
                        }
                    }

                    //returnValue為NULL而且HttpStatusCode=200可能是連不到簡訊商API Server
                    if (returnValue == null)
                    {
                        string message = string.Format("[{2}]GetApiInvokeResult 呼叫API發生錯誤：API Url = {0}\nMessage：HttpStatusCode={1}\n", url, response.StatusCode, tranSn);
                        _ErrorLogger.Error(message);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("[{6}]GetApiInvokeResult 呼叫API發生錯誤：API Url = {0}\n{1}.{2}：{3}={4}\n{5}", url, ex.TargetSite.DeclaringType.Name, ex.TargetSite.Name, (ex.InnerException == null ? "Exception" : "InnerException"), (ex.InnerException == null ? ex.Message : ex.InnerException.Message), ex.StackTrace, tranSn);
                _ErrorLogger.Error(message);
            }

            return returnValue;
        }
    }
}