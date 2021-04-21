using UnityEngine;

public class CursorManager : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
         if(Cursor.lockState != CursorLockMode.None){
                Cursor.lockState = CursorLockMode.None;
            }
        
    }
}
