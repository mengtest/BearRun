using System.IO;
using System.Net;
using UnityEngine;
using XLua;

public class HotFix : MonoSingleton<HotFix>
{
    // 新建一个唯一的全局Lua虚拟机
    private LuaEnv luaEnv;
    public string path;

    public void StartHotFix()
    {
        path = Application.persistentDataPath + "/";
        // 初始化lua虚拟机
        luaEnv = new LuaEnv();
        Debug.Log("lua start");
        luaEnv.AddLoader(myLoader);
        luaEnv.DoString("require 'main'");
    }

    private byte[] myLoader(ref string filePath)
    {
        string luaPath = path + filePath + ".lua.txt";
        return File.ReadAllBytes(luaPath);
    }

    private void OnDisable()
    {
        luaEnv.DoString("require 'dispose'");
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}