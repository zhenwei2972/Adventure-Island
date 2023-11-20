using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    Animator _animator;
    int _moveSpeedHash;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _moveSpeedHash = Animator.StringToHash("moveSpeed");
    }

    public void SetRunSpeed(float runSpeed) 
    {
        _animator.SetFloat(_moveSpeedHash, runSpeed);
    }
}
