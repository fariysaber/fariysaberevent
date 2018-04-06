using System;
using System.Collections.Generic;

public enum UILayer
{
    LowLayer = 1,
    MiddleLayer = 2,
    HighLayer = 3,
    TopLayer = 4,
    User = 5,
}
public enum OpenState
{
    Open = 1,
    Close = 2,
}
public enum UILoadingState
{
    Init = 1,
    Loading = 2,
    LoadComplete = 3,
    LoadError = 4,
    UnLoad = 5,
    Destroy = 6,
}