public enum MainEvent
{
    None,
    OnDragLoopScrollItemState = 533, //拖动循环滚动状态
    EnhancelScrollViewOnDragEnd = 567,
    ShowOrCloseGaussian,//打开或关闭场景模糊
    
    ClearCSMapManager,//清除CSGameManager
    DestroyCSScene,//销毁 CSScene
    CSSceneInit,//初始化主场景
    StartEnterScene,//切换场景
    PreLoadScene,//加载场景资源
    PreloadingScaleMap,//加载缩略图
    ShowUILoading,//显示读条页面
    InitCSGameManager,//初始化单利
    DestroyCSGameManager,//销毁单利
    PreLoadingScene,//加载场景

    Avatar_AttachBottom,        //重新挂载bottom
    Avatar_AttachBottomNPC,        //重新挂载bottomNPC
    CloseSelectionPanel,       //关闭玩家/怪物选中面板
    MainPlayer_DirectionChange, //角色方向改变
}