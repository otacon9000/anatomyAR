using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { 
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UIManager is NULL");
            }
            return _instance;
        } 
    }

    public Animator anim;
    private int _currentAnim;
    public int CurrentAnim 
    {
        get
        {
            return _currentAnim;
        }
        set
        {
            _currentAnim = value;
            if (_currentAnim == 1)
                label.SetActive(true);
            else
                label.SetActive(false);
        }
    }
    public GameObject label;

    private void Awake()
    {
        _instance = this;
        label.SetActive(false);
    }

    public void Next()
    {
        if (CurrentAnim >= 2) return;
        CurrentAnim++;
        anim.SetInteger("NextAnim", CurrentAnim);
    }

    public void Previous()
    {
        if (CurrentAnim <= 0) return;

        CurrentAnim--;
        anim.SetInteger("NextAnim", CurrentAnim);
    }


}
