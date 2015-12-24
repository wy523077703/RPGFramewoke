﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LuaInterface;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool isLowDevice;
    public float npcRefreshTime = 0.05f;
    private bool initRes = true;

    public Text text;

    

    void Awake() {
        gameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        InitManager();
        InitObjPool();
        ResourceManager.getInstance().Init();
        if (SystemInfo.systemMemorySize < Constants.LIMIT_MEMORY_SIZE || SystemInfo.processorCount < Constants.PROCESSOR_COUNT)
        {
            isLowDevice = true;
            npcRefreshTime = 0.09f;
        }
    }

    private void InitManager() {         
        gameObject.AddComponent<CheckUpdate>();
        gameObject.AddComponent<NetworkManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<SoundManager>();
        gameObject.AddComponent<TimerManager>();
        gameObject.AddComponent<LuaManager>();
        //TODO other
    }


    private void InitObjPool() {
        GameObject objPoll = new GameObject("ObjectPool");
        objPoll.AddComponent<ObjectPool>();
        DontDestroyOnLoad(objPoll);
    }

    void Update() {
        if (AssetBundleManager.AssetBundleManifestObject != null) {
            if (initRes) {
                initRes = false;
                ResourceManager.getInstance().StartDownLoad();
            }
        }
            

    }

    private void OnEnable()
    {
        Application.logMessageReceived += UploadLog;
    }

    private void OnDisable() {
        Application.logMessageReceived -= UploadLog;
    }

    private void UploadLog(string message, string stacktrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                
                break;

            case LogType.Warning:
                
                break;

            case LogType.Log:
                
                break;

            case LogType.Exception:
                
                break;
        }
    }





//-----------------------------------test--------------------------------------------
    public void HotUpdateResource() {
        AssetBundleService.getInstance().LoadAsset("prefabs/object/cube.unity3d", "cube", testPrefabs);
    }

    public void HotUpdateScript() {
        object[] results = ManagerStore.luaManager.CallMethod("GameManager", "hello");
        text.text = results[0].ToString();
    }

    public void LoadTxt() {
        CSkillLoader skillLoader = CStaticDownload<CSkillLoader>.getInstance().GetStaticInfo(100);
        GLog.Log(skillLoader.GetSkillName());
    }

    private void testPrefabs(string assetName,UnityEngine.Object obj)
    {
        GameObject go = Instantiate(obj as GameObject);
        go.transform.position = Vector3.zero;
    }

//-----------------------------------test--------------------------------------------
}
