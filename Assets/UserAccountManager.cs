
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
  public static UserAccountManager instance;

  public string lobbySceneName = "Lobby";

  public static string LoggedInUsername;

  public void LogIn( Text username){
    LoggedInUsername = username.text;
    Debug.Log("Logged in as :"+ LoggedInUsername);
    SceneManager.LoadScene(lobbySceneName);
  }
  void Awake(){
      if(instance != null){
          Destroy(gameObject);
          return;
      }
      instance = this;
     DontDestroyOnLoad(this);
  }
}
