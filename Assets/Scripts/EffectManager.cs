using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] public GameObject Smoke;
    [SerializeField] public GameObject SmokeDynamite;
    [SerializeField] public GameObject Ripple;
    [SerializeField] public GameObject Splash;

    private void Awake()
    {
        Instance = this;
    }
}
