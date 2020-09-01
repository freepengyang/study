using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CSCameraManager : Singleton<CSCameraManager>
{
    private const float CAMERA_OFFSET_Y = 0;
    private Transform mMainCameraTrans;
    private Transform mMainCameraParent;

    float perLengthHor;
    float perLengthVer;
    int VisionCount;
    float halfScreenHor;
    float halfScreenVer;
    Vector2 xRange;
    Vector2 yRange;
    Vector3 lastMainPosition;
    bool mIsInitCamera = false;
    float mCameraYDelta = 0;
    Vector3 mCameraCenterPos = new Vector3(0, CAMERA_OFFSET_Y, 0);
    public bool IsInitCamera
    {
        get { return mIsInitCamera; }
        set { mIsInitCamera = value; }
    }

    public Transform  MainCameraTrans
    {
        get
        {
            if (mMainCameraTrans == null)
            {
                if (Camera.main != null)
                    mMainCameraTrans = Camera.main.transform;
            }
            return mMainCameraTrans;
        }
    }


    private Transform cacheTransform = null;
    public Transform CacheTransform {

        get {

            if (cacheTransform == null)
            {
                cacheTransform =CSAvatarManager.MainPlayer.CacheTransform;
            }
            return cacheTransform;
        }
    }

    public Transform MainCameraParent
    {
        get
        {
            if (mMainCameraParent == null)
            {
                mMainCameraParent = MainCameraTrans.parent;
            }
            return mMainCameraParent;
        }
    }

    public void InitMainCamera(Transform parent)
    {
        if (MainCameraParent.parent != parent)
        {
            NGUITools.SetParent(parent, MainCameraParent.gameObject);
            MainCameraTrans.localPosition = new Vector3(0, CAMERA_OFFSET_Y, 0);
        }
    }

    public void ResetCameraPosition()
    {
        mIsInitCamera = false;
        UpdateCameraPosition();
    }

    public void UpdateCameraPosition()
    {
        if (!CSScene.IsLanuchMainPlayer) return;

        if (MainCameraTrans == null || CacheTransform == null)
        {
            return;
        }
        if (!mIsInitCamera)
        {
            mIsInitCamera = true;
            VisionCount = CSMesh.Instance.VisionCount;
            perLengthHor = CSConstant.PixelRatio * CSCell.Size.x;
            perLengthVer = CSConstant.PixelRatio * CSCell.Size.y;
            halfScreenHor = (CSMesh.PadVisionCountX == 0 ? VisionCount : CSMesh.PadVisionCountX) * perLengthHor;
            halfScreenVer = (CSMesh.PadVisionCountY == 0 ? VisionCount : CSMesh.PadVisionCountY) * perLengthVer;
            float curRota = (float)((Screen.width * 1.0f) / (Screen.height * 1.0));

            xRange = CSMesh.Instance.XRange + new Vector2(perLengthHor * (curRota - 1136 / 640), -perLengthHor * (curRota - 1136 / 640));
            yRange = CSMesh.Instance.YRange;
            xRange.y -= perLengthHor;//有半格的情况
            yRange.y -= perLengthVer;//有半格的情况
            mCameraYDelta = CSConstant.PixelRatio * CAMERA_OFFSET_Y;
        }
        Vector3 lastMainPosition = MainCameraParent.position;

        float minX = lastMainPosition.x - halfScreenHor;
        float maxX = lastMainPosition.x + halfScreenHor;
        float minY = lastMainPosition.y - halfScreenVer;
        float maxY = lastMainPosition.y + halfScreenVer;

        bool isChange = false;

        if (minX < xRange.x)
        {
            lastMainPosition.x = xRange.x + halfScreenHor;
            isChange = true;
        }
        if (maxX > xRange.y)
        {
            lastMainPosition.x = xRange.y - halfScreenHor;
            isChange = true;
        }
        if (minY < yRange.x)
        {
            lastMainPosition.y = yRange.x + halfScreenVer + mCameraYDelta;
            isChange = true;
        }
        if (maxY > yRange.y)
        {
            lastMainPosition.y = yRange.y - halfScreenVer + mCameraYDelta;
            isChange = true;
        }
       
        if (isChange)
        {
            lastMainPosition = CacheTransform.InverseTransformPoint(lastMainPosition);
            lastMainPosition.x = (int)lastMainPosition.x;
            lastMainPosition.y = (int)lastMainPosition.y;
            lastMainPosition.z = (int)lastMainPosition.z;
            MainCameraTrans.localPosition = lastMainPosition;
        }
        else
        {
            MainCameraTrans.localPosition = mCameraCenterPos;//边界使用随机石传送的问题
        }
    }
}
