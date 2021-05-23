using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using System;
using LitJson;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SystemInfo
{
    public double currentBtc; //float을 json에 저장하는 것이 지원이 안 됨
    public UInt64 currentMoney;
    public int pcCount;
    public int currentGpuLevel;
    public DateTime recentlyTerminatedAt;
    public double currentOverclockLevel;
    public int currentBgmVolume;
    public int currentSoundEffectVolume;
    public Boolean currentNotificationStatus;

    public SystemInfo(double _currentBtc, UInt64 _currentMoney, int _pcCount, int _currentGpuLevel, DateTime _recentlyTerminatedAt, double _currentOverclockLevel, int _currentBgmVolume, int _currentSoundEffectVolume, Boolean _currentNotificationStatus)
    {
        currentBtc = _currentBtc;
        currentMoney = _currentMoney;
        pcCount = _pcCount;
        currentGpuLevel = _currentGpuLevel;
        recentlyTerminatedAt = _recentlyTerminatedAt;
        currentOverclockLevel = _currentOverclockLevel;
        currentBgmVolume = _currentBgmVolume;
        currentSoundEffectVolume = _currentSoundEffectVolume;
        currentNotificationStatus = _currentNotificationStatus;
    }
}

public class GooglePlayManager : MonoBehaviour
{
    private GameSystem scriptGameSystem;
    private PcPanel scriptPcPanel;
    private Setting scriptSetting;
    private GooglePlayManager scriptGooglePlayManager;
    private Msgbox scriptMsgbox;
    private TopAreaImage scriptTopAreaImage;
    private Cartoon scriptCartoon;

    private float X_VELOCITY;
    private float Y_VELOCITY;

    private SystemInfo systemInfo;

    GameObject loadedBtcPanel;
    private Image loadedBtcBackgroundImageImage;
    private Text loadedBtcTextText;
    public GameObject itemBtc;

    private string authCode;
    Firebase.Auth.FirebaseAuth auth = null;

    private string dataToSave = null;
    private string loadedData = null;

    public string SAVE_FILE_NAME = "SystemInfo";

    // Start is called before the first frame update
    void Start()
    {
        scriptMsgbox = GameObject.Find("EventSystem").GetComponent<Msgbox>();

        SetLoadingPanelActive(true);
        try
        {
            ConfigGooglePlayGameClient();
        }
        catch
        {
            AfterSetDataToGame(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        SaveToCloud();
    }

    public void ConfigGooglePlayGameClient()
    {
        SetLoadingContent("ConfigGooglePlayGameClient");

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
           .EnableSavedGames()
           .RequestServerAuthCode(false)
           .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();

        LoginGooglePlayGame();
    }

    public void LoginGooglePlayGame()
    {
        SetLoadingContent("LoginGooglePlayGame");

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                ConnectFirebaseWithGooglePlayGame();

                LoadFromCloud();
            }
            else
            {
                AfterSetDataToGame(true);
            }
        });
    }

    public void ConnectFirebaseWithGooglePlayGame()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

            //After Login
            SetBtcAchievement();
        });
    }

    public void LogoutGooglePlayGame()
    {
        auth.SignOut();
    }

    public void SetPcAchievement()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        
        if(auth == null)
        {
            return;
        }

        if (scriptGameSystem.currentPcList.Count >= 49)
        {
            Social.ReportProgress(GPGSIds.achievementPc4, 100f, null);
        }
        else if (scriptGameSystem.currentPcList.Count >= 33)
        {
            Social.ReportProgress(GPGSIds.achievementPc3, 100f, null);
        }
        else if (scriptGameSystem.currentPcList.Count >= 17)
        {
            Social.ReportProgress(GPGSIds.achievementPc2, 100f, null);
        }
        else if (scriptGameSystem.currentPcList.Count >= 1)
        {
            Social.ReportProgress(GPGSIds.achievementPc1, 100f, null);
        }
    }

    public void SetBtcAchievement()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        if (auth == null)
        {
            return;
        }

        if (scriptGameSystem.currentBtc >= 1000)
        {
            Social.ReportProgress(GPGSIds.achievementBtc4, 100f, null);
        }
        else if (scriptGameSystem.currentBtc >= 100)
        {
            Social.ReportProgress(GPGSIds.achievementBtc3, 100f, null);
        }
        else if (scriptGameSystem.currentBtc >= 10)
        {
            Social.ReportProgress(GPGSIds.achievementBtc2, 100f, null);
        }
        else if (scriptGameSystem.currentBtc >= 1)
        {
            Social.ReportProgress(GPGSIds.achievementBtc1, 100f, null);
        }
    }

    public void OpenStoreLink()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.KangHam.BitcoinMiningGame");
#endif
    }

    public void LoadFromCloud()
    {
        SetLoadingContent("LoadFromCloud");

        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            SAVE_FILE_NAME, //name of file.
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            OnFileOpenToLoad);
        }
    }

    private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        SetLoadingContent("OnFileOpenToLoad");

        if (status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
        }
        else
        {
            SetLoadingContent("SavedGameRequestStatus File Open Error: " + status);
            AfterSetDataToGame(true);
        }
    }

    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        SetLoadingContent("OnGameLoad");

        if (status == SavedGameRequestStatus.Success)
        {
            ProcessCloudData(bytes);
        }
        else
        {
            SetLoadingContent("SavedGameRequestStatus Save Error: " + status);
            AfterSetDataToGame(true);
        }
    }

    private void ProcessCloudData(byte[] cloudData)
    {
        SetLoadingContent("ProcessCloudData");

        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptPcPanel) scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();

        string progress = BytesToString(cloudData);
        loadedData = progress;

        JsonData jsonSystemInfo = JsonMapper.ToObject(loadedData);

        scriptGameSystem.currentMoney = Convert.ToUInt64(jsonSystemInfo["currentMoney"].ToString());
        scriptGameSystem.currentBtc = Convert.ToSingle(jsonSystemInfo["currentBtc"].ToString());
        scriptGameSystem.currentGpuLevel = Convert.ToInt16(jsonSystemInfo["currentGpuLevel"].ToString());
        scriptGameSystem.currentOverclockLevel = Convert.ToSingle(jsonSystemInfo["currentOverclockLevel"].ToString());
        scriptGameSystem.currentBgmVolume = Convert.ToInt16(jsonSystemInfo["currentBgmVolume"].ToString());
        scriptGameSystem.currentSoundEffectVolume = Convert.ToInt16(jsonSystemInfo["currentSoundEffectVolume"].ToString());
        scriptGameSystem.currentNotificationStatus = Convert.ToBoolean(jsonSystemInfo["currentNotificationStatus"].ToString());
        
        // PC Load & 시간에 따라 Btc 추가
        DateTime recentlyTerminatedAt = Convert.ToDateTime(jsonSystemInfo["recentlyTerminatedAt"].ToString());
        TimeSpan timeDifference = DateTime.Now - recentlyTerminatedAt;

        if (timeDifference > new TimeSpan(scriptGameSystem.MAX_BTC_STORING_HOUR, 0, 0)) // 최대 3시간까지만 저장
        {
            timeDifference = new TimeSpan(scriptGameSystem.MAX_BTC_STORING_HOUR, 0, 0);
        }

        float btcToGet = 0f;
        for (int i = 0; i < Convert.ToInt16(jsonSystemInfo["pcCount"].ToString()); i++)
        {
            Pc scriptNewPcPanel = scriptPcPanel.AddNewPc();
            btcToGet = (timeDifference.Seconds + timeDifference.Minutes * 60 + timeDifference.Hours * 3600) * scriptNewPcPanel.btcPerSecond * scriptGameSystem.GPU_RATES[scriptGameSystem.currentGpuLevel];
            scriptGameSystem.currentBtc += btcToGet;
        }

        // 부재하는 동안 얼마 벌었는지 알려주는 메시지 표시  
        ShowLoadedBtcPanel(btcToGet, timeDifference.Seconds + timeDifference.Minutes * 60 + timeDifference.Hours * 3600);

        AfterSetDataToGame(false);
    }

    private void AfterSetDataToGame(bool isFirst)
    {
        if (!scriptSetting) scriptSetting = GameObject.Find("EventSystem").GetComponent<Setting>();
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptCartoon) scriptCartoon = GameObject.Find("EventSystem").GetComponent<Cartoon>();
        if (!scriptPcPanel) scriptPcPanel = GameObject.Find("EventSystem").GetComponent<PcPanel>();
        if (!scriptTopAreaImage) scriptTopAreaImage = GameObject.Find("TopAreaImage").GetComponent<TopAreaImage>();

        // Btc, Money 업데이트
        scriptTopAreaImage.UpdateCurrentBtcText();
        scriptTopAreaImage.UpdateCurrentMoneyText();

        // 소리 세팅에 따라서 설정
        scriptSetting.SetSoundAfterDataLoad();

        // 게임 처음 시작했을 때, 기본 pc 1개 지급
        if (scriptGameSystem.currentPcList.Count == 0) scriptPcPanel.AddNewPc();

        // 로딩 화면 해제
        SetLoadingPanelActive(false);

        // 카툰 보여주기
        if (isFirst)
        {
            scriptCartoon.SetCartoonActive(true);
        }
    }


    private void SetLoadingPanelActive(bool flag)
    {
        GameObject loadingPanel = GameObject.Find("PopupPanels").transform.Find("LoadingPanel").gameObject;
        loadingPanel.SetActive(flag);
    }

    public void SaveToCloud()
    {
        if (!scriptGameSystem) scriptGameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        if (!scriptGooglePlayManager) scriptGooglePlayManager = GameObject.Find("EventSystem").GetComponent<GooglePlayManager>();

        systemInfo = new SystemInfo(Convert.ToDouble(scriptGameSystem.currentBtc), scriptGameSystem.currentMoney, scriptGameSystem.currentPcList.Count, scriptGameSystem.currentGpuLevel, DateTime.Now, Convert.ToDouble(scriptGameSystem.currentOverclockLevel), scriptGameSystem.currentBgmVolume, scriptGameSystem.currentSoundEffectVolume, scriptGameSystem.currentNotificationStatus);
        JsonData jsonSystemInfo = JsonMapper.ToJson(systemInfo);
        string data = jsonSystemInfo.ToString();

        if (Social.localUser.authenticated)
        {
            this.dataToSave = data;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
                SAVE_FILE_NAME,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnFileOpenToSave);
        }
    }

    private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            byte[] data = StringToBytes(dataToSave);

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, data, OnGameSave);
        }
    }

    private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            Debug.Log("SavedGameRequestStatus Save Error" + status);
        }
    }

    private byte[] StringToBytes(string stringToConvert)
    {
        return Encoding.UTF8.GetBytes(stringToConvert);
    }
    
    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    private void ShowLoadedBtcPanel(float btcToGet, int timeDifferenceSeconds)
    {
        loadedBtcPanel = GameObject.Find("PopupPanels").transform.Find("LoadedBtcPanel").gameObject;
        Text loadedBtcPanelText = loadedBtcPanel.transform.Find("LoadedBtcText").GetComponent<Text>();

        loadedBtcPanel.SetActive(true);
        loadedBtcPanelText.text = "그동안 " + btcToGet.ToString("0." + new string('#', 8)) + "BTC를 채굴하였습니다!";

        loadedBtcBackgroundImageImage = loadedBtcPanel.transform.Find("LoadedBtcBackgroundImage").GetComponent<Image>();
        loadedBtcTextText = loadedBtcPanelText.GetComponent<Text>();

        StartCoroutine(SetLoadedBtcPanelFadeOut(3f, 1f));

        int coinCountToPopUp = Convert.ToInt16((timeDifferenceSeconds / 10800f) * 80f);
        for (int i = 1; i < coinCountToPopUp; i++)
        {
            GameObject instantObject = Instantiate(itemBtc, new Vector2(Screen.width / 2, Screen.height / 2), transform.rotation) as GameObject;
            instantObject.transform.SetParent(GameObject.Find("ClickerButton").transform);

            Rigidbody2D itemRigid = instantObject.GetComponent<Rigidbody2D>();
            X_VELOCITY = Random.Range(-60.0f, 60.0f);
            Y_VELOCITY = Random.Range(220.0f, 340.0f);
            Vector2 velocityVector = new Vector2(X_VELOCITY, Y_VELOCITY);
            itemRigid.AddForce(velocityVector, ForceMode2D.Impulse);
            Destroy(instantObject.gameObject, 15.0f);
        }
    }

    IEnumerator SetLoadedBtcPanelFadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        Color curColor = loadedBtcBackgroundImageImage.color;
        while (curColor.a > 0f)
        {
            curColor.a -= Time.deltaTime / duration;
            loadedBtcBackgroundImageImage.color = curColor;
            loadedBtcTextText.color = curColor;

            yield return null;
        }

        loadedBtcPanel.SetActive(false);
    }

    private void SetLoadingContent(string content)
    {
        GameObject.Find("LoadingContent").GetComponent<Text>().text = content;
    }
}
