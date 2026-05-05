using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] private float _health;
    [SerializeField] private Transform head;
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float flashSpeed = 8f;

    private Coroutine damageRoutine;

    private Vector3 lastPosition;
    private float distanceCovered;
    private float stepDistance = 0.75f;

    public event Action OnFootstep;
    public event Action OnHealthPickup;

    void Awake()
    {
        lastPosition = head.position;
    }

    void Update()
    {
        UpdateSteps();
        if (!GameManager.Instance.IsPaused())
            Stats.Instance.SetHealthText(_health);
    }

    private void UpdateSteps()
    {
        Vector3 horizontalMovement = head.position - lastPosition;
        horizontalMovement.y = 0;

        float moveAmount = horizontalMovement.magnitude;

        if (moveAmount < 2f) // ignore teleport-sized jumps
        {
            distanceCovered += moveAmount;
        }

        if (distanceCovered >= stepDistance)
        {
            distanceCovered = 0;
            OnFootstep?.Invoke();
        }

        lastPosition = head.position;
    }

    public void UpdateHealth(float damage)
    {
        if (damage < 0)
        {            
            OnHealthPickup?.Invoke();
        }

        if ((damage < 0 && _health < 100f) || (damage > 0 && _health > 0))
        {            
            _health -= damage;
            if (_health > 100f)
                _health = 100f;
        }

        if (_health <= 0)
        {
            GameManager.Instance.GameOver(0);
        }
    }

    public Vector3 GetHeadPosition()
    {
        return head.position;
    }

    public void TakeDamage(Weapons weapon, Projectile projectile, Vector3 contactpoint)
    {       
        SetPlayerDamage(weapon);
        UpdateHealth(weapon.GetPlayerDamage());
        ShowDamageEffect();
        PlayHaptic(XRNode.LeftHand, 0.7f, 0.2f);
        PlayHaptic(XRNode.RightHand, 0.7f, 0.2f);
    }

    public void ShowDamageEffect()
    {
        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        damageRoutine = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        damageOverlay.color = new Color(1, 0, 0, 0.6f);

        while (damageOverlay.color.a > 0)
        {
            Color c = damageOverlay.color;
            c.a -= Time.deltaTime * flashSpeed;
            damageOverlay.color = c;
            yield return null;
        }
    }

    private void PlayHaptic(XRNode node, float amplitude, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (device.isValid &&
            device.TryGetHapticCapabilities(out HapticCapabilities capabilities) &&
            capabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, amplitude, duration);
        }
    }

    internal void ResetHealth()
    {
        _health = 100f;
    }


    public void SetPlayerDamage(Weapons weapon)
    {
        if (GameManager.Instance.currentMode == SpawnMode.Wave)
        {
            weapon.SetPlayerDamage(1f);            
        }
        else
        {
            weapon.SetPlayerDamage(5f);            
        }
    }


}
