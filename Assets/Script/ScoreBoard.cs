using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
[SerializeField]
private GameObject playerScoreBoardItem;


[SerializeField]
private Transform playerScoreBoardList;


   private void OnEnable() {
       //recuperer une array de tous les joueurs du serveurs
        Player[] players = GameManager.GetAllPlayers();

        foreach (Player player in players)
        {
            GameObject itemGO = Instantiate(playerScoreBoardItem,playerScoreBoardList);
            PlayerScoreBoardItem item = itemGO.GetComponent<PlayerScoreBoardItem>();
            item.Setup(player);
            Debug.Log(player.name+" "+player.kills+" "+player.deaths);
        }
       //boucle sur l'array et mise en place d'une ligne pour chaque joueur + remplissage de la ligne 
   }
   private void OnDisable() {
       
       //vider la liste des joueurs
       foreach (Transform child in playerScoreBoardList)
       {
           Destroy(child.gameObject);
       }
   }
}
