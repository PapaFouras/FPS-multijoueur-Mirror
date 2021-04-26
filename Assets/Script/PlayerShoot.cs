using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;
    public WeaponData currentWeapon;


    
    // Start is called before the first frame update
    void Start()
    {

        if(cam == null){
            Debug.LogError("pas de cam renseignée sur le système de tir");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    void Update() {
        if(PauseMenu.isOn == true){
            return;
        }

        currentWeapon = weaponManager.GetCurrentWeapon();
        if(currentWeapon.fireRate <= 0f){
            if(Input.GetButtonDown("Fire1")){

            Shoot();
        }
        }else{
            if(Input.GetButtonDown("Fire1")){
                InvokeRepeating("Shoot",0f,1f / currentWeapon.fireRate);
            }else if (Input.GetButtonUp("Fire1")){
                CancelInvoke("Shoot");
            }

        }
        
    }

    //fonction appelé sur le server losque le joueur tire
    [Command]
    private void CmdOnShoot(){

        RpcDoShootEffect();
    }

    [Command]
    private void CmdOnHit(Vector3 pos, Vector3 normal){
        RpcDoHitEffect(pos,normal);
    }

    // fait apparaitre les effets de tirs chez tous les clients

    [ClientRpc]
    private void RpcDoShootEffect(){
        weaponManager.GetCurrentWeaponGraphics().muzzleFlash.Play();


    }
    [ClientRpc]
    private void RpcDoHitEffect(Vector3 pos, Vector3 normal){
       GameObject hitObject = Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
       Destroy(hitObject, 2f);


    }

    [Client]
    private void Shoot(){

        if(!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();
        
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask)){
            if(hit.collider.tag == "Player"){
                CmdPlayerShot(hit.collider.name, currentWeapon.damage, transform.name);
                

            }
            CmdOnHit(hit.point,hit.normal);
        }

    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage, string sourceID){
       // Debug.Log(playerId + " a été touché");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage,sourceID);


    }
}
