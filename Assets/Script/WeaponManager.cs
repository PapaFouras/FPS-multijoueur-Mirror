using UnityEngine;
using Mirror;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private WeaponData primaryWeapon;

    private WeaponData currentWeapon;

    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [HideInInspector]
    public int currentMagazineSize;

    public bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    private void EquipWeapon(WeaponData _weapon){
        currentWeapon = _weapon;
        currentMagazineSize = _weapon.magazineSize;
        GameObject weaponIns = Instantiate(_weapon.graphics,weaponHolder.position,weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if(currentGraphics == null){
            Debug.LogError("Pas de script weapon graphics sur l'arme: "+weaponIns.name);
        }

        if(isLocalPlayer){
        Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));

        }
    }

    public WeaponData GetCurrentWeapon(){
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentWeaponGraphics(){
        return currentGraphics;
    }

    public IEnumerator Reload(){
        if(isReloading){
            yield break;
        }

        Debug.Log("reloading ...");
        isReloading = true;
        CmdOnReload();
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentMagazineSize = currentWeapon.magazineSize;

        isReloading = false;
        Debug.Log("reloading finished");

    }

    [Command]
    void CmdOnReload(){
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload(){
        Animator animator = currentGraphics.GetComponent<Animator>();
        if(animator != null){
            animator.SetTrigger("Reload");
        }
    }
     

   
}
