using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSScaleMapSystem : Singleton2<CSScaleMapSystem>
{
    private GameObject mScaleMapGO = null;
    private CSSprite mScaleMap = null;
    private float mStartTime = 0.0f;
    private float mLoadTime = 3f;
    private bool mIsLoadedScaleMap = false;
    private string mScaleMapPath = string.Empty;
    public bool IsLoadedScaleMap
    {
        get
        {
            if ((Time.time - mStartTime >= mLoadTime) && (!mIsLoadedScaleMap))
            {
                mIsLoadedScaleMap = true;
            }
            return mIsLoadedScaleMap;
        }
    }

    public void Create(Transform parent)
    {
        if (mScaleMapGO == null)
        {
            mScaleMapGO = new GameObject("ScaleMap");
            GameObject go = new GameObject("UV");
            go.transform.parent = mScaleMapGO.transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            mScaleMap = go.AddComponent<CSSprite>();
            mScaleMap.MainMaterial = CSShaderManager.GetMaterial("Mobile/LZF/Alpha Blended");
            mScaleMapGO.transform.parent = parent;
        }
    }

    public void PreloadingScaleMap()
    {
        if (CSResourceManager.Singleton == null)
        {
            return;
        }
        mStartTime = Time.time;
        //TABLE.MAPINFO tblMapInfo;
        //if (MapInfoTableManager.Instance.TryGetValue(CSMainPlayerInfo.Instance.MapID, out tblMapInfo))
        //{
            mIsLoadedScaleMap = false;
            if (!string.IsNullOrEmpty(mScaleMapPath))
            {
                CSResource preRes = CSResourceManager.Instance.GetRes(mScaleMapPath);
                if (preRes != null)
                {
                    preRes.IsCanBeDelete = true;
                }
                CSResourceManager.Singleton.DestroyResource(mScaleMapPath);
            }
            CSStringBuilder.Clear();
            CSStringBuilder.Append("s_", CSMainParameterManager.tableMapInfo.img);     //TODO:ddn
            CSResource res = CSResourceManager.Singleton.AddQueue(CSStringBuilder.ToString(),
                ResourceType.ScaleMap, OnLoadedScaleMap, ResourceAssistType.ForceLoad);
            mScaleMapPath = res.Path;
        //}
    }

    private void OnLoadedScaleMap(CSResource res)
    {
        if (res == null || res.MirrorObj == null || mScaleMap == null)
        {
            mIsLoadedScaleMap = true;
            FNDebug.Log("OnLoadedScaleMap = null = " + res.GetRelatePath());
            return;
        }
        FNDebug.Log("OnLoadedScaleMapOnLoadedScaleMapOnLoadedScaleMap");
        res.IsCanBeDelete = false;
        mScaleMap.Picture = res.MirrorObj as Texture;
        SetScaleMapShader();
        SetScaleMapPosition();
        mIsLoadedScaleMap = true;
    }

    private void SetScaleMapShader()
    {
        //TODO:yezhangbiqi
        mScaleMap.SetShader(mScaleMap.MainMaterial, Vector4.one, Vector4.one);
    }

    private void SetScaleMapPosition()
    {
        //TABLE.MAPINFO tblMapInfo;
        Vector2 mFillSize;
        Vector2 size = Vector2.zero;

        //if (MapInfoTableManager.Instance.TryGetValue(CSMainPlayerInfo.Instance.MapID, out tblMapInfo))
        //{
            // 初始化地图尺寸
            string[] strs = CSMainParameterManager.tableMapInfo.mapSize.Split('#');

            size.x = Convert.ToInt32(strs[0]);
            size.y = Convert.ToInt32(strs[1]);
            mFillSize.x = size.x % CSTerrainCell.Size.x;
            mFillSize.y = size.y % CSTerrainCell.Size.y;

            int Columns = (int)(size.x / CSTerrainCell.Size.x) + (mFillSize.x > 0 ? 1 : 0);
            int Rows = (int)(size.y / CSTerrainCell.Size.y) + (mFillSize.y > 0 ? 1 : 0);

            float exactX = size.x % CSCell.Size.x;
            float exactY = size.y % CSCell.Size.y;

            int mHorizontalCount = (int)(size.y / CSCell.Size.y) + (exactY > 0 ? 1 : 0);
            int mVerticalCount = (int)(size.x / CSCell.Size.x) + (exactX > 0 ? 1 : 0);

            // float scale = mVerticalCount * CSCell.Size.x / 256f;
            float scale = size.x / 128;
            mScaleMapGO.transform.localScale = new Vector3(scale, scale, 1);

            float x = CSTerrainCell.Size.x / 2;
            float y = (Rows * CSTerrainCell.Size.y - CSTerrainCell.Size.y / 2);

            mScaleMapGO.transform.localPosition = new Vector3(x, y, 12000f);
            mScaleMapGO.transform.localPosition += new Vector3(128 * scale * 0.5f - 256, -mScaleMap.Picture.height * scale * 0.5f + 128, 0);

            //Debug.Log("======> MapID = " + CSMainPlayerInfo.Instance.MapID + "  size = " + size + "  height = " + mScaleMap.Picture.height);


            //if (CSScene.IsLanuchMainPlayer) CSScene.Sington.MainPlayer.UpdatePosTerrainDataIsNotLoaded(Columns, Rows, mHorizontalCount, mVerticalCount);
        //}
    }

        public void Destroy()
    {

    }
}
