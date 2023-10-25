using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] float collectorRange,rewardCollectorRange;

    [SerializeField] float collectorScanCooldown = 0.5f;
    [SerializeField] LayerMask collectibleLayer;

    Coroutine scanCoroutine,scanRewardCoroutine;

    public void SetupCollector(float collectRange)
    {
        collectorRange = collectRange;

        StartCollect();
    }

    public void SetCollectRange(float _collectRange)
    {
        collectorRange = _collectRange;
    }
    public void SetRewardCollectRange(float _collectRange)
    {
        rewardCollectorRange = _collectRange;
    }

    private void StartCollect()
    {
        if (scanCoroutine == null)
            scanCoroutine = StartCoroutine(ScanCollectible());
        else
        {
            StopCoroutine(scanCoroutine);
            scanCoroutine = StartCoroutine(ScanCollectible());
        }
    }

    IEnumerator ScanCollectible()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, collectorRange, collectibleLayer);

            foreach (Collider collider in colliders)
            {
                Collectible collectible = collider.gameObject.GetComponent<Collectible>();

                if(collectible)
                {
                    collectible.Collect(this);
                }
            }

            yield return new WaitForSeconds(collectorScanCooldown);
        }
    }

    public void CollectEnd(int amount)
    {
        MoneyManager.instance.IncreaseMoney(amount);
    }
}
