using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDescPopup : MonoBehaviour
{
    public static WeaponDescPopup Instance;
    
    private TMP_Text title, description;
    private Image bg;
    [SerializeField] private AnimationCurve curve;
    private bool isOpen;
    // private 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        title = transform.Find("Title").GetComponent<TMP_Text>();
        description = transform.Find("Description").GetComponent<TMP_Text>();
        bg = GetComponent<Image>();
        
        gameObject.SetActive(false);
    }
    
    public void SetWeapon(string title, string description)
    {
        this.title.text = title;
        this.description.text = description;
    }

    public void Toggle()
    {
        if (!isOpen) Show();
        else Hide();
    }
    
    public void Show()
    {
        isOpen = true;
        
        gameObject.SetActive(true);
        
        StopCoroutine(nameof(AnimationEnter));
        StartCoroutine(nameof(AnimationEnter));
    }
    
    public void Hide()
    {
        isOpen = false;
        
        gameObject.SetActive(false);
        
        StopCoroutine(nameof(AnimationEnter));
    }

    IEnumerator AnimationEnter()
    {
        float value = 0f;
        float t = 0;
        Vector3 start = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 end = new Vector3(1.5f, 1.5f, 1.5f);
        
        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            value = curve.Evaluate(t * 2);
            transform.localScale = Vector3.Lerp(start, end, value);
            // bg.color = new Vector4(0, 0, 0, t * 0.6f);
            // title.color = new Vector4(1, 1, 1, t * 0.6f);
            bg.color = new Vector4(0, 0, 0, value * 1.4f);
            title.color = new Vector4(1, 1, 1, value * 1.4f);
            yield return null;
        }
    }
}
