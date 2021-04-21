using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField]
    Text text;
    public void SetUp(string player, string source){
        text.text = source + " killed "+player;
    }
}
