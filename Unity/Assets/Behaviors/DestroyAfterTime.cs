using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float _time = 3;
    private void Start() => Destroy(gameObject, _time);
}
