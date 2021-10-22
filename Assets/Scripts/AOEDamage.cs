using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    float radius;
    float damage;
    LayerMask layer;
    public float fadeInTime;
    public float fadeOutTime;
    public CircleMesh circle;
    float time;

    public float impactForce;

    public void Init(float radius, float damage, LayerMask layer)
    {
        this.radius = radius;
        this.damage = damage;
        this.layer = layer;
        this.time = 0;

        StartCoroutine(DamageSequence());
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void Damage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach(Collider c in colliders)
        {
            Character character = c.GetComponent<Character>();
            if (character)
            {
                //Debug.Log("explode");
                Vector3 normal = Vector3.Normalize(character.gameObject.transform.position - transform.position);
                Vector2 holdingForce = new Vector2(normal.x, normal.z) * impactForce;
               // Debug.Log(holdingForce);
                character.gameObject.transform.position = character.gameObject.transform.position + new Vector3(holdingForce.x, 0, holdingForce.y);
                character.hp -= damage;
            }
        }
        //Destroy(gameObject);
    }

    IEnumerator DamageSequence()
    {
        circle.CreateCircleMesh(radius);
        Damage();
        yield return new WaitForSeconds(fadeOutTime);
        Destroy(gameObject);
    }
}
