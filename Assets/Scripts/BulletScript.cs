using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Civilian")
        {
            AudioManager.instance.playImpactAudio(this.gameObject.transform.position, "CIVILIAN");

            ParticleManager.instance.emitDeathParticles(this.gameObject.transform.position);
            this.GetComponent<TrailRenderer>().enabled = false;
            GameManager.instance.gameOver();
            this.gameObject.SetActive(false);
        }
        else if (collider.gameObject.tag == "Player")
        {
            AudioManager.instance.playImpactAudio(this.gameObject.transform.position, "PLAYER");

            ParticleManager.instance.emitBulletParticles(this.gameObject.transform.position);
            this.GetComponent<TrailRenderer>().enabled = false;
            this.gameObject.SetActive(false);
        }
        else if (collider.gameObject.tag == "Bullet")
        {
            AudioManager.instance.playImpactAudio(this.gameObject.transform.position, "BULLET");

            ParticleManager.instance.emitBulletParticles(this.gameObject.transform.position);
            this.GetComponent<TrailRenderer>().enabled = false;
            this.gameObject.SetActive(false);
        }
        else if (collider.gameObject.tag == "Wall") //collider.gameObject.tag == "Wall"
        {
            this.GetComponent<TrailRenderer>().enabled = false;
            this.gameObject.SetActive(false);
        }
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
