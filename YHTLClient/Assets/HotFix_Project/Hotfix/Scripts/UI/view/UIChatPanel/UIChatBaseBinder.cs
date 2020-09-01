using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatBaseBinder : UIBinder
{
    protected UILabel lb_title;
    protected NHyperLink_Voice sp_voice;
    protected UILabel lb_text;
    protected UISprite sp_point;
    protected UISprite sp_bg;
    protected BoxCollider sp_bg_colider;
    protected GameObject go_baby_toggle;
    protected UISprite sp_channel_spring;
    public UISprite FaceTemplate { get; set; }
    public enum LocationType
    {
        LT_LEFT = 0,
        LT_RIGHT,
    }
    public LocationType Location { get; set; }
    public enum ChatVisibleMode
    {
        CVM_CHAT_PANEL = 0,
        CVM_MAIN_PANEL = 1,
    }
    protected ChatVisibleMode _chatVisibleMode = ChatVisibleMode.CVM_CHAT_PANEL;
    public ChatVisibleMode Mode 
    {
        get
        {
            return _chatVisibleMode;
        }
        set
        {
            _chatVisibleMode = value;
        }
    }
    public virtual int Height
    {
        get
        {
            return lb_title.height;
        }
    }

    protected int VoiceStartPosX = -123;
    protected void OnOpenChatFrame(GameObject go)
    {
        HotManager.Instance.EventHandler.SendEvent(CEvent.ShowChatPanel);
    }

    public override void Init(UIEventListener handle)
    {
        lb_title = Get<UILabel>("title");
        sp_voice = Get<NHyperLink_Voice>("sp_voice");
        lb_text = Get<UILabel>("sp_voice/lb_text");
        //sp_point = Get<UISprite>("sp_voice/sp_yuandian");

        //注意sp_bg 只有主界面聊天频道有
        sp_bg = Get<UISprite>("bg");
        if (null != sp_bg)
        {
            UIEventListener.Get(sp_bg.gameObject).onClick = this.OnOpenChatFrame;
            sp_bg_colider = sp_bg.GetComponent<BoxCollider>();
        }

        var go_transform = Handle.transform.Find("babyToggle");
        if (null != go_transform)
            go_baby_toggle = Handle.transform.Find("babyToggle").gameObject;
        else
            go_baby_toggle = null;
        sp_channel_spring = Get<UISprite>("channel");

        if(null != sp_voice)
        {
            VoiceStartPosX = (int)sp_voice.transform.localPosition.x + 2;
        }

        lb_title.SetupLink();
    }

    private string _value = string.Empty;
    public string value
    {
        get
        {
            return _value;
        }
    }
    //语音占位符号
    protected string mVoiceReplaceText = "                  ";
    //行首偏移占位符号
    protected string mHeadOffsetReplaceText = "\u3000\u3000\u3000";

    public ChatData Value
    {
        get
        {
            return chatData;
        }
    }

    protected ChatData chatData;
    public override void Bind(object data)
    {
        chatData = data as ChatData;
        if(null != chatData)
        {
            //频道图标
            sp_channel_spring.SteupChannel(chatData);
            //获取玩家名字 [position]|[name]
            var playerName = chatData.GetName();
            //设置语音消息
            if (chatData.IsVoiceMessage())
            {
                var msgs = chatData.msg.message.Split('$');
                if(msgs.Length >= 6)
                {
                    CreateVoice(msgs[1], msgs[2], uint.Parse(msgs[3]), msgs[4], msgs[5],playerName);
                }
                else
                {
                    //隐藏语音对象
                    if (null != sp_voice && sp_voice.gameObject.activeSelf)
                    {
                        sp_voice.gameObject.SetActive(false);
                    }
                }

                //暂时取消语音
                sp_bg.CustomActive(true);
                if (null != sp_bg)
                {
                    sp_bg.height = Mathf.Max(26, 7 + lb_title.height);
                    if (null != sp_bg_colider)
                    {
                        sp_bg_colider.size = sp_bg.localSize;
                        sp_bg_colider.center = new Vector3(0, sp_bg.localSize.y * -0.50f, 0);
                    }
                }
            }
            else
            {
                //隐藏语音对象
                sp_voice.CustomActive(false);
                //[玩家名称]|[文本内容]
                var colorString = chatData.IsSystemMessage() ? "[b8a586]" : $"{CSString.Format(1501)}[dcd5b8]";
                //设置玩家名称|设置聊天内容颜色|设置内容
                _value = $"{mHeadOffsetReplaceText}{playerName}{colorString}{chatData.msg.message}";

                ParseSymbol(lb_title, lb_title.width, ref _value);
                lb_title.text = _value;
                CreateFace(lb_title);
                //设置背景高度 起始偏移 + 文字高度 - 字体收尾
                sp_bg.CustomActive(true);
                if (null != sp_bg)
                {
                    sp_bg.height = Mathf.Max(26, 7 + lb_title.height);
                    if(null != sp_bg_colider)
                    {
                        sp_bg_colider.size = sp_bg.localSize;
                        sp_bg_colider.center = new Vector3(0, sp_bg.localSize.y * -0.50f, 0);
                    }
                }
            }
        }
    }

    protected virtual void CreateVoice(string id, string url, uint duration, string text, string ext,string playerName)
    {
        //string[] exts = ext.Split('#');
        //if(exts.Length >= 4)
        {
            //计算文字size
            UpdateNGUIText(lb_title);
            var replaceText = NGUIText.StripSymbols(playerName);
            var size = NGUIText.CalculateCharacterSize(replaceText,lb_title.width);
            if(null != sp_voice)
            {
                //设置语音时长
                int durationTime = (int)System.Math.Ceiling(duration * 0.001f);
                sp_voice.mDuration = durationTime;
                //设置语音超链接
                sp_voice.mUrl = url;
                //设置语音参数
                sp_voice.ext = ext;
                //设置语音时长文本
                if(null != lb_text)
                {
                    lb_text.text = CSString.Format(329, durationTime);
                }
                //设置语音模块位置
                sp_voice.transform.localPosition = new Vector3(VoiceStartPosX + size.x, -9f, 0f);
                //显示语音模块
                sp_voice.CustomActive(true);
            }

            if (/*text != exts[3].ToString() || */text.Contains(":") || !QuDaoConstant.OpenTranslate)
                text = string.Empty;

            //是否自动翻译
            if(null != lb_title)
            {
                lb_title.text = $"{mHeadOffsetReplaceText}{playerName}{mVoiceReplaceText}[dcd5b8]{text}";
            }
        }
        //else
        //{
        //    //隐藏语音对象
        //    sp_voice.CustomActive(false);
        //}
    }

    /// <summary>
    /// Update NGUIText.current with all the properties from this label.
    /// </summary>
    protected virtual void UpdateNGUIText(UILabel label)
    {
        NGUIText.fontSize = label.fontSize;//设置当前使用字体大小
        NGUIText.rectWidth = label.width;//设置当前最大宽度
        NGUIText.dynamicFont = label.trueTypeFont;//设置动态字体
        NGUIText.fontScale = 1;
        NGUIText.encoding = true;
        NGUIText.spacingX = label.spacingX;
        NGUIText.spacingY = label.spacingY;
        NGUIText.Update(false);
    }

    protected const string emotion = @"[emoticon";
    protected const int faceWidth = 24;
    protected const string replaceSpace = "     ";//占位符
    protected int mEmotionAdd = 0;//如果表情是最后一行要附加高度4
    protected int offset_X = 0;//空格采用中半角，表情需做个偏移
    protected int offset_Y = 4;
    protected FastArray<Emotion> mEmotions;

    protected bool IsEmotionLink(string text,int index)
    {
        if (text[index] != '[' || index + 12 > text.Length || text[index + 11] != ']')
        {
            return false;
        }

        for(int i = 0; i < emotion.Length; ++i)
        {
            if (text[index + i] != emotion[i])
                return false;
        }

        if(!(text[index + 9] >= '0' && text[index + 9] <= '9'))
        {
            return false;
        }

        if (!(text[index + 10] >= '0' && text[index + 10] <= '9'))
        {
            return false;
        }

        return true;
    }

    protected void ParseSymbol(UILabel label, int width, ref string text)
    {
        //更新静态属性
        UpdateNGUIText(label);

        mEmotions?.Clear();
        mEmotionAdd = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if(IsEmotionLink(text,i))
            {
                Vector3 facePos = Vector3.zero;
                string eName = text.Substring(i, 12);
                var replaceText = NGUIText.StripSymbols(text.Substring(0, i));

                var mCalculatedSize = NGUIText.CalculateCharacterSize(replaceText, width);
                var textWidth = Mathf.RoundToInt(mCalculatedSize.x) + offset_X;
                var textHigh = Mathf.RoundToInt(mCalculatedSize.y);
                if (string.IsNullOrEmpty(replaceText))
                {
                    textHigh = label.height;//当该文本为空的时候，算出的高度为0；位置会错乱
                }
                //如果文字加表情图片长度大于行宽，则表情换行，并将重置表情位置
                //否则重置表情位置
                if (textWidth > NGUIText.rectWidth - faceWidth)
                {
                    text = text.Insert(i, "\n");
                    i += 1;
                    facePos.x = 0;
                    facePos.y = -(textHigh) + offset_Y;
                    facePos.z = 0;
                    mEmotionAdd = 0;
                }
                else
                {
                    mEmotionAdd = 4;
                    facePos.x = textWidth;
                    facePos.y = -(textHigh - NGUIText.spacingY - NGUIText.fontSize) + offset_Y;
                    facePos.z = 0;
                }
                text = text.Remove(i, 12);
                text = text.Insert(i, replaceSpace);//用空格代替表情字符串

                if (null == mEmotions)
                {
                    mEmotions = new FastArray<Emotion>(8);
                }
                var emotion = mEmotions.PushNewElementToTail();
                emotion.pos = facePos;
                emotion.m_name = eName;
            }
        }
    }

    protected void CreateFace(UILabel label)
    {
        label?.transform.DestroyChildren();

        if(null != mEmotions && null != label)
        {
            for (int i = 0; i < mEmotions.Count; i++)
            {
                var sp = GameObject.Instantiate(FaceTemplate) as UISprite;
                sp.gameObject.SetActive(true);
                sp.spriteName = mEmotions[i].m_name;
                sp.transform.parent = label.transform;
                sp.transform.localPosition = mEmotions[i].pos;
                sp.transform.localScale = Vector3.one;
            }
        }
    }

    public override void OnDestroy()
    {
        sp_bg = null;
        sp_bg_colider = null;
        chatData = null;
        lb_title = null;
        sp_voice = null;
        lb_text = null;
        sp_point = null;
        go_baby_toggle = null;
        sp_channel_spring = null;
        mEmotions?.Clear();
        mEmotions = null;
    }
}