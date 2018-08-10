using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D m_rb2d;

    public Transform m_groundCheck;
    public bool m_grounded = false;
    float m_groundCheckRadius = 0.1f;
    public LayerMask m_groundLayer;

    [Range(1.0f, 10.0f)]
    public float m_jumpHeightMultiplier;
    float m_jumpForce = 150.0f;

    [HideInInspector]
    public static int m_playerWallet;
    public Text m_walletTotal;

    public bool m_doubleJumpEnabled = false;
    public bool m_coinSprint = false;
    

    private void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jump();
        m_walletTotal.text = "Dollars: $" + m_playerWallet; //UI displaying player wallet total
    }

    private void FixedUpdate()
    {
        m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, 
            m_groundCheckRadius, m_groundLayer);
    }

    public void UpdatePlayerWallet(int choice)
    {
        if (choice == 1)
            m_playerWallet += 10;

        /*FUTURE_DEVELOPMENT
         * Choice == 2 for making aesthetic purchases inside the in-game store*/
    }

    void Jump()
    {
        if (m_grounded && Input.GetMouseButtonDown(0))
        {
            m_rb2d.AddForce(new Vector2(m_rb2d.velocity.x, m_jumpForce * m_jumpHeightMultiplier));
        }
        else if (m_doubleJumpEnabled && Input.GetMouseButtonDown(0) && !m_grounded)
        {
            /*In order for the double jump to work consistently, smoothly and predictably
             * When the player double jumps the velocity of the player is set back to zero 
             * Then a second jump is intiated providing the player with a second jump */
             
            if (m_rb2d.velocity.y != 0f)
            {
                m_rb2d.velocity = new Vector2(m_rb2d.velocity.x, 0f);
                m_rb2d.AddForce(new Vector2(m_rb2d.velocity.x, m_jumpForce * m_jumpHeightMultiplier));
            }

            m_doubleJumpEnabled = false; //After a successful double jump, double jump will be disabled 
                                            //Until next double jump is activated
        }
        else if (m_doubleJumpEnabled && m_grounded && Input.GetMouseButtonDown(0))
        {
            m_doubleJumpEnabled = true;
        }
    }
    
}
