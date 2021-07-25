﻿using System;
using System.Collections;
using UnityEngine;

public class CharacterDash : MonoBehaviour
{
    private KeyCode dashKey;

    private CharacterMovement _characterMovement;
    private Rigidbody2D rb;

    private bool isDashing = false;
    private bool cooldownReady = true;
    private float dashTime;
    private Vector2 dashDestination;
    private Vector2 newPosition;

    [Header("Dash")]
    public float DashDuration;
    public float DashDistance;
    public float DashCoolTime;

    [Header("Cannot Move Layer")]
    public String CannotMoveLayer;


    void Awake()
    {
        dashKey = InputManager.Instance.Dash;
        _characterMovement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && cooldownReady)
        {
            DashStart();
        }

        if (isDashing)
        {
            if (dashTime < DashDuration)
            {
                newPosition = Vector2.Lerp(rb.position, dashDestination, dashTime/DashDuration);
                dashTime += Time.deltaTime;
                rb.position = newPosition;
            }
            else
            {
                isDashing = false;
            }
        }
    }

    private void DashStart()
    {
        StartCoroutine("CoolDown");
        isDashing = true;
        dashTime = 0f;
        dashDestination = GetDashDestination(_characterMovement.moveDirection, DashDistance);
    }

    IEnumerator CoolDown()
    {
        cooldownReady = false;
        yield return new WaitForSeconds(DashCoolTime);
        cooldownReady = true;
    }

    private Vector2 GetDashDestination (Vector2 dir, float distance)
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, distance, 1 << LayerMask.NameToLayer(CannotMoveLayer));

        if (ray.collider == null)
        {
            return rb.position + dir * distance;
        }
        else   //장애물에 막히면
        {
            return ray.point;
        }
    }
    
}