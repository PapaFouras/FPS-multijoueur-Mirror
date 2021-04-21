using UnityEngine;
using Mirror;

public class PauseMenu : NetworkBehaviour
{
    public static bool isOn = false;

    private NetworkManager networkManager;

    public void LeaveRoomButton(){
        if(isClientOnly){
            networkManager.StopClient();
        }
        else{
            networkManager.StopHost();
        }
    }

    private void Start() {
        networkManager = NetworkManager.singleton;
    }
   

    
}
