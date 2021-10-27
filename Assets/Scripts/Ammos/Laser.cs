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
    FMOD.Studio.EventInstance soundInstance;

    void Start()
    {
        randomOffsetDirection = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        randomOffSet = Random.Range(-3, 3);
        currentLaserLength = owner.laserStats.maxLaserLength.value;
        currentLaserWidth = owner.laserStats.maxLaserWidth.value;
        if (facingOtherWay)
        {
            currentLaserLength = 8;
        }

        thisLineRenderer.startWidth = 0;
        thisLineRenderer.endWidth = 0;

        EventManager.current.onAmmoDestroy += OnLaserDestroy;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GameTransitional()) return;
        soundInstance = SoundManager.PlaySound(SoundType.LaserHit, other.gameObject.transform.position, 1);
        HandleAmmoHit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.current.GameTransitional()) return;
        //Debug.Log("laser");
        if (!SoundManager.current.IsPlaying(soundInstance))
        {
            soundInstance.start();
        }
        HandleAmmoHit(other);
    }

    void Update()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        if (GameManager.current.GameTransitional()) Destroy(gameObject);
        bornTime += Time.deltaTime;
        if (bornTime >= lifeTime)
        {
            currentLaserLength -= Time.deltaTime * lengthDecreaseSpeed;
            currentLaserWidth -= Time.deltaTime * widthDecreaseSpeed;
            if (currentLaserLength <= 0)
            {
                Destroy(gameObject);
            }
        }
        //transform.rotation = Quaternion.RotateTowards(owner.bulletContainer.transform.rotation, Quaternion.LookRotation(randomOffsetDirection), randomOffSet);

        float laserLength = currentLaserLength;
        RaycastHit hitInfo;
        if (Physics.Raycast(owner.bulletContainer.position, owner.bulletContainer.forward, out hitInfo, currentLaserLength, 1 << 10))
        {
            laserLength = hitInfo.distance;
        }

        Vector3 lookDir = (owner.bulletContainer.forward) * laserLength;
        //lookDir = Quaternion.AngleAxis(-randomOffSet, Vector3.up) * lookDir;

        this.gameObject.transform.forward = owner.bulletContainer.forward;
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);

        thisLineRenderer.SetPosition(0, owner.bulletContainer.position);
        thisLineRenderer.SetPosition(1, owner.bulletContainer.position + lookDir);

        thisLineRenderer.startWidth = currentLaserWidth;
        thisLineRenderer.endWidth = currentLaserWidth;

        transform.position = owner.bulletContainer.position + (lookDir / 2);
        ourCollider.GetComponent<BoxCollider>().size = new Vector3(currentLaserWidth, 1, laserLength);
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
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EventManager.current.onAmmoDestroy -= OnLaserDestroy;
    }

   public override float GetDamage()
   {
        //Debug.Log(owner.laserStats.damage.value * owner.stats.damageMultiplier.value * Time.deltaTime);
       return damage;
   }

    public override float GetImpactForce()
    {
        //Debug.Log("laser impact");
        return ImpactForce * Time.deltaTime;
    }

    public override void PlayImpactSound(Vector3 position)
    {
        //SoundManager.PlaySound(SoundType.LaserHit, position, 1);

    }

    //public override void Init(Character owner, float angle)
    //{
    //    this.owner = owner;
    //    transform.SetParent(null);
    //    transform.localScale = new Vector3(owner.bulletStats.size.value, owner.bulletStats.size.value, owner.bulletStats.size.value);
    //    transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localRotation.eulerAngles.y, 0f));
    //}
}