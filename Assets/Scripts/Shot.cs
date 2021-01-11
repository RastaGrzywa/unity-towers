using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shot : MonoBehaviour, IPooledObject
{
    private float _moveUnit = 10;

    private ObjectPooler _objectPooler;

    private void Start()
    {
        _objectPooler = ObjectPooler.I;
    }

    public void OnObjectSpawn()
    {
        float unitAmount = Random.Range(1, 4) * _moveUnit;

        Vector3 destinationPoint = transform.position + transform.rotation * Vector3.up * unitAmount;
        destinationPoint.y = 0;
        
        iTween.MoveTo(gameObject, destinationPoint, unitAmount / _moveUnit / 4f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_objectPooler.towersMaximumAmountReached)
        {
            return;
        }

        if (other.collider.tag == "Ground")
        {
            Vector3 newTowerPosition = transform.position + Quaternion.identity * Vector3.up * 0.5f;
            _objectPooler.SpawnTowerFromPool(newTowerPosition, Quaternion.identity);
            _objectPooler.ReturnObjectToPool("shot", gameObject);
            return;
        }

        if (other.collider.tag == "Tower")
        {
            other.collider.GetComponent<Tower>().DestroyTower();
            _objectPooler.ReturnObjectToPool("shot", gameObject);
            return;
        }
    }
}