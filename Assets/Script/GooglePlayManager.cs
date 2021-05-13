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

        if (scriptGameSystem.currentPcList.Count >= 1)
        {
            Social.ReportProgress(GPGSIds.achievementPc1, 100f, null);
        }
    }

    public void OpenStoreLink()
    {
        //TODO: iOS 개발 시 앱스토어로 링크
        Application.OpenURL("market://details?id=com.KangHam.BitcoinMiningGame");

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

        scriptJson.OnDataLoad(loadedData);
    }

    private byte[] StringToBytes(string stringToConvert)
    {
        return Encoding.UTF8.GetBytes(stringToConvert);
    }

    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}
