using UnityEngine;

[CreateAssetMenu(fileName = "weaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject {
    
    public string _name = "Submachine Gun";
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0f;
    public GameObject graphics;
}
