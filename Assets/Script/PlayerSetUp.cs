using UnityEngine;
using Mirror;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerControler))]
public class PlayerSetUp : NetworkBehaviour
{
 [SerializeField]
 Behaviour[] componentsToDisable;


 [SerializeField]
 private string remoteLayerName = "RemotePlayer";

[SerializeField]
 private string dontDrawLayerName = "DontDraw";

 [SerializeField]
 private GameObject playerGraphics;

 [SerializeField]
 private GameObject playerNameplateGraphics;

 [SerializeField]
private GameObject playerUIPrefab;
[HideInInspector]
public GameObject playerUIInstance;




 private void Start() {
     if(!isLocalPlayer){
        DisableComponents();
        AssignRemoteLayer();

     }
     else
     {
       
    //desactiver la partie graphique du joueur local
        Util.SetLayerRecursively(playerGraphics,LayerMask.NameToLayer(dontDrawLayerName));
        Util.SetLayerRecursively(playerNameplateGraphics,LayerMask.NameToLayer(dontDrawLayerName));

        // création du UI du joueur local
        playerUIInstance = Instantiate(playerUIPrefab);
        //Config du UI
        PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
        if(ui == null){
            Debug.LogError("Pas de Component playerUI sur playerUIInstance");
        }
        else{
            ui.SetPlayer(GetComponent<Player>());
        }

    GetComponent<Player>().SetUp();


    CmdSetUsername(transform.name,UserAccountManager.LoggedInUsername);

        
     }

     

     
 }
 [Command]
     public void CmdSetUsername(string playerId, string username){
        Player player = GameManager.GetPlayer(playerId);
        if(player != null){
            Debug.Log(username+" has joined the game.");
            player.username = username;
        }
     }
 

 public override void OnStartClient(){
     base.OnStartClient();
     string netId = GetComponent<NetworkIdentity>().netId.ToString();
    Player player = GetComponent<Player>();
     GameManager.RegisterPlayer(netId,player);
 }

 private void DisableComponents(){
 for (int i = 0; i < componentsToDisable.Length; i++)
         {
             componentsToDisable[i].enabled = false;
         }
 }

 private void AssignRemoteLayer(){

     gameObject.layer = LayerMask.NameToLayer(remoteLayerName);

 }

 private void OnDisable() {

    Destroy(playerUIInstance);
    if(isLocalPlayer){
    GameManager.instance.SetSceneCameraActive(true);

    }
    GameManager.UnregisterPlayer(transform.name);
     
 }
}
