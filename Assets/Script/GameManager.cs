
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{


    private const string playerIdPrefix = "Player";
   private static Dictionary<string, Player> players = new Dictionary<string,Player>();

   public static GameManager instance;

   [SerializeField]
   private GameObject sceneCamera;

   public delegate void OnPlayerKilledCallback(string player, string source);
   public OnPlayerKilledCallback onPlayerKilledCallback;

   private void Awake() {
     if(instance == null){
       instance = this;
       return;
     }

     Debug.LogError("Il y a plusieurs instances de GameManager");
   }

   public void SetSceneCameraActive(bool isActive){
     if(sceneCamera == null){
       return;
     }
     sceneCamera.SetActive(isActive);
   }

public MatchSettings matchSettings;

  


  public static void RegisterPlayer(string netId, Player player)
  {
    string playerId = playerIdPrefix + netId;
    players.Add(playerId,player);
    player.transform.name = playerId;
  }

  public static void UnregisterPlayer(string playerId){
    players.Remove(playerId);
  }

  public static Player GetPlayer(string playerId){
    return players[playerId];
  }

  public static Player[] GetAllPlayers(){
      return players.Values.ToArray();
  }

}
