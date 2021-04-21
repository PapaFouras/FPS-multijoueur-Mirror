using UnityEngine;

public class Util : MonoBehaviour
{
    public static void SetLayerRecursively(GameObject obj, int newLayer){
    obj.layer = newLayer;

    foreach (Transform child in obj.transform)
    {
        SetLayerRecursively(child.gameObject,newLayer);
    }
 }
  
}
