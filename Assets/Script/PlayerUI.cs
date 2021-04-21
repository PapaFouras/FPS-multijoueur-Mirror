using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerControler controler;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreBoard;

    public void SetControler(PlayerControler _controler){
        controler = _controler;
    }

    private void Start() {
        PauseMenu.isOn = false;
    }

    private void Update() {
       SetThrusterFuelAmount(controler.GetThrusterFuelAmount()); 

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


  
}
