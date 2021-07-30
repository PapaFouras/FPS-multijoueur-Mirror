using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour
{
    [SerializeField]
    Text usernameText;
    [SerializeField]
    private RectTransform healthBarFill;
    [SerializeField]
    private Player player;
  
    void Update()
    {
        usernameText.text = player.username;
        healthBarFill.localScale = new Vector3(player.GetHealthPct(),1f,1f);
    }
}
