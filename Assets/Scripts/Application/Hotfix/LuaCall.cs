using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

// 为lua提供C#方法调用的静态类
public static class LuaCall
{
    [LuaCallCSharp]
    public static GameObject InstantiateGo(string goName, Transform parent)
    {
        GameObject goPreab = Game.Instance.loadAB.hotfixDict[goName];
        return UnityEngine.Object.Instantiate(goPreab, parent);
    }
}
