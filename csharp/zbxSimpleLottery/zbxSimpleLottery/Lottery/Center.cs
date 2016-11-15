using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBContact;
using Newtonsoft.Json;

namespace Lottery
{
    /// <summary>
    /// 抽奖活动中心
    /// </summary>
    public static class lotteryCenter
    {
        private static List<ActivityDic> activityList = new List<ActivityDic>();
        /// <summary>
        /// 创建/添加抽奖活动
        /// </summary>
        /// <returns></returns>
        public static bool Create(int id, Action<Exception> error, Action<Exception> errorHandle)
        {
            try
            {
                Delete(id, error);
                var activity = new Activity(id, error);
                if (activity != null&&activity.IsInit)
                {
                    var timer = new ActivityTimer(activity, error,errorHandle);
                    if (timer != null && timer.IsInit)
                    {
                        if (timer.Test(error))
                        {
                            activityList.Add(new ActivityDic() { Id = id, Timer = timer });
                            return true;
                        }
                        else
                        {
                            error(new Exception("活动测试启动失败！"));
                        }
                    }
                    else
                    {
                        error(new Exception("活动实例化失败！"));
                    }
                }
                else
                {
                    error(new Exception("数据转化失败！"));
                }
            }
            catch (Exception ex) { error(ex); }
            return false;
        }
        /// <summary>
        /// 更新活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Update(int id, Action<Exception> error, Action<Exception> errorHandle)
        {
           return Create(id, error, errorHandle);
        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="id"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Delete(int id, Action<Exception> error)
        {
            return activityList.RemoveAll(p=>p.Id==id)>0;
        }
        /// <summary>
        /// 抽奖方法
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="activeId"></param>
        /// <returns></returns>
        public static PrizeRate Lottery(int uid, int activeId)
        {
            if (activityList == null && activityList.Count() == 0) return null;
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.LotteryMethod.LotteryFunc();
            }
            return null;
        }
        /// <summary>
        /// 查看当前是否还有奖品
        /// </summary>
        /// <param name="activeId"></param>
        /// <returns></returns>
        public static bool HasPrize(int activeId)
        {
            if (activityList == null && activityList.Count() == 0) return false;
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.LotteryMethod.HasAllPrize;
            }
            return false;
        }
        /// <summary>
        /// 查看当前中奖记录(debug)
        /// </summary>
        /// <param name="activeId"></param>
        /// <returns></returns>
        public static string PrizeRecord(int activeId)
        {
            if (activityList == null && activityList.Count() == 0) return null;
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.LotteryMethod.PrizeRecordStr;
            }
            return null;
        }
        /// <summary>
        /// 奖项设置列表
        /// </summary>
        /// <param name="activeId"></param>
        /// <returns></returns>
        public static List<PrizeRate> PrizeList(int activeId)
        {
            if (activityList == null && activityList.Count() == 0) return new List<PrizeRate>();
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.LotteryMethod.Prizes;
            }
            return new List<PrizeRate>();
        }
        /// <summary>
        /// 查看当前奖品运行中参数列表（debug）
        /// </summary>
        /// <param name="activeId"></param>
        /// <returns></returns>
        public static List<paramRecord> Paramlist(int activeId)
        {
            if (activityList == null && activityList.Count() == 0) return new List<paramRecord>();
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.LotteryMethod.Paramlist;
            }
            return new List<paramRecord>();
        }
        /// <summary>
        /// 查看当前中奖历史记录（debug）
        /// </summary>
        /// <param name="activeId"></param>
        /// <returns></returns>
        public static List<PrizeRate> HisRecord(int activeId)
        {
            if (activityList == null && activityList.Count() == 0) return new List<PrizeRate>();
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.LotteryMethod.HisRecord;
            }
            return new List<PrizeRate>();
        }
        public static List<ActivityRateListDic> GetRateList(int activeId) {
            if (activityList == null && activityList.Count() == 0) return new List<ActivityRateListDic>();
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.Activity.RateDic;
            }
            return new List<ActivityRateListDic>();
        }
        public static ActivityRateListDic GetCurrentRate(int activeId) {
            if (activityList == null && activityList.Count() == 0) return new ActivityRateListDic();
            var activity = activityList.Where(p => p.Id == activeId).FirstOrDefault();
            if (activity != null && activity.Timer != null && activity.Timer.LotteryMethod != null)
            {
                return activity.Timer.Current;
            }
            return new ActivityRateListDic();
        }
    }
    /// <summary>
    /// 数据库基础数据类
    /// </summary>
    internal class Activity
    {
        public Activity(int id, Action<Exception> error)
        {
            var currentError = new Action<Exception>(err =>
            {
                this.IsInit = false;
                error(err);
            });
            try
            {
                using (var db = new LotteryEntities())
                {
                    Active = db.Shop_LuckDrawInfo.Where(p => p.ID == id&&p.State==1).FirstOrDefault();
                    Awards = db.Shop_LuckAward.Where(p => p.LuckID == id && p.LuckRate > 0 && p.LuckRate <= 1&&p.StockBalance>0&&p.ProductCount>=p.StockBalance).ToList();
                    if (Active != null && Active.StartTime < Active.EndTime && Active.EndTime>DateTime.Now.AddMinutes(2) && Awards != null && Awards.Count() > 0)
                    {
                        int[] ints= Awards.Select(a => a.ID).ToArray();
                        Stages = db.Shop_LuckAwardStage.Where(p => p.LuckID == id && p.StageTime > Active.StartTime && p.StageTime < Active.EndTime && ints.Contains(p.AwardID)).ToList();
                        Conv2RateList(currentError);
                    }
                    else
                    {
                        currentError(new Exception("数据缺失！或活动未激活！"));
                    }
                }
            }
            catch (Exception ex) { currentError(ex); }
            this.IsInit = true;
        }
        public Activity(Action<Exception> error, Shop_LuckDrawInfo act, List<Shop_LuckAward> awards = null, List<Shop_LuckAwardStage> stages = null)
        {
            var currentError = new Action<Exception>(err =>
            {
                this.IsInit = false;
                error(err);
            });
            try
            {
                if (act == null) currentError(new Exception("null"));
                Active = act;
                using (var db = new LotteryEntities())
                {
                    Awards = awards == null ? db.Shop_LuckAward.Where(p => p.LuckID == Active.ID && p.LuckRate > 0 && p.LuckRate <= 1 && p.StockBalance > 0 && p.ProductCount >= p.StockBalance).ToList() : awards.Where(p => p.LuckID == Active.ID && p.LuckRate > 0 && p.LuckRate <= 1 && p.StockBalance > 0 && p.ProductCount >= p.StockBalance).ToList();
                    if (Active != null && Active.StartTime < Active.EndTime && Active.EndTime > DateTime.Now.AddMinutes(2) && Awards != null && Awards.Count() > 0)
                    {
                        Stages = stages == null ? db.Shop_LuckAwardStage.Where(p => p.LuckID == act.ID && p.StageTime > Active.StartTime && p.StageTime < Active.EndTime && Awards.Select(a => a.ID).Contains(p.AwardID)).ToList() : stages.Where(p => p.LuckID == act.ID && p.StageTime > Active.StartTime && p.StageTime < Active.EndTime && Awards.Select(a => a.ID).Contains(p.AwardID)).ToList();
                        Conv2RateList(currentError);
                    }
                    else
                    {
                        currentError(new Exception("数据缺失！"));
                        return;
                    }
                }
            }
            catch (Exception ex) { currentError(ex); return; }
            this.IsInit = true;
        }
        public bool IsInit { get; set; }
        public Shop_LuckDrawInfo Active { get; set; }
        public List<Shop_LuckAward> Awards { get; set; }
        public List<Shop_LuckAwardStage> Stages { get; set; }
        public List<ActivityRateListDic> RateDic { get; set; }
        private void Conv2RateList(Action<Exception> error)
        {
            try
            {
                RateDic = new List<ActivityRateListDic>();
                DateTime _startTime = Active.StartTime, _endTime = Active.EndTime, _nowTime = DateTime.Now.AddMinutes(1);
                bool _hasStage = Stages == null ? false : Stages.Count() == 0 ? false : true;
                if (_hasStage)
                {
                    for (var i = 0; i < Stages.Count(); i++)
                    {
                        Stages[i].StageTime = Convert.ToDateTime(Stages[i].StageTime.ToString("yyyy/MM/dd hh:mm"));
                    }
                    _startTime = Convert.ToDateTime(_startTime.ToString("yyyy/MM/dd hh:mm"));
                    _endTime = Convert.ToDateTime(_endTime.ToString("yyyy/MM/dd hh:mm"));
                    var _AwardList = ToolMethods.Conv2RateFromAward(Awards);
                    if (_AwardList == null || _AwardList.Count() <= 0)  error(new Exception("_AwardList 转化失败！"));
                    var _dtList = Stages.Select(p => p.StageTime).ToList();
                    _dtList.Add(_startTime);
                    _dtList = _dtList.Distinct().OrderBy(p => p).ToList();
                    int length=_dtList.Count();
                    var _maxlenth = (_endTime - _startTime).Ticks;
                    List<TempClass> temp1=new List<TempClass>();
                    for (var i = 0; i < length; i++) {
                        if (i < length - 1)
                        {
                            temp1.Add(new TempClass() { time = _dtList[i], ticks = (_dtList[i + 1] - _dtList[i]).Ticks });
                        }
                        else {
                            temp1.Add(new TempClass() { time = _dtList[i],ticks = (_endTime - _dtList[i]).Ticks });
                        }
                    }

                    List<TempClass> tempDic = new List<TempClass>();

                    _AwardList.ForEach(award =>
                    {
                        if (award.IsDefaultPrize)
                        {
                             for (var i = 0; i < length; i++)
                             {
                                 tempDic.Add(new TempClass() { prizeId = award.Id, time = _dtList[i], startCount = award.PrizeCount, useCount = award.PrizeCount, remainCount = award.PrizeCount, currentCount = award.PrizeCount });
                             }
                        }
                        else
                        {
                            int tempuseCount = 0;
                            for (var i = 0; i < length; i++)
                            {
                                long tempticks = 0;
                                TempClass temp1current = null;
                                Shop_LuckAwardStage tempControlEnd = null;
                                int tempremainCount = 0, tempcurrentCount = 0; 
                                if (i == 0)
                                {
                                    tempControlEnd = Stages.Where(p => p.StageTime > _dtList[i] && p.AwardID == award.Id).OrderBy(p=>p.StageTime).FirstOrDefault();
                                    temp1current = temp1.Where(p => p.time == _dtList[i]).FirstOrDefault();
                                    tempremainCount = tempControlEnd != null ? tempControlEnd.StageCount : award.PrizeCount;
                                    if (award.PrizeCount - tempuseCount < tempremainCount)
                                        tempremainCount = award.PrizeCount - tempuseCount;
                                    tempticks=((tempControlEnd != null ? tempControlEnd.StageTime : _endTime) - _startTime).Ticks;
                                    tempcurrentCount = Convert.ToInt32(Math.Floor(temp1current.ticks * (double)tempremainCount / tempticks));
                                }
                                else
                                {
                                    tempControlEnd = Stages.Where(p => p.StageTime > _dtList[i] && p.AwardID == award.Id).OrderBy(p=>p.StageTime).FirstOrDefault();
                                    temp1current = temp1.Where(p => p.time == _dtList[i]).FirstOrDefault();
                                    tempremainCount = (tempControlEnd != null ? tempControlEnd.StageCount : award.PrizeCount) - tempDic[i - 1 + _AwardList.IndexOf(award)*length].useCount;
                                    if (award.PrizeCount - tempuseCount < tempremainCount)
                                        tempremainCount = award.PrizeCount - tempuseCount;
                                    tempticks =((tempControlEnd != null ? tempControlEnd.StageTime : _endTime) - _dtList[i]).Ticks;
                                    tempcurrentCount = Convert.ToInt32(Math.Floor(temp1current.ticks * (double)tempremainCount / tempticks));
                                }
                                if (tempremainCount < tempcurrentCount)
                                {
                                    tempcurrentCount = tempremainCount;
                                }
    
                                tempuseCount += tempcurrentCount;
                                tempDic.Add(new TempClass() { prizeId = award.Id, time = _dtList[i], startCount = award.PrizeCount, useCount = tempuseCount, remainCount = tempremainCount, currentCount = tempcurrentCount });
                            }
                        }
                    });

                    _dtList.ForEach(start =>
                    {
                        var list = new List<PrizeRate>();
                        var temp2=tempDic.Where(p => p.time == start).ToList();
                        temp2.ForEach(item => {
                            PrizeRate rate = _AwardList.Where(p => p.Id == item.prizeId).FirstOrDefault().Clone();
                            rate.PrizeCount = item.currentCount<0?0:item.currentCount;
                            rate.RemainCount = item.currentCount<0?0:item.currentCount;
                            list.Add(rate);
                        });
                        if (list != null && list.Count() > 0)
                        {
                            if (start < _nowTime)
                                RateDic.Add(new ActivityRateListDic() { Id = _dtList.IndexOf(start), Start = _nowTime, RateList = list });
                            else
                                RateDic.Add(new ActivityRateListDic() { Id = _dtList.IndexOf(start), Start = start, RateList = list });
                        }
                        else {
                            error(new Exception("ActivityRateListDic null"));
                        }
                    });
                }
                else
                {
                    var list = ToolMethods.Conv2RateFromAward(Awards);
                    if (list != null && list.Count() > 0)
                    {
                        if (_startTime < _nowTime)
                            RateDic.Add(new ActivityRateListDic() { Id = 0, Start = _nowTime, RateList = list });
                        else
                            RateDic.Add(new ActivityRateListDic() { Id = 0, Start = _startTime, RateList = list });
                    }
                    else {
                        error(new Exception("ActivityRateListDic null"));
                    }
                }
            }
            catch (Exception ex) { error(ex); }
        }
        private class TempClass {
            public DateTime time { get; set; }
            public long ticks { get; set; }
            public int startCount { get; set; }
            public int useCount { get; set; }
            public int remainCount { get; set; }
            public int currentCount { get; set; }
            public int prizeId { get; set; }
        }
    }
    internal class ActivityDic
    {
        public int Id { get; set; }
        public ActivityTimer Timer { get; set; }
    }
    public class ActivityRateListDic {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public List<PrizeRate> RateList { get; set; }
    }

    /// <summary>
    /// 活动定时类
    /// </summary>
    internal class ActivityTimer
    {
        public ActivityTimer(Activity act, Action<Exception> error, Action<Exception> errorHand)
        {
            this.IsInit = false;
            if (error == null) return;
            if (act == null || act.Active == null || act.Awards == null || act.Awards.Count() <= 0||act.RateDic==null||act.RateDic.Count()<=0)
            {
                this.IsInit = false;
                error(new Exception("null"));
                return;
            }
            activity = act;
            errorHandle = errorHand;
            this.IsInit = true;
        }
        public bool IsInit { get; set; }
        private Action<Exception> errorHandle { get; set; }
        private Activity activity { get; set; }
        private System.Timers.Timer startTimer = null;
        private System.Timers.Timer endTimer = null;
        public LotteryMethods LotteryMethod = null;
        private int currentId = -1;
        public ActivityRateListDic Current { get { return activity.RateDic.Find(p => p.Id == currentId); } }
        public Activity Activity { get { return activity; } }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public bool Test(Action<Exception> error)
        {
            try
            {
                bool bl = true;
                activity.RateDic.ForEach(change =>
                {
                    if (change.RateList.Find(p => p.IsDefaultPrize) == null) bl = false;
                    LotteryMethod = new LotteryMethods(new Action<Exception>(err => {
                        error(err);
                        bl = false;
                    }), new Func<List<PrizeRate>>(() =>
                    {
                        return activity.RateDic.Where(p => p.Id == change.Id).FirstOrDefault().RateList;
                    }));
                });
                LotteryMethod = null;
                if (bl) Init();
                return bl;
            }
            catch (Exception ex) { error(ex); }
            return false;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        private void Init()
        {
            try
            {
                var nowTime=DateTime.Now;
                var temp=activity.RateDic.Where(p=>p.Start<nowTime).LastOrDefault();
                currentId = temp == null ? -1 : temp.Id-1;
                activity.RateDic.RemoveAll(p=>p.Start<nowTime&&p.Id!=temp.Id);

                DateTime time = activity.Active.StartTime < nowTime ? nowTime : activity.Active.StartTime;

                var timediff = time.Subtract(nowTime).TotalMilliseconds;
                if (DateTime.Now < time && timediff > 0)
                {
                    startTimer = new System.Timers.Timer(timediff);
                }
                else
                {
                    startTimer = new System.Timers.Timer(1000);
                }
                startTimer.Elapsed += new System.Timers.ElapsedEventHandler(Start);
                startTimer.Enabled = true;
                startTimer.AutoReset = false;

               
            }
            catch (Exception ex) { errorHandle(ex); }
        }

        /// <summary>
        /// 触发修改抽奖参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                currentId++;

                //抽奖参数等数据源重置
                LotteryMethod = new LotteryMethods(errorHandle, new Func<List<PrizeRate>>(() =>
                  {
                      return activity.RateDic.Where(p => p.Id == currentId).FirstOrDefault().RateList;
                  }));

                var nowTime = DateTime.Now;
                var nextTime = this.activity.RateDic.Where(p => p.Id == currentId + 1).FirstOrDefault();
                if (nextTime != null)
                {
                    var timediff = (nextTime.Start < nowTime ? nowTime : nextTime.Start).Subtract(nowTime).TotalMilliseconds;
                    startTimer.Interval = timediff;
                }
                else
                {
                    startTimer.Stop();
                    var time = activity.Active.EndTime;

                    var timediff = time.Subtract(nowTime).TotalMilliseconds;
                    if (nowTime < time && timediff > 0)
                    {
                        endTimer = new System.Timers.Timer(timediff);
                    }
                    else
                    {
                        endTimer = new System.Timers.Timer(1000);
                    }
                    endTimer.Elapsed += new System.Timers.ElapsedEventHandler(End);
                    endTimer.Enabled = true;
                    endTimer.AutoReset = false;
                }
            }
            catch (Exception ex) { errorHandle(ex); }
        }

        /// <summary>
        /// 触发关闭timer,清空lotteryMethod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void End(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (startTimer != null)
                    startTimer.Stop();
                if (endTimer != null)
                    endTimer.Stop();
                LotteryMethod = null;
            }
            catch (Exception ex) { errorHandle(ex); }
        }

    }
    /// <summary>
    /// 抽奖主方法类
    /// </summary>
    public class LotteryMethods
    {
        private List<PrizeRate> prizes = new List<PrizeRate>();
        private List<paramRecord> paramlist = new List<paramRecord>();
        private List<PrizeRate> hisRecord = new List<PrizeRate>();
        private PrizeRate selectedElement = null;
        private PrizeRate emptyPrize = null;
        private Random random = new Random();
        private long basicNumber = 0;
        private double allRate;
        private bool hasPrize = false;
        private bool hasAllPrize = false;

        /// <summary>
        /// 中奖字符串（debug）
        /// </summary>
        public string PrizeRecordStr { get; set; }
        /// <summary>
        /// 是否有奖
        /// </summary>
        public bool HasAllPrize
        {
            get
            {
                return hasAllPrize;
            }
        }
        /// <summary>
        /// 奖项设置列表
        /// </summary>
        public List<PrizeRate> Prizes
        {
            get
            {
                return prizes;
            }
        }
        /// <summary>
        /// 奖品运行中参数列表（debug）
        /// </summary>
        public List<paramRecord> Paramlist
        {
            get
            {
                return paramlist;
            }
        }
        /// <summary>
        /// 中奖历史记录（debug）
        /// </summary>
        public List<PrizeRate> HisRecord
        {
            get
            {
                return hisRecord;
            }
        }
        /// <summary>
        /// 实例化抽奖参数
        /// </summary>
        /// <param name="errorHandle"></param>
        /// <param name="prizeListHandleFunc"></param>
        public LotteryMethods(Action<Exception> errorHandle, Func<List<PrizeRate>> prizeListHandleFunc = null)
        {
            try
            {
                if (prizeListHandleFunc == null)
                {
                    //获取活动奖品列表
                    GetPrizeList();
                }
                else {
                    prizes = prizeListHandleFunc();
                }
                emptyPrize = prizes.Where(p => p.IsDefaultPrize).OrderByDescending(p => p.Rate).FirstOrDefault();

                //求出概率基数  
                double[] array = prizes.Select(p => p.Rate).ToArray();
                basicNumber = ToolMethods.GetBaseNumber(array);

                random = new Random();

                //判断设置的概率  
                allRate = prizes.Sum(p => p.Rate);
                if (allRate != 1)
                {
                    errorHandle(new Exception("奖品概率设置错误!" + allRate + "\r\n"));
                }
                else if (emptyPrize == null)
                {
                    errorHandle(new Exception("奖项设置中没有设置默认奖!\r\n"));
                }
                else
                {
                    hasPrize = true;
                    hasAllPrize = true;
                }
            }
            catch (Exception ex)
            {
                errorHandle(ex);
            }
        }
        /// <summary>
        /// 测试用数据
        /// </summary>
        public void GetPrizeList()
        {
            prizes.Add(new PrizeRate() { Id = 1, PrizeId = 1, PrizeName = "奖品1", Rate = 0.01, ActivityId = 1, PrizeCount = 5, RemainCount = 5, IsDefaultPrize = false });
            prizes.Add(new PrizeRate() { Id = 2, PrizeId = 2, PrizeName = "奖品2", Rate = 0.02, ActivityId = 1, PrizeCount = 10, RemainCount = 10, IsDefaultPrize = false });
            prizes.Add(new PrizeRate() { Id = 3, PrizeId = 3, PrizeName = "奖品3", Rate = 0.03, ActivityId = 1, PrizeCount = 15, RemainCount = 15, IsDefaultPrize = false });
            prizes.Add(new PrizeRate() { Id = 4, PrizeId = 4, PrizeName = "奖品4", Rate = 0.04, ActivityId = 1, PrizeCount = 20, RemainCount = 20, IsDefaultPrize = false });
            prizes.Add(new PrizeRate() { Id = 5, PrizeId = 5, PrizeName = "空奖5", Rate = 0.90, ActivityId = 1, PrizeCount = int.MaxValue, RemainCount = int.MaxValue, IsDefaultPrize = true });
        }
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="isDebug"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public PrizeRate LotteryFunc(bool isDebug = true, EmptyHandle handle = EmptyHandle.MoveToNothing)
        {
            try
            {
                selectedElement = null;
                //抽奖  
                if (hasAllPrize)
                {
                    long diceRoll = ToolMethods.GetRandomNumber(random, 1, basicNumber);
                    long cumulative = 0;
                    int prizeLength = prizes.Count();

                    for (int i = 0; i < prizeLength; i++)
                    {
                        cumulative += (long)(prizes[i].Rate * basicNumber);
                        if (isDebug)
                        {
                            paramRecord r = new paramRecord()
                            {
                                basicNumber = basicNumber.ToString(),
                                diceRoll = diceRoll.ToString(),
                                item = "id:" + prizes[i].Id,
                                prizesRate = prizes[i].Rate.ToString(),
                                cumulative = cumulative.ToString(),
                                select = diceRoll <= cumulative
                            };
                            paramlist.Add(r);
                        }
                        if (diceRoll != 0 && diceRoll <= cumulative)
                        {
                            selectedElement = prizes[i];
                            prizes[i].RemainCount--;
                            if (prizes[i].RemainCount <= 0)
                            {
                                prizes.RemoveAt(i);
                                if (prizes.Count() > 0)
                                {
                                    allRate = prizes.Sum(p => p.Rate);
                                    if (allRate != 1)
                                    {
                                        var tempList = prizes.Where(a => a.Rate > 0).ToList();
                                        switch (handle)
                                        {
                                            case EmptyHandle.Avg:
                                                if (tempList.Count() == 1)
                                                {
                                                    tempList.FirstOrDefault().Rate = 1;
                                                }
                                                else
                                                {
                                                    tempList.ForEach(p =>
                                                    {
                                                        p.Rate += (1.00 - allRate) / tempList.Count();
                                                    });
                                                }
                                                break;
                                            case EmptyHandle.Rate:
                                                if (tempList.Count() == 1)
                                                {
                                                    tempList.FirstOrDefault().Rate = 1;
                                                }
                                                else
                                                {
                                                    tempList.ForEach(p =>
                                                    {
                                                        p.Rate += (1.00 - allRate) * p.Rate / tempList.Count() / allRate;
                                                    });
                                                }
                                                break;
                                            case EmptyHandle.MoveToNothing:
                                            default:
                                                if (emptyPrize != null)
                                                    emptyPrize.Rate += 1 - allRate;
                                                break;
                                        }
                                    }
                                    basicNumber = ToolMethods.GetBaseNumber(prizes.Select(p => p.Rate).ToArray());
                                }
                                else
                                {
                                    hasAllPrize = false;
                                }
                                hasPrize = false;
                            }
                            break;
                        }
                    }
                    if (selectedElement == null)
                        throw new Exception("empty");
                    if (isDebug)
                        PrizeRecordStr += selectedElement.PrizeName + "\n";
                    hisRecord.Add(selectedElement);
                    if (!hasPrize && isDebug)
                        PrizeRecordStr += selectedElement.PrizeName + "已空！\n";
                    hasPrize = true;
                }
                else if (emptyPrize != null)
                {
                    if (isDebug)
                        hisRecord.Add(emptyPrize);
                    selectedElement = emptyPrize.Clone();
                }
                else
                {
                    if (isDebug)
                        PrizeRecordStr += "emptyPrize is null!";
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (emptyPrize != null)
                {
                    if (isDebug)
                    {
                        PrizeRecordStr += emptyPrize.PrizeName + "\n";
                        hisRecord.Add(emptyPrize);
                    }
                    selectedElement = emptyPrize.Clone();
                }
                else
                {
                    if (isDebug)
                        PrizeRecordStr += "emptyPrize is null!";
                    return null;
                }
            }
            return selectedElement;
        }

    }

    public static class ToolMethods
    {
        public static List<PrizeRate> Conv2RateFromAward(List<Shop_LuckAward> awards) {
            var list = new List<PrizeRate>();
            awards.ForEach(award => {
                PrizeRate rate = new PrizeRate()
                {
                    ActivityId = award.LuckID,
                    Id = award.ID,
                    IsDefaultPrize = award.isEmpty == 1,
                    PrizeId = award.ProductID,
                    PrizeName = award.ProductName,
                    RemainCount = award.StockBalance,
                    PrizeCount = award.StockBalance,
                    Rate =Convert.ToDouble(award.LuckRate)
                };
                list.Add(rate);
            });
            return list;
        }

        /// <summary>  
        /// 获取概率的基数  
        /// </summary>  
        /// <param name="array"></param>  
        /// <returns></returns>  
        public static long GetBaseNumber(double[] array)
        {
            long result = 0;


            try
            {
                if (array == null || array.Length == 0)
                {
                    return result;
                }


                string targetNumber = string.Empty;


                foreach (double item in array)
                {
                    string temp = item.ToString();


                    if (!temp.Contains('.'))
                    {
                        continue;
                    }


                    temp = temp.Substring(temp.IndexOf('.')).Replace(".", "");


                    if (targetNumber.Length < temp.Length)
                    {
                        targetNumber = temp;
                    }
                }


                if (!string.IsNullOrEmpty(targetNumber))
                {
                    int ep = targetNumber.Length;


                    result = (long)Math.Pow(10, ep);
                }
            }
            catch { }


            return result;
        }

        /// <summary>  
        /// 获取随机数  
        /// </summary>  
        /// <param name="random"></param>  
        /// <param name="min"></param>  
        /// <param name="max"></param>  
        /// <returns></returns>  
        public static long GetRandomNumber(this Random random, long min, long max)
        {
            byte[] minArr = BitConverter.GetBytes(min);


            int hMin = BitConverter.ToInt32(minArr, 4);


            int lMin = BitConverter.ToInt32(new byte[] { minArr[0], minArr[1], minArr[2], minArr[3] }, 0);


            byte[] maxArr = BitConverter.GetBytes(max);


            int hMax = BitConverter.ToInt32(maxArr, 4);


            int lMax = BitConverter.ToInt32(new byte[] { maxArr[0], maxArr[1], maxArr[2], maxArr[3] }, 0);


            if (random == null)
            {
                random = new Random();
            }


            int h = random.Next(hMin, hMax);


            int l = 0;


            if (h == hMin)
            {
                l = random.Next(Math.Min(lMin, lMax), Math.Max(lMin, lMax));
            }
            else
            {
                l = random.Next(0, Int32.MaxValue);
            }


            byte[] lArr = BitConverter.GetBytes(l);


            byte[] hArr = BitConverter.GetBytes(h);


            byte[] result = new byte[8];


            for (int i = 0; i < lArr.Length; i++)
            {
                result[i] = lArr[i];
                result[i + 4] = hArr[i];
            }


            return BitConverter.ToInt64(result, 0);
        }
    }

    /// <summary>
    /// 空奖处理枚举
    /// </summary>
    public enum EmptyHandle
    {
        /// <summary>
        /// 平分中率
        /// </summary>
        Avg = 0,
        /// <summary>
        /// 移给默认
        /// </summary>
        MoveToNothing = 1,
        /// <summary>
        /// 按初始比例分
        /// </summary>
        Rate = 2
    }
    /// <summary>
    /// 运行中的关键参数记录类
    /// </summary>
    public class paramRecord
    {
        public string item { get; set; }
        public string diceRoll { get; set; }
        public string basicNumber { get; set; }
        public string prizesRate { get; set; }
        public string cumulative { get; set; }
        public bool select { get; set; }
    }
    /// <summary>
    /// 运行中的奖品类
    /// </summary>
    [Serializable]
    public class PrizeRate
    {
        public PrizeRate()
        {
        }
        public PrizeRate(Shop_LuckAward award)
        {
            this.Id = award.ID;
            this.Rate = Convert.ToDouble(award.LuckRate);
            this.PrizeCount = award.ProductCount;
            this.RemainCount = award.StockBalance;
            this.IsDefaultPrize = award.isEmpty == 1;
            this.PrizeId = award.ProductID;
            this.ActivityId = award.LuckID;
            this.PrizeName = award.ProductName;
        }
        public int ActivityId { get; set; }
        public int Id { get; set; }
        public double Rate { get; set; }
        public int PrizeId { get; set; }
        public string PrizeName { get; set; }
        public int PrizeCount { get; set; }
        public int RemainCount { get; set; }
        public bool IsDefaultPrize { get; set; }

        public PrizeRate Clone()
        {
            return JsonConvert.DeserializeObject<PrizeRate>(JsonConvert.SerializeObject(this));
        }
    }
}


