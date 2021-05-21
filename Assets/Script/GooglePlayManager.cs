using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using System;

public class GooglePlayManager : MonoBehaviour
{
    public GameSystem scriptGameSystem;
    public Json scriptJson;

    public string authCode;
    Firebase.Auth.FirebaseAuth auth = null;

    public string dataToSave = null;
    public string loadedData = null;

    public string SAVE_FILE_NAME = "SystemInfo";

    // Start is called before the first frame update
    void Start()
    {
        SetLoadingPanelActive(true);
        ConfigGooglePlayGameClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigGooglePlayGameClient()
    {
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
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                ConnectFirebaseWithGooglePlayGame();

                LoadFromCloud();
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
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            SAVE_FILE_NAME, //name of file.
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            OnFileOpenToLoad);
        }
    }

    public void SaveToCloud(string dataToSave)
    {
        if (Social.localUser.authenticated)
        {
            this.dataToSave = dataToSave;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
                SAVE_FILE_NAME,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnFileOpenToSave);
        }
    }

    private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
        }
        else
        {
            Debug.Log("SavedGameRequestStatus File Open Error" + status);
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

    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ProcessCloudData(bytes);
        }
        else
        {
            Debug.Log("SavedGameRequestStatus Save Error" + status);
        }
    }

    private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            Debug.Log("SavedGameRequestStatus Save Error" + status);
        }
    }

    private void ProcessCloudData(byte[] cloudData)
    {
        if (!scriptJson) scriptJson = GameObject.Find("Json").GetComponent<Json>();

        if (cloudData == null)
        {
            Debug.Log("No Data");
            return;
        }

        string progress = BytesToString(cloudData);
        loadedData = progress;

        // 로드 후 액션
        scriptJson.OnDataLoad(loadedData);
        SetLoadingPanelActive(false);
    }

    private byte[] StringToBytes(string stringToConvert)
    {
        return Encoding.UTF8.GetBytes(stringToConvert);
    }

    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    private void SetLoadingPanelActive(bool flag)
    {
        GameObject loadingPanel = GameObject.Find("PopupPanels").transform.Find("LoadingPanel").gameObject;
        loadingPanel.SetActive(flag);
    }
}
