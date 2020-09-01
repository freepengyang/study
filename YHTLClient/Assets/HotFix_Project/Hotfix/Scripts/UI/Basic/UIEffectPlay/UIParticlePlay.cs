using UnityEngine;
using System.Collections;

public class  UIParticlePlay : UIEffectPlayBase
{
    private GameObject ParticleObj = null;
    private int mParticlePlayTime = 0;
    private ParticleSystem[] particleSystems;
    private int objScale;
    private int sortingOrder = 300;
    private Schedule schedule1;
    private Schedule schedule2;
    Vector3? mPosition = null;
    
    
    public void ShowParticleEffect(GameObject go, string str, int mParticlePlayTime = 0, int type = ResourceType.UIEffect, bool delete = true, int objScale = 1, bool IsDestroyParent = false, Vector3? position = null, int sortingStage = 300, bool isrepeat = false, System.Action action = null)
    {
        gameObject = go;
        if (string.IsNullOrEmpty(str)) return;

        if (resName == str)
        {
            if (isrepeat)
            {
                this.gameObject.SetActive(false);
                this.gameObject.SetActive(true);
            }
            return;
        }
        if (!string.IsNullOrEmpty(resName)) DestroyEffect();
        CancelParticleInvoke();
        IsDestroy = IsDestroyParent;
        resName = str;
        mPosition = position;
        this.objScale = objScale;
        this.mParticlePlayTime = mParticlePlayTime;
        this.sortingOrder = sortingStage;
        this.renderCallBack = action;
        CSResource res = CSResourceManager.Singleton.AddQueue(str, type, OnLoadParticleEffect, ResourceAssistType.UI);
        mPath = res.Path;
        res.IsCanBeDelete = delete;
    }
    
    private void OnCloseParticle(Schedule schedule)
    {
        if (particleSystems != null && ParticleObj != null)
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Stop();
            }
        schedule1 = Timer.Instance.InvokeRepeating(0f, 0.5f, OnDesParticle);
    }
    private void OnDesParticle(Schedule schedule)
    {
        bool allStopped = true;

        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (!particleSystems[i].isStopped)
            {
                allStopped = false;
                break;
            }
        }
        if (allStopped)
        {
            if (Timer.Instance.IsInvoking(schedule))
                Timer.Instance.CancelInvoke(schedule);
            Dispose();
        }
    }
    private void OnLoadParticleEffect(CSResource res)
    {
        if (res == null || res.MirrorObj == null) return;
        if (gameObject == null)
        {
            if (ParticleObj != null) GameObject.Destroy(ParticleObj);
            return;
        }
        ParticleObj = res.GetObjInst() as GameObject;

        if (ParticleObj == null) return;

        Renderer[] mrenderer = ParticleObj.GetComponentsInChildren<Renderer>();
        ParticleObj.transform.parent = gameObject.transform;
        ParticleObj.transform.localScale = Vector3.one * objScale;
        if (mPosition != null) ParticleObj.transform.localPosition = (Vector3)mPosition;
        if (mParticlePlayTime != 0)
        {
            particleSystems = ParticleObj.GetComponentsInChildren<ParticleSystem>();
            schedule2 = Timer.Instance.Invoke(mParticlePlayTime, OnCloseParticle);
        }
        if (mrenderer != null)
        {
            for (int i = 0; i < mrenderer.Length; i++)
            {
                mrenderer[i].material.shader = Shader.Find(mrenderer[i].material.shader.name);
            }
        }
        for (int i = 0; i < ParticleObj.transform.childCount; i++)
        {
            ParticleObj.transform.GetChild(i).gameObject.layer = 5;
        }
        ParticleObj.layer = 5;
        if (ParticleObj.GetComponent<RenderQueue>() == null)
            ParticleObj.AddComponent<RenderQueue>();
        ParticleObj.GetComponent<RenderQueue>().sortingStage = sortingOrder;
        if (renderCallBack != null)
            renderCallBack();
    }

    private void CancelParticleInvoke()
    {
        if (Timer.Instance.IsInvoking(schedule1))
            Timer.Instance.CancelInvoke(schedule1);
        if (Timer.Instance.IsInvoking(schedule2))
            Timer.Instance.CancelInvoke(schedule2);
    }

    protected override void DestroyEffect()
    {
        resName = "";
        if (ParticleObj != null)
        {
            CancelParticleInvoke();
            GameObject.Destroy(ParticleObj);
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        mParticlePlayTime = 0;
        particleSystems = null;
        schedule1 = null;
        schedule2 = null;
    }
}
