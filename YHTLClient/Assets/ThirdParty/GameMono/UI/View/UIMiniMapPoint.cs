using UnityEngine;

public class UIMiniMapPoint : MonoBehaviour
{
    public bool StartMove { get; set; }
    public Vector3 StartPos { get; set; }
    public Vector3 TargetPosition { get; set; }
    public float Speed { get; set; }

    private float intervalTime = 0;

    private Vector2 temporary = Vector2.zero;

    private Vector2 mMainPlayerPos;

    public Transform Transform { get; set; }

    public void Awake()
    {
        Speed = 0.5f; //默认值
        Transform = transform;
    }

    public void SetSpeed(int _speed)
    {
        Speed = _speed * 0.0001f;
    }

    public void SetRelativePos(Vector3 pos)
    {
        if (Transform == null) Transform = transform;
        TargetPosition = Transform.localPosition;
        TargetPosition -= pos;
        Transform.localPosition = TargetPosition;
    }

    public void SetFixedPos()
    {
        if (Transform == null) Transform = transform;
        Transform.localPosition = temporary;
        TargetPosition = temporary;
    }

    public void SetLocalPos(int dotX, int dotY, int interval_X, int interval_Y,
        Vector2 mMapOneCellPos)
    {
        temporary.x = (dotX - interval_X) * mMapOneCellPos.x + mMainPlayerPos.x;
        temporary.y = (dotY - interval_Y) * mMapOneCellPos.y * -1 + mMainPlayerPos.y;
    }

    public void SetMainPos(Vector2 mMainPlayerPos)
    {
        this.mMainPlayerPos = mMainPlayerPos;
    }

    public void BeginStartMove()
    {
        TargetPosition = temporary;
        StartMove = true;
    }

    private void FixedUpdate()
    {
        if (!StartMove) return;
        intervalTime += Time.fixedDeltaTime;
        if (intervalTime >= 0.1f)
        {
            if (Transform == null) return;
            Transform.localPosition = Vector3.Lerp(Transform.localPosition, TargetPosition, Speed);
            intervalTime = 0;

            if (Vector3.Distance(Transform.localPosition, TargetPosition) < 0.2f)
                StartMove = false;
        }
    }

    public void Dispose()
    {
        StartMove = false;
        Speed = 0.5f;
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        TargetPosition = Vector3.zero;
    }
}