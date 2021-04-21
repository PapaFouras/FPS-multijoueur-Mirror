using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    private void EquipWeapon(PlayerWeapon _weapon){
        currentWeapon = _weapon;
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

    public PlayerWeapon GetCurrentWeapon(){
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentWeaponGraphics(){
        return currentGraphics;
    }

     

   
}
