using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{

    [SerializeField]
    private WeaponData theWeapon;
    private GameObject pickUpGraphics;

    [SerializeField]
    private float resetDelay = 30f;

    private bool canPickUp = false;
    // Start is called before the first frame update
    void Start()
    {
        ResetWeapon();
    }

    void ResetWeapon(){
        pickUpGraphics = Instantiate(theWeapon.graphics,transform);
        pickUpGraphics.transform.position = transform.position;
        canPickUp = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && canPickUp){
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            EquipNewWeapon(weaponManager);
        }
    }

    void EquipNewWeapon(WeaponManager weaponManager){
        Destroy(weaponManager.GetCurrentWeaponGraphics().gameObject);
        weaponManager.EquipWeapon(theWeapon);
        canPickUp = false;
        Destroy(pickUpGraphics);
        StartCoroutine(DelayResetWeapon());
    }

    IEnumerator DelayResetWeapon(){
        yield return new WaitForSeconds(resetDelay);
        ResetWeapon();
    }
    
}
