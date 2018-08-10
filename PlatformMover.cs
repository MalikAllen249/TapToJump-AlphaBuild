using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour {

    private Rigidbody2D m_rb2d;
    public float m_speed;

    private void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();

        m_rb2d.velocity = new Vector2(-1.0f, m_rb2d.velocity.y) * m_speed;
    }
    
}
