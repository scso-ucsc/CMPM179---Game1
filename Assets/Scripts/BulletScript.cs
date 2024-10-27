using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Civilian")
        {
            GameManager.instance.gameOver();
            this.gameObject.SetActive(false);
        }
        else if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
