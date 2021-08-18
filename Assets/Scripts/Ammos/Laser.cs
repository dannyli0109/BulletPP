using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Ammo
{
    public LineRenderer thisLineRenderer;
    public BoxCollider ourCollider;

    Vector3 randomOffsetDirection;
    float randomOffSet;


    public float lengthDecreaseSpeed;
    public float widthDecreaseSpeed;

    float currentLaserLength;
    float currentLaserWidth;

    void Start()
    {
        randomOffsetDirection = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        randomOffSet = Random.Range(0, 3);
        currentLaserLength = owner.laserStats.maxLaserLength.value;
        currentLaserWidth = owner.laserStats.maxLaserWidth.value;

        EventManager.current.onAmmoDestroy += OnLaserDestroy;
    }
    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (owner)
        //{
        //    if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        //    {
        //        // make sure the bullet is not hitting itself
        //        EventManager.current.OnAmmoHit(owner, other.gameObject);
        //    }
        //    else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //    {
        //        Debug.Log("enem " + owner.laserStats.damage.value * Time.deltaTime);
        //        EventManager.current.OnLaserHit(owner.laserStats.damage.value * Time.deltaTime, owner, other.gameObject);
        //    }
        //}

        HandleAmmoHit(other);
    }

    void Update()
    {
        bornTime += Time.deltaTime;
        if (bornTime >= owner.laserStats.travelTime.value)
        {
            currentLaserLength -= Time.deltaTime * lengthDecreaseSpeed;
            currentLaserWidth -= Time.deltaTime * widthDecreaseSpeed;
            if (currentLaserWidth <= 0)
            {
                Destroy(gameObject);
            }
        }
        transform.rotation = Quaternion.RotateTowards(owner.bulletContainer.transform.rotation, Quaternion.LookRotation(randomOffsetDirection), randomOffSet);

        // Vector3 lookDir = (owner.gunTip.forward)* currentLaserLength;
        Vector3 lookDir = (transform.forward) * currentLaserLength;
        thisLineRenderer.SetPosition(0, owner.bulletContainer.position);
        thisLineRenderer.SetPosition(1, owner.bulletContainer.position + lookDir);
        thisLineRenderer.startWidth = currentLaserWidth;
        thisLineRenderer.endWidth = currentLaserWidth * 1.3f;
        //thisLineRenderer.SetWidth(currentLaserWidth, currentLaserWidth * 1.3f);

        transform.position = owner.bulletContainer.position + (lookDir / 2);
        ourCollider.GetComponent<BoxCollider>().size = new Vector3(currentLaserWidth, 1, currentLaserLength);
    }

    private void OnLaserDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnLaserDestroy;
    }

    public override float GetDamage()
    {
        return owner.laserStats.damage.value * owner.stats.damageMultiplier.value;
    }
}