using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePooler : MonoBehaviour
{
    [SerializeField] Example bulletPrefab;

    private ObjectPooler<Example> objectPool;

    private void Start()
    {
        objectPool = new ObjectPooler<Example>(BulletFactoryMethod, TurnOnBullet, TurnOffBullet, 5, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var obj = objectPool.GetObject();
        }
    }

    private Example BulletFactoryMethod()
    {
        return Instantiate(bulletPrefab);
    }

    private void TurnOnBullet(Example bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void TurnOffBullet(Example bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
