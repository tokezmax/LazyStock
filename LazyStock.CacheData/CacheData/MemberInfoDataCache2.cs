using System;
using System.Collections.Generic;
using System.Data;
using Common.CacheObject;
using Common.DataAccess;
using Common.Tools;
using LazyStock.CacheData.Model;

namespace LazyStock.CacheData
{
    public class MemberInfoDataCache2 : List<MemberInfoModel>
    {
        private MemberInfoModel _defaultData = new MemberInfoModel { Id = 0, Email = "", UserName = "", PicUrl = "", LineId = "", Permission = "1" };


        public MemberInfoDataCache2()
        {
            this.GetAllData();
        }

        /// <summary>
        /// 重新取得資料，並判斷若尚未建立ObjectCache則自動新增
        /// </summary>
        /// <returns></returns>
        public virtual List<MemberInfoModel> GetAllData()
        {
            if (!CacheItems.Exists("MemberInfoList"))
                CreateSmsAppInfo();
            return CacheItems.Get<List<MemberInfoModel>>("MemberInfoList");
        }

        /// <summary>
        /// 重新取得SmsPlatformAppInfo資料並重新建立ObjectCache
        /// </summary>
        public virtual void CreateData()
        {
            CreateSmsAppInfo();
        }

        #region void CreateSmsAppInfo()
        /// <summary>
        /// 設定簡訊應用程式列表資料
        /// </summary>
        void CreateSmsAppInfo()
        {
            try
            {
                DataTable dt = Dao.SelectUSP("dbo.uspSmsPlatformAppInfoList", null, null, Setting.ConnectionString("SmsPlatform"));
                List<SmsPlatformAppInfoModel> smsAppInfoDataList = new List<SmsPlatformAppInfoModel> { };
                List<SmsPlatformAppSmsCorpModel> smsPlatformAppSmsCorpList = GetValidSmsAppSmsCorp();
                if (dt != null)
                {
                    foreach (DataRow cd in dt.Rows)
                    {
                        smsAppInfoDataList.Add(new SmsPlatformAppInfoModel
                        {
                            Id = Convert.ToInt32(cd["Id"].ToString()),
                            ApplicationName = cd["ApplicationName"].ToString(),
                            DepartmentId = Convert.ToInt32(cd["DepartmentId"].ToString()),
                            ApplicationSn = new Guid(cd["ApplicationSn"].ToString()),
                            IsValid = Convert.ToBoolean(cd["Valid"]),
                            IsTest = Convert.ToBoolean(cd["IsTest"]),
                            SentCount = Convert.ToInt32(cd["Count"].ToString()),
                            CreatorId = Convert.ToInt32(cd["CreatorId"].ToString()),
                            CreateOn = Convert.ToDateTime(cd["CreateOn"]).ToString("yyyy/MM/dd HH:mm:ss"),
                            LastEditorId = cd["LastEditorId"] == DBNull.Value ? new int?() : Convert.ToInt32(cd["LastEditorId"]),
                            LastEditOn = cd["LastEditorId"] == DBNull.Value ? "" : Convert.ToDateTime(cd["LastEditOn"]).ToString("yyyy/MM/dd HH:mm:ss"),
                            AppSmsCorps = smsPlatformAppSmsCorpList.FindAll(p => p.AppInfoId == Convert.ToInt32(cd["Id"].ToString())),
                            ActivityCount = Convert.ToInt32(cd["ActivityCount"].ToString()),
                            ActivityEndTime = cd["ActivityEndTime"] == DBNull.Value ? "" : Convert.ToDateTime(cd["ActivityEndTime"]).ToString("yyyy/MM/dd HH:mm:ss"),
                            ActivityUpperLimit = cd["ActivityUpperLimit"] == DBNull.Value ? new int?() : Convert.ToInt32(cd["ActivityUpperLimit"]),
                            Reason = cd["Reason"].ToString()
                        });
                    }
                }
                else
                    smsAppInfoDataList.Add(_defaultData);

                DateTimeOffset policyDay = new DateTimeOffset(DateTime.Now.AddDays(Convert.ToDouble((Setting.GetConfig("CacheItemPolicy", "AbsoluteExpiration", "SmsPlatformAppInfo")))));
                CacheItems.Add(smsAppInfoDataList, "SmsAppInfoList", policyDay, true);
            }
            catch (Exception ex)
            {
                throw new Exception("[SmsPlatformAppInfoDataCache.CreateSmsAppInfo]Error," + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 由appSn取得指定已啟用的程式資料
        /// </summary>
        /// <param name="appSn"></param>
        /// <returns></returns>
        public virtual MemberInfoModel GetMemberInfo(int? Id)
        {
            if (Id == null)
                return null;
            else
                return this.GetAllData().Find(p => p.Id == Id);
        }

    }
}
