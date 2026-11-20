using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Respawn")]
    [SerializeField] private bool _respawnEnabled = true;
    [SerializeField] private float _respawnDelay = 5f;

    private GameObject[] _spawnedCoins;

    private void Start()
    {
        InitializeCoins();
        SpawnAllCoins();

        if (_respawnEnabled)
            SubscribeToCoins();
    }

    private void InitializeCoins()
    {
        _spawnedCoins = new GameObject[_spawnPoints.Length];
    }

    private void SpawnAllCoins()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
            SpawnCoinAt(i);
    }

    private void SpawnCoinAt(int index)
    {
        if (_coinPrefab == null || _spawnPoints[index] == null) 
            return;

        _spawnedCoins[index] = Instantiate(_coinPrefab, _spawnPoints[index].position, Quaternion.identity);
    }

    private void SubscribeToCoins()
    {
        foreach (GameObject coinObject in _spawnedCoins)
        {
            Coin coin = coinObject.GetComponent<Coin>();

            if (coin != null)
                coin.OnCollected += HandleCoinCollected;
        }
    }

    private void UnsubscribeFromCoins()
    {
        foreach (GameObject coinObject in _spawnedCoins)
        {
            if (coinObject == null) 
                continue;

            Coin coin = coinObject.GetComponent<Coin>();

            if (coin != null)
                coin.OnCollected -= HandleCoinCollected;
        }
    }

    private void HandleCoinCollected(Coin coin)
    {
        StartCoroutine(RespawnAfterDelay(coin.gameObject));
    }

    private IEnumerator RespawnAfterDelay(GameObject coinObject)
    {
        yield return new WaitForSeconds(_respawnDelay);

        coinObject.SetActive(true);
    }

    private void OnDestroy()
    {
        UnsubscribeFromCoins();
    }
}