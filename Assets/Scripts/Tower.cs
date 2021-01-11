using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour, IPooledObject
{
    public Material activeMaterial;
    public Material inactiveMaterial;
    public Transform shotPoint;

    private ObjectPooler _objectPooler;

    private float _timeToStartFiring = 6f;

    [SerializeField] private bool firingEnabled = false;
    private float _fireRate = 0.5f;
    private float _shotsAmount = 0;
    private float _fireTimer = 0;

    private Coroutine _startingCoroutine;

    private void Start()
    {
        _objectPooler = ObjectPooler.I;
        SetTowerMaterial(activeMaterial);
    }

    private void Update()
    {
        if (firingEnabled)
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireRate)
            {
                _fireTimer = 0;
                Fire();
            }
        }
    }

    public IEnumerator OnTowerSpawn()
    {
        yield return new WaitForSeconds(_timeToStartFiring);
        StartFiring();
    }

    private void StartFiring()
    {
        _fireTimer = 0;
        firingEnabled = true;
    }

    private void StopFiring()
    {
        firingEnabled = false;
        SetTowerMaterial(inactiveMaterial);
    }

    private void Fire()
    {
        float rotationAmount = Random.Range(15f, 45f);
        rotationAmount += transform.rotation.eulerAngles.y + rotationAmount;
        LeanTween.rotateY(gameObject, rotationAmount, 0.1f).setEase(LeanTweenType.easeInOutBounce).setOnComplete(() =>
        {
            _objectPooler.SpawnShotFromPool(shotPoint.position, shotPoint.rotation);
            _shotsAmount++;
            if (_shotsAmount >= 12)
            {
                StopFiring();
            }
        });
    }

    public void ActivateTower()
    {
        StopCoroutine(_startingCoroutine);
        SetTowerMaterial(activeMaterial);
        StartFiring();
    }

    private void SetTowerMaterial(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }

    public void DestroyTower()
    {
        _objectPooler.ReturnObjectToPool("tower", gameObject);
        _objectPooler.uIController.SubtractTowers(1);
    }

    public void OnObjectSpawn()
    {
        _objectPooler = ObjectPooler.I;
        gameObject.SetActive(true);
        _startingCoroutine = StartCoroutine("OnTowerSpawn");
        SetTowerMaterial(activeMaterial);
        _objectPooler.uIController.AddTowers(1);
    }
}