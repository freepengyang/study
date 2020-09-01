using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : IndexedItem
{
    public int Index { get; set; }
    public Vector3 pos;
    public string m_name;
    public Emotion()
    {
        pos = Vector3.zero;
        m_name = string.Empty;
    }

    public Emotion(Vector3 _pos, string _m_name)
    {
        this.m_name = _m_name;
        this.pos = _pos;
    }
}

public class UIChatBinder : UIChatBaseBinder
{
    protected const int dis_vip_level = 2;
    protected const int dis_vip_interval = 4;//vip图标和title的间隔
    protected UISprite sp_vip;
    protected UILabel lb_vip;
    protected UILabel lb_content;
    protected UISprite sp_bubble;
    protected GameObject go_baiyin;
    protected AutoChangeColorLabel lb_content_color;
    public override int Height
    {
        get
        {
            if (sp_bubble.gameObject.activeSelf)
            {
                return lb_title.height + sp_bubble.height + 15;
            }
            return lb_title.height + 15;
        }
    }

    private int bubbleSprWhite = 351;//背景框的宽度
    private int lb_constentWidth = 324;//聊天框的宽度
    private int ConstentPosX = 62;//聊天内容起始X轴位置
    private int ConstentPosY = -36;//聊天内容起始Y轴位置位置
    private Transform lbConstentTrans;

    public override void Init(UIEventListener handle)
    {
        base.Init(handle);

        sp_vip = Get<UISprite>("vipIcon");
        lb_vip = Get<UILabel>("vipLevel");
        lb_content = Get<UILabel>("constent");
        lb_content_color = lb_content.GetComponent<AutoChangeColorLabel>();
        sp_bubble = Get<UISprite>("bubble");
        go_baiyin = Handle.transform.Find("baiyin").gameObject;

        lbConstentTrans = lb_content.transform;
        ConstentPosX = (int)lbConstentTrans.localPosition.x;
        ConstentPosY = (int)lbConstentTrans.localPosition.y;

        lb_content.SetupLink();
    }

    protected override void CreateVoice(string id, string url, uint duration, string text, string ext, string playerName)
    {
        //string[] exts = ext.Split('#');
        //if(exts.Length >= 4)
        //{
            if (sp_voice != null)
            {
                sp_voice.gameObject.SetActive(true);
                int durationTime = (int)System.Math.Ceiling(duration * 0.001f);
                sp_voice.mDuration = durationTime;
                sp_voice.mUrl = url;
                sp_voice.ext = ext;
                //设置语音时长文本
                if (null != lb_text)
                {
                    lb_text.text = CSString.Format(329, durationTime);
                }
            }

            if (/*text != exts[3].ToString() || */text.Contains(":") || !QuDaoConstant.OpenTranslate)
                text = "";

            if(chatData.coloredMsg)
            {
                SetAutoColoursWrold(lb_content, lb_content_color, 294);
            }

            lb_content.HorizontalApplyGradient = chatData.coloredMsg;
            lb_content_color.enabled = chatData.coloredMsg;

            //是否自动翻译
            var appendValue = string.Empty;
            if (string.IsNullOrEmpty(text))
                appendValue = "\n";
            else
                appendValue = "\n\n[dcd5b8]" + text;

            //解析表情
            ParseSymbol(lb_content, lb_content.width, ref appendValue);
            //更新字体设置
            UpdateNGUIText(lb_content);
            var size = NGUIText.CalculateCharacterSize(appendValue, lb_content.width);
            int textWidth = Mathf.RoundToInt(size.x);
            if (textWidth <= lb_constentWidth && size.y <= 30)
            {
                sp_bubble.width = textWidth + 37;
            }
            else
            {
                sp_bubble.width = bubbleSprWhite;
            }
            lb_content.text = "[dcd5b8]" + appendValue;
            sp_bubble.height = 22 + lb_content.height;
            CreateFace(lb_content);
            if (Location == LocationType.LT_RIGHT)
            {
                if (size.y <= 30)
                    lbConstentTrans.localPosition = new Vector3(ConstentPosX + (lb_constentWidth - textWidth), ConstentPosY, 0);
                else
                    lbConstentTrans.localPosition = new Vector3(ConstentPosX, ConstentPosY, 0);
            }
            else
            {
                lbConstentTrans.localPosition = new Vector3(ConstentPosX, ConstentPosY, 0);
            }

            if (!lb_content.gameObject.activeSelf)
                lb_content.gameObject.SetActive(true);
        /*}
        else
        {
            if (null != sp_voice)
            {
                sp_voice.gameObject.SetActive(false);
            }
        }*/
    }

    public override void Bind(object data)
    {
        chatData = data as ChatData;
        if(null != chatData)
        {
            bool isSelf = chatData.msg.sender == CSMainPlayerInfo.Instance.ID;
            //设置频道标签
            sp_channel_spring.SteupChannel(chatData);
            //获取玩家名字 [position]|[name]
            var playerName = chatData.GetName();
            bool isSystemMsg = chatData.IsSystemMessage();
            //设置底框
            if (null != sp_bubble)
            {
                //if (!isSystemMsg)
                //{
                //    sp_bubble.spriteName = "paopao";
                //}

                if(sp_bubble.gameObject.activeSelf != !isSystemMsg)
                {
                    sp_bubble.gameObject.SetActive(!isSystemMsg);
                }

                if(lb_content.gameObject.activeSelf != !isSystemMsg)
                {
                    lb_content.gameObject.SetActive(!isSystemMsg);
                }
            }
            //设置标题内容
            if (null != lb_title)
            {
                if(isSystemMsg)
                {
                    sp_vip.CustomActive(false);
                    lb_vip.CustomActive(false);
                    lb_title.overflowMethod = UILabel.Overflow.ResizeHeight;
                    lb_title.width = 310;
                    lb_title.text = chatData.msg.message;
                }
                else
                {
                    bool isVip = chatData.msg.vip > 0;
                    sp_vip.CustomActive(isVip);
                    lb_title.overflowMethod = UILabel.Overflow.ResizeFreely;
                    lb_title.text = playerName;
                    bool vipLevelVisible = isVip && (chatData.msg.showVipLevel == 1);
                    if (null != lb_vip && vipLevelVisible)
                    {
                        lb_vip.text = chatData.msg.vip.ToString();
                    }
                    lb_vip.CustomActive(vipLevelVisible);

                    if(isSelf)
                    {
                        if(vipLevelVisible)
                        {
                            //VIP等级
                            Vector3 pos = Vector3.zero;
                            if(null != lb_vip && null != lb_title)
                            {
                                pos = lb_vip.transform.localPosition;
                                pos.x = lb_title.transform.localPosition.x - lb_title.width - dis_vip_level;
                                lb_vip.transform.localPosition = pos;
                            }

                            if (null != sp_vip && null != lb_vip)
                            {
                                //VIP图标
                                pos = sp_vip.transform.localPosition;
                                pos.x = lb_vip.transform.localPosition.x - lb_vip.width;
                                sp_vip.transform.localPosition = pos;
                            }
                        }
                        else
                        {
                            if(null != sp_vip && null != lb_title)
                            {
                                //VIP图标
                                Vector3 pos = sp_vip.transform.localPosition;
                                pos.x = lb_title.transform.localPosition.x - lb_title.width - dis_vip_interval;
                                sp_vip.transform.localPosition = pos;
                            }
                        }
                    }
                    else
                    {
                        if (vipLevelVisible)
                        {
                            //VIP图标
                            Vector3 pos = Vector3.zero;
                            if(null != sp_vip && null != lb_title)
                            {
                                pos = sp_vip.transform.localPosition;
                                pos.x = lb_title.transform.localPosition.x + lb_title.width;
                                sp_vip.transform.localPosition = pos;
                            }

                            //VIP等级
                            if(null != lb_vip && null != sp_vip)
                            {
                                pos = lb_vip.transform.localPosition;
                                pos.x = sp_vip.transform.localPosition.x + sp_vip.width;
                                lb_vip.transform.localPosition = pos;
                            }
                        }
                        else
                        {
                            if (null != sp_vip && null != lb_title)
                            {
                                //VIP图标
                                Vector3 pos = sp_vip.transform.localPosition;
                                pos.x = lb_title.transform.localPosition.x + lb_title.width + dis_vip_interval;
                                sp_vip.transform.localPosition = pos;
                            }
                        }
                    }
                }
            }

            if (chatData.IsVoiceMessage())
            {
                if (!sp_bubble.gameObject.activeSelf)
                    sp_bubble.gameObject.SetActive(true);

                string[] msgs = chatData.msg.message.Split('$');
                if (msgs.Length >= 6)
                {
                    CreateVoice(msgs[1], msgs[2], (uint)(int.Parse(msgs[3])), msgs[4], msgs[5], playerName);
                }
                else
                {
                    if (null != sp_voice)
                    {
                        sp_voice.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (null != sp_voice)
                {
                    sp_voice.gameObject.SetActive(false);
                }

                bool enableColoredMsg = chatData.coloredMsg && !isSystemMsg;
                //彩世界消息
                if (enableColoredMsg)
                    SetAutoColoursWrold(lb_content, lb_content_color, 294);
                lb_content_color.enabled = enableColoredMsg;
                lb_content.HorizontalApplyGradient = enableColoredMsg;

                //设置文本内容
                if(!isSystemMsg)
                {
                    var text = chatData.msg.message;
                    ParseSymbol(lb_content, lb_content.width, ref text);
                    UpdateNGUIText(lb_content);

                    var size = NGUIText.CalculateCharacterSize(text,lb_content.width);
                    int textWidth = Mathf.RoundToInt(size.x);
                    if (textWidth <= lb_constentWidth && size.y <= 30)
                    {
                        sp_bubble.width = textWidth + 30;
                    }
                    else
                    {
                        sp_bubble.width = bubbleSprWhite;
                    }
                    lb_content.text = "[dcd5b8]" + text;
                    sp_bubble.height = 13 + mEmotionAdd + lb_content.height;
                    CreateFace(lb_content);
                    if (Location == LocationType.LT_RIGHT)
                    {
                        if (size.y <= 30)
                            lbConstentTrans.localPosition = new Vector3(ConstentPosX + (lb_constentWidth - textWidth), ConstentPosY, 0);
                        else
                            lbConstentTrans.localPosition = new Vector3(ConstentPosX, ConstentPosY, 0);
                    }
                    else
                    {
                        lbConstentTrans.localPosition = new Vector3(ConstentPosX, ConstentPosY, 0);
                    }
                }
            }
        }
    }

    //设置彩世颜色
    public static void SetAutoColoursWrold(UILabel lb_content,AutoChangeColorLabel coloredLabel,int id)//根据sundry表的不同id显示不同的颜色60034\60098
    {
        TABLE.SUNDRY sundry;
        if (SundryTableManager.Instance.TryGetValue(id, out sundry))
        {
            lb_content.gameObject.SetActive(false);
            string[] color = sundry.effect.Split('#');
            AutoChangeColorLabel colorLabel = coloredLabel;

            Color c1 = Color.white;

            if (color.Length >= 1)
            {
                ColorUtility.TryParseHtmlString("#" + color[0], out c1);
                colorLabel.StartColor = c1;
            }

            if (color.Length >= 2)
            {
                ColorUtility.TryParseHtmlString("#" + color[1], out c1);
                colorLabel.MiddleColor = c1;
            }
            if (color.Length >= 3)
            {
                ColorUtility.TryParseHtmlString("#" + color[2], out c1);
                colorLabel.EndColor = c1;
            }
            lb_content.gameObject.SetActive(true);
        }
    }

    public override void OnDestroy()
    {
        sp_vip = null;
        lb_vip = null;
        lb_content = null;
        sp_bubble = null;
        go_baiyin = null;
        base.OnDestroy();
    }
}