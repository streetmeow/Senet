﻿using System.Collections;
using UnityEngine;

public class cctvZone : MonoBehaviour
{
    public AudioClip clip;
    private bool cctvON;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(CCTVCoroutine());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && cctvON)
        {
            UIManager.Instance.Panel_Enable(Chapter2Manager.Instance.cctcDetected);
        }
    }

    private IEnumerator CCTVCoroutine()
    {
        SoundManager.instance.SFXPlay("thunder", clip);
        cctvON = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(1f);

        cctvON = true;
        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(Chapter2Manager.Instance.LightingTime);


        StartCoroutine(CCTVCoroutine());
    }
}
