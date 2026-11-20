using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [Header("Collect Animation")]
    [SerializeField] private string _isCollectedParameter = "IsCollected";
    [SerializeField] private float _collectAnimationDuration = 0.3f;

    [Header("References")]
    [SerializeField] private Animator _animator;

    private Collider2D _collider;
    private bool _isCollected;

    public event Action<Coin> OnCollected;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ResetState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryCollect(collision);
    }

    private void ResetState()
    {
        _isCollected = false;
        _collider.enabled = true;

        if (_animator != null)
            _animator.SetBool(_isCollectedParameter, false);
    }

    private void TryCollect(Collider2D collision)
    {
        if (_isCollected)
            return;

        if (collision.GetComponent<Character>() == false)
            return;

        Collect();
    }

    private void Collect()
    {
        _isCollected = true;
        _collider.enabled = false;

        if (_animator != null)
            _animator.SetBool(_isCollectedParameter, true);

        StartCoroutine(FinishCollect());
    }

    private IEnumerator FinishCollect()
    {
        yield return new WaitForSeconds(_collectAnimationDuration);
        OnCollected?.Invoke(this);
        gameObject.SetActive(false);
    }
}