using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected LayerMask playerMask;
    [SerializeField] protected float explodeDamage = 4;
    [SerializeField] protected float contactDamage = 1;
    [SerializeField] protected int currencyOnDeath = 10;
    [SerializeField] protected bool faceForwardDuringOrbit;

    protected EnemyMovement movement;
    protected SmoothRotate smoothRotate;

    protected virtual void Awake()
    {
        smoothRotate = GetComponent<SmoothRotate>();
        movement = GetComponent<EnemyMovement>();

        float angle = Mathf.Atan2(-transform.position.y, -transform.position.x) * Mathf.Rad2Deg;
        smoothRotate.Rotate(Quaternion.Euler(0, 0, angle - 90));
    }

    protected virtual void Update()
    {
        float angle = Mathf.Atan2(-transform.position.y, -transform.position.x) * Mathf.Rad2Deg;

        smoothRotate.Rotate(Quaternion.Euler(0, 0, angle - (movement.GetIsOrbiting() && !faceForwardDuringOrbit ? 180 : 90)));
    }

    public virtual void StartOrbiting(float val)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!LayerInLayermask(playerMask, collision.gameObject.layer)) return;

        collision.gameObject.GetComponent<Health>()?.TakeDamage(explodeDamage);
        GetComponent<Health>().SetHealth(0);
    }

    private bool LayerInLayermask(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }

    protected float GetSquaredDistance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2);
    }

    public void AwardCurrency()
    {
        CurrencyManager.Instance.DefeatEnemy(currencyOnDeath);
    }

    public float GetContactDamage()
    {
        return contactDamage;
    }
}
