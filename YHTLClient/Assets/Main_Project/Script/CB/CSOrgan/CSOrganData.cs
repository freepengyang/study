using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSOrganData
{
    public CSAction action;

    public BoxCollider box;

    public int avatarType;

    public bool isDataSplit = false;

    public EShareMatType matType = EShareMatType.Normal;

    public CSOrganData(CSAction _action, BoxCollider _box, int _avatarType, bool _isDataSplit, EShareMatType _matType)
    {
        this.action = _action;
        this.box = _box;
        this.avatarType = _avatarType;
        this.isDataSplit = _isDataSplit;
        this.matType = _matType;
    }
}
