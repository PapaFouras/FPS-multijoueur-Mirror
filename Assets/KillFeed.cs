using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeed : MonoBehaviour
{
    [SerializeField]
    GameObject killFeedItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onPlayerKilledCallback += OnKilled;
    }

  public void OnKilled(string player, string source){
      GameObject go = Instantiate(killFeedItemPrefab, transform);
      go.GetComponent<KillFeedItem>().SetUp(player,source);
      Destroy(go,4f);
      
  }
}
