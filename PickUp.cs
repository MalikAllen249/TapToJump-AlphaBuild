using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && this.gameObject.tag == "Coin")
        {
            other.gameObject.GetComponent<PlayerMovement>().UpdatePlayerWallet(1);
            Destroy(this.gameObject);
        }else if(other.gameObject.tag == "Player" && this.gameObject.tag == "DoubleJump")
        {
            other.gameObject.GetComponent<PlayerMovement>().m_doubleJumpEnabled = true;
            Destroy(this.gameObject);
        }
    }
}
