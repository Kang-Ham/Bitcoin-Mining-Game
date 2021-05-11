using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    public GameSystem scriptGameSystem;

    public string authCode;
    Firebase.Auth.FirebaseAuth auth = null;

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
           .RequestServerAuthCode(false)
           .Build();

        PlayGamesPlatform.InitializeInstance(config);
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

    //¾÷Àû
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
}
