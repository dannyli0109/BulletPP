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
        randomOffSet = Random.Range(-3, 3);
        currentLaserLength = owner.laserStats.maxLaserLength.value;
        currentLaserWidth = owner.laserStats.maxLaserWidth.value;

        thisLineRenderer.startWidth = 0;
        thisLineRenderer.endWidth = 0;

        EventManager.current.onAmmoDestroy += OnLaserDestroy;
    }
    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GamePausing()) return;

        HandleAmmoHit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.current.GamePausing()) return;
        //Debug.Log("laser");
        HandleAmmoHit(other);
    }

    void Update()
    {
        if (GameManager.current.GamePausing()) Destroy(gameObject);
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
        //transform.rotation = Quaternion.RotateTowards(owner.bulletContainer.transform.rotation, Quaternion.LookRotation(randomOffsetDirection), randomOffSet);

        Vector3 lookDir = (owner.bulletContainer.forward) * currentLaserLength;
        //Vector3 lookDir = (transform.forward) * currentLaserLength;
        lookDir = Quaternion.AngleAxis(-randomOffSet, Vector3.up) * lookDir;

        thisLineRenderer.SetPosition(0, owner.bulletContainer.position);
        thisLineRenderer.SetPosition(1, owner.bulletContainer.position + lookDir);

        thisLineRenderer.startWidth = currentLaserWidth;
        thisLineRenderer.endWidth = currentLaserWidth * 1.3f;

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
        //Debug.Log(owner.laserStats.damage.value * owner.stats.damageMultiplier.value * Time.deltaTime);
       return damage * Time.deltaTime;
   }

    //public override void Init(Character owner, float angle)
    //{
    //    this.owner = owner;
    //    transform.SetParent(null);
    //    transform.localScale = new Vector3(owner.bulletStats.size.value, owner.bulletStats.size.value, owner.bulletStats.size.value);
    //    transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localRotation.eulerAngles.y, 0f));
    //}
}