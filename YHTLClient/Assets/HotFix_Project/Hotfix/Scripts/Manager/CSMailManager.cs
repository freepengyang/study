using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum MailState
{
    MS_UNREAD = 0,
    MS_READ = 1,
    MS_FETCHED = 2,
}

public class MailItemData : CSPool.IPoolItem
{
    private static DateTime utcTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static string utcTimeFmt = @"yyyy-MM-dd";
    public static MailItemData FocusItem;

    public OnMailItemSelected onMailItemSelected;

    ScriptBinder ScriptBinder { get; set; }

    UIEventListener meventListener;
    UISprite mcover;
    UISprite micon;
    UILabel mtitle;
    UILabel msendTime;
    UISprite mgift;

    public mail.MailInfo data;

    protected bool isOn = false;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (isOn != value)
            {
                isOn = value;
                mcover.gameObject.SetActive(isOn);
            }
        }
    }

    public void OnRecycle()
    {
        this.isOn = false;
        meventListener.onClick = null;
        mgift = null;
        msendTime = null;
        mtitle = null;
        mtitle = null;
        mcover = null;
        meventListener = null;
        ScriptBinder = null;
        this.data = null;
        onMailItemSelected = null;
    }

    public bool HasFetched
    {
        get
        {
            return data.state == (int)MailState.MS_FETCHED;
        }
    }

    protected void TryMarkAsReaded()
    {
        if (data.state == (int)MailState.MS_UNREAD)
        {
            data.state = (int)MailState.MS_READ;
            micon.spriteName = ScriptBinder.GetStringArgv(1);
            HotManager.Instance.EventHandler.SendEvent(CEvent.OnMailRead);

            if (!CSMailManager.Instance.DebugMode)
                Net.ReqReadOneMailMessage(data.mailId);
        }
    }

    public void OnItemVisible(ScriptBinder binder, mail.MailInfo data, bool isOn)
    {
        this.data = data;
        ScriptBinder = binder;
        this.isOn = isOn;

        meventListener = ScriptBinder.GetObject("eventListener") as UIEventListener;
        mcover = ScriptBinder.GetObject("cover") as UISprite;
        micon = ScriptBinder.GetObject("icon") as UISprite;
        mtitle = ScriptBinder.GetObject("title") as UILabel;
        msendTime = ScriptBinder.GetObject("sendTime") as UILabel;
        mgift = ScriptBinder.GetObject("gift") as UISprite;
        meventListener.onClick = this.OnClick;

        mcover.gameObject.SetActive(isOn);
        if(isOn)
        {
            FocusItem = this;
            TryMarkAsReaded();
            onMailItemSelected?.Invoke(this);
        }

        if (data.state == (int)MailState.MS_UNREAD)
        {
            micon.spriteName = ScriptBinder.GetStringArgv(0);
        }
        else
        {
            micon.spriteName = ScriptBinder.GetStringArgv(1);
        }

        if (CSMailManager.Instance.HasUnAcquiredItemsInMail(data))
        {
            mgift.spriteName = ScriptBinder.GetStringArgv(2);
        }
        else
        {
            mgift.spriteName = ScriptBinder.GetStringArgv(3);
        }

        bool unreaded = data.state == (int)MailState.MS_UNREAD;
        mtitle.text = data.title.BBCode(unreaded ? ColorType.MainText : ColorType.SecondaryText);
        DateTime startTime = TimeZoneInfo.ConvertTime(utcTime, TimeZoneInfo.Local);
        DateTime dt = startTime.AddSeconds(data.sendTime / 1000);
        msendTime.text = dt.ToString(utcTimeFmt).BBCode(unreaded ? ColorType.MainText : ColorType.SecondaryText);
    }

    public void OnClick(GameObject go)
    {
        if (ScriptBinder.gameObject != go)
        {
            return;
        }

        if (FocusItem != this)
        {
            if (null != FocusItem)
            {
                FocusItem.IsOn = false;
            }
            FocusItem = this;
            if (null != FocusItem)
            {
                FocusItem.IsOn = true;

                TryMarkAsReaded();

                onMailItemSelected?.Invoke(this);
            }
        }
    }
}

public delegate void OnMailItemSelected(MailItemData data);

public class CSMailManager : CSInfo<CSMailManager>
{
    public CSMailManager()
    {
        Initialize();
    }

    public override void Dispose()
    {
        mailItemDataPool?.Dispose();
        mailItemDataPool = null;
        mMailsDic.Clear();
        mMailsDic = null;
        mailCaches?.Clear();
        mailCaches = null;
    }

    protected CSPool.Pool<MailItemData> mailItemDataPool = null;
    public CSPool.Pool<MailItemData> MailItemDataPool
    {
        get
        {
            if(null == mailItemDataPool)
            {
                mailItemDataPool = CSPool.ListPoolHandle.CreateList<MailItemData>(new PoolHandleManager());
            }
            return mailItemDataPool;
        }
    }

    protected const bool debug = false;
    public bool DebugMode
    {
        get
        {
            return debug;
        }
    }
    protected Dictionary<long, mail.MailInfo> mMailsDic = new Dictionary<long, mail.MailInfo>(32);
    public Dictionary<long, mail.MailInfo> Mails
    {
        get
        {
            return mMailsDic;
        }
    }

    public int MaxMailCount
    {
        get
        {
            return 99;
        }
    }

    protected List<object> mailCaches;
    public List<object> SortedMailList
    {
        get
        {
            if(null == mailCaches)
            {
                mailCaches = new List<object>(mMailsDic.Count);
            }
            mailCaches.Clear();
            var iter = mMailsDic.GetEnumerator();
            while(iter.MoveNext())
            {
                mailCaches.Add(iter.Current.Value);
            }
            mailCaches.Sort(MailComparer);
            return mailCaches;
        }
    }

    protected int MailComparer(object x, object y)
    {
        if ((x as mail.MailInfo).sendTime < (y as mail.MailInfo).sendTime)
            return 1;
        if ((x as mail.MailInfo).sendTime > (y as mail.MailInfo).sendTime)
            return -1;
        return 0;
    }

    public void LogFormat(string fmt,params object[] argv)
    {
        FNDebug.LogFormat("<color=#00ffff>[mail]:{0}</color>", string.Format(fmt,argv));
    }

    public void Initialize()
    {
        mMailsDic.Clear();
        if (DebugMode)
        {
            int cnt = UnityEngine.Random.Range(15, 30);
            string[] name = new string[]
            {
            "DogKey","SkyNet","神之领域","至尊狗","大宝",
            };
            int configId = 95587;
            for (int i = 0; i < cnt; ++i)
            {
                long id = (i + 1001);
                var mailInfi = new mail.MailInfo();
                mailInfi.mailId = id;
                mailInfi.state = UnityEngine.Random.Range(0, 2);
                mailInfi.title = $"邮件_{id}";
                mailInfi.sendTime = 1583243670 + UnityEngine.Random.Range(-100, 1000);
                mailInfi.from = name[UnityEngine.Random.Range(0, name.Length - 1)];
                mailInfi.content = $"免去詹永新的中华人民共和国驻以色列国特命全权大使职务;任命杜伟为中华人民共和国驻以色列国特命全权大使。--{UnityEngine.Random.Range(111111,222222)}";
                int itemsCount = UnityEngine.Random.Range(0, 6);
                if (itemsCount == 0)
                {
                    mailInfi.items = string.Empty;
                }
                else
                {
                    var item = string.Empty;
                    for (int j = 0; j < itemsCount; ++j)
                    {
                        item += $"{configId}#";
                        item += $"{UnityEngine.Random.Range(1, 99)}#";
                        item += $"{UnityEngine.Random.Range(0, 1) * UnityEngine.Random.Range(30, 500)}#";
                        item += $"{UnityEngine.Random.Range(0, 20)}#";
                        if (j == itemsCount - 1)
                            item += $"注灵:{name[UnityEngine.Random.Range(0, name.Length - 1)]}";
                        else
                            item += $"注灵:{name[UnityEngine.Random.Range(0, name.Length - 1)]};";
                    }
                    mailInfi.items = item;
                }
                mMailsDic.Add(id, mailInfi);
            }
            LogFormat("初始化邮件{0}", cnt);
        }
        else
        {
            //Net.ReqGetMailListMessage();
        }
    }

    public void InitMailList(mail.MailList mailList)
    {
        mMailsDic.Clear();
        int n = mailList.mails.Count;
        for (int i = 0; i < n; ++i)
        {
            var mail = mailList.mails[i];
            if (!mMailsDic.ContainsKey(mail.mailId))
            {
                mMailsDic.Add(mail.mailId, mail);
            }
            else
            {
                mMailsDic[mail.mailId] = mail;
                FNDebug.LogErrorFormat("mail repeated id = {0}", mail.mailId);
            }
        }

        HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
    }

    public void AddNewMail(mail.MailInfo mail)
    {
        if (!mMailsDic.ContainsKey(mail.mailId))
        {
            mMailsDic.Add(mail.mailId, mail);
        }
        else
        {
            mMailsDic[mail.mailId] = mail;
            FNDebug.LogErrorFormat("mail repeated id = {0}", mail.mailId);
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
    }

    public void ModifyOneMailStatus(long mailId, MailState mailState)
    {
        mail.MailInfo mailData = null;
        if (mMailsDic.ContainsKey(mailId))
        {
            mailData = mMailsDic[mailId];
            if (null != mailData)
            {
                mailData.state = (int)mailState;
                if(mailState == MailState.MS_FETCHED)
                {
                    mailData.items = string.Empty;
                }
            }

            HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
        }
    }

    public void MarkDirty()
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
    }

    public void ModifyMailStatus(mail.MailIdMsg mailIds, MailState mailState)
    {
        int n = mailIds.mailIds.Count;
        for (int i = 0; i < n; ++i)
        {
            var mailId = mailIds.mailIds[i];
            mail.MailInfo mailData = null;
            if(mMailsDic.ContainsKey(mailId))
            {
                mailData = mMailsDic[mailId];
                if (null != mailData)
                {
                    mailData.state = (int)mailState;
                    //if(mailState == MailState.MS_FETCHED)
                    //{
                    //    mailData.items = string.Empty;
                    //}
                }
            }
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.OnMailStateChanged);
    }

    //public void ReadMail(long mailId)
    //{
    //    mail.MailInfo mailData = null;
    //    if(mMailsDic.TryGetValue(mailId,out mailData))
    //    {
    //        if(null != mailData && mailData.state == (int)MailState.MS_UNREAD)
    //        {
    //            mailData.state = (int)MailState.MS_READ;
    //            if (!DebugMode)
    //                Net.ReqReadOneMailMessage(mailData.mailId);
    //        }
    //    }
    //}

    public void DeleteMail(mail.MailIdMsg mailIds)
    {
        int n = mailIds.mailIds.Count;
        for (int i = 0; i < n; ++i)
        {
            var mailId = mailIds.mailIds[i];
            mMailsDic.Remove(mailId);
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
    }

    public void clear()
    {
        mMailsDic.Clear();
        HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
    }

    public bool HasUnAcquiredMail
    {
        get
        {
            var iter = mMailsDic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (HasUnAcquiredItemsInMail(iter.Current.Value))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool HasUnReadMail
    {
        get
        {
            var iter = mMailsDic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.state == (int)MailState.MS_UNREAD)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool IsMailFull
    {
        get
        {
            return mMailsDic.Count >= MaxMailCount;
        }
    }

    public bool AcquireAllMails(PoolHandleManager poolHandle)
    {
        LogFormat("[Operate]:AcquireAllMails");
        List<long> datas = poolHandle.GetSystemClass<List<long>>();
        var mails = SortedMailList;
        for(int i = 0; i < mails.Count;++i)
        {
            var mailData = mails[i] as mail.MailInfo;
            if (HasUnAcquiredItemsInMail(mailData))
            {
                datas.Add(mailData.mailId);
            }
        }
        if(datas.Count > 0)
        {
            if(!DebugMode)
                Net.ReqGetMailItemMessage(datas);
            else
            {
                mail.MailIdMsg msg = new mail.MailIdMsg();
                msg.mailIds.AddRange(datas);
                ModifyMailStatus(msg, MailState.MS_FETCHED);
            }
        }
        poolHandle.Recycle(datas);
        bool result = datas.Count > 0;
        datas.Clear();
        return result;
    }

    public void AcquireOneMail(long id)
    {
        LogFormat("[Operate]:AcquireOnMail id = {0}", id);
        if (!mMailsDic.ContainsKey(id))
        {
            LogFormat("[Operate]:AcquireOnMail id = {0} Not Exist In Dic", id);
            return;
        }
        var data = mMailsDic[id];
        if(null != data && data.state != (int)MailState.MS_FETCHED)
        {
            LogFormat("[Operate]:AcquireOnMail Start Fetch id = {0}", id);
            if (debug)
            {
                ModifyOneMailStatus(id,MailState.MS_FETCHED);
            }
            else
            {
                Net.ReqGetOneMailItemMessage(id);
            }
        }
    }

    public void DeleteOneMail(long id)
    {
        if (mMailsDic.Remove(id))
        {
            LogFormat("[Operate]:DeleteOneMail id = {0}", id);
            if (DebugMode)
            {
                HotManager.Instance.EventHandler.SendEvent(CEvent.MailListChanged);
            }
            else
            {
                Net.ReqDeleteOneMailMessage(id);
            }
        }
    }

    public bool HasUnAcquiredItemsInMail(mail.MailInfo mail)
    {
        if(null == mail)
        {
            return false;
        }

        if(mail.state == (int)MailState.MS_FETCHED)
        {
            return false;
        }

        if(string.IsNullOrEmpty(mail.items))
        {
            return false;
        }

        return true;
    }

    public bool DeleteAllMails(PoolHandleManager poolHandle)
    {
        List<long> datas = poolHandle.GetSystemClass<List<long>>();
        datas.Clear();
        var iter = mMailsDic.GetEnumerator();
        while(iter.MoveNext())
        {
            if(!HasUnAcquiredItemsInMail(iter.Current.Value))
            {
                datas.Add(iter.Current.Key);
            }
        }
        if(datas.Count <= 0)
        {
            return false;
        }
        LogFormat("[Operate]:DeleteAllMails cnt = {0}",datas.Count);
        if(debug)
        {
            var msg = new mail.MailIdMsg();
            msg.mailIds.AddRange(datas);
            DeleteMail(msg);
        }
        else
        {
            Net.ReqDeleteMailMessage(datas);
            datas.Clear();
            poolHandle.Recycle(datas);
        }
        return true;
    }
}