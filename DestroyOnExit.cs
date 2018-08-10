using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : MonoBehaviour {

    //Despawns objects outside the boundary of the screen 
        //Using less resources

    private void OnTriggerExit2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
