using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;
    
    [SerializeField]
    private RectTransform healthBarFill;

    private PlayerControler controler;
    private Player player;

    private WeaponManager weaponManager;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreBoard;

    [SerializeField]
    private Text ammoText;

    

    public void SetPlayer(Player _player){
        player = _player;
        controler = player.GetComponent<PlayerControler>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    private void Start() {
        PauseMenu.isOn = false;
    }

    private void Update() {
       SetThrusterFuelAmount(controler.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthPct());
        SetAmmoAmount(weaponManager.currentMagazineSize);

       if(Input.GetKeyDown(KeyCode.Escape)){
            TogglePauseMenu();
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            scoreBoard.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab)){
            scoreBoard.SetActive(false);
        }
    }

    public void TogglePauseMenu(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }
    void SetThrusterFuelAmount(float ammount){
        thrusterFuelFill.localScale = new Vector3(1f,ammount,1f);
    }
    void SetHealthAmount(float ammount){
        healthBarFill.localScale = new Vector3(1f,ammount,1f);
    }
    void SetAmmoAmount(int _ammount){
        ammoText.text = _ammount.ToString();
    }


  
}
