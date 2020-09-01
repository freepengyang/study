using UnityEngine;
using System.Collections;

//流光控制
public class NGUIScrollLight : MonoBehaviour
{
    public enum PlayModel
    {
        Once,
        Loop,
    }

    public PlayModel playModel = PlayModel.Loop;
    public float uvSpeed = 2;       //流光速度
    public float time = 3;

    private bool mState = false;

    private float widthRate;
    private float heightRate;
    private float xOffsetRate;
    private float yOffsetRate;
    private UISprite _sprite;
    private UISprite sprite { get { return _sprite ?? (_sprite = this.GetComponent<UISprite>()); } }

    void Awake()
    {
        //跟流光无关,可以改变
       
    }

    public void Init()
    {
        if (sprite != null)
        {
            widthRate = sprite.GetAtlasSprite().width * 1.0f / sprite.atlas.spriteMaterial.mainTexture.width;
            heightRate = sprite.GetAtlasSprite().height * 1.0f / sprite.atlas.spriteMaterial.mainTexture.height;
            xOffsetRate = sprite.GetAtlasSprite().x * 1.0f / sprite.atlas.spriteMaterial.mainTexture.width;
            yOffsetRate = (sprite.atlas.spriteMaterial.mainTexture.height - (sprite.GetAtlasSprite().y + sprite.GetAtlasSprite().height)) * 1.0f / sprite.atlas.spriteMaterial.mainTexture.height;
        }

        if (sprite != null)
        {
            sprite.atlas.spriteMaterial.SetFloat("_WidthRate", widthRate);
            sprite.atlas.spriteMaterial.SetFloat("_HeightRate", heightRate);
            sprite.atlas.spriteMaterial.SetFloat("_XOffset", xOffsetRate);
            sprite.atlas.spriteMaterial.SetFloat("_YOffset", yOffsetRate);

            //流光 speed
            sprite.atlas.spriteMaterial.SetFloat("_UVSpeed", uvSpeed);
        }

        OpenEffect();
    }

    public void Destroy()
    {
        CloseEffect();
    }
    void OnDestroy()
    {
        CloseEffect();
    }

    public void OpenEffect()
    {
        CloseEffect();
        InvokeRepeating("StartScroll", 0, time);
    }

    public void CloseEffect()
    {
        CancelInvoke("StartScroll");
        SetActive(false);
    }

    public void SetActive(bool state)
    {
        mState = state;
        if (sprite != null)
        {
            if (state)
            {
                sprite.color = new Color(1, 0, 1);
            }
            else
            {
                sprite.color = Color.white;
            }
        }

    }

    private void StartScroll()
    {
        SetActive(!mState);
    }
}
