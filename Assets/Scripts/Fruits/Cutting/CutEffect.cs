using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CutEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystemRenderer[] particleSystems;
    [SerializeField] private Material[] particlesMaterials;
    [SerializeField] private float lifeTime;

    private const int GreenParticleIndex = 0;
    private const int PurpleParticleIndex = 1;
    private const int RedParticleIndex = 2;
    private const int YellowParticleIndex = 3;
    
    private float _currentTime;

    private int GetParticleIndex(int spriteIndex)
    {
        if (spriteIndex == 1)
        {
            return GreenParticleIndex;
        }
        
        if (spriteIndex == 0 || spriteIndex == 2)
        {
            return PurpleParticleIndex;
        }
        
        if (spriteIndex == 3 || spriteIndex == 7)
        {
            return RedParticleIndex;
        }
        
        if (spriteIndex >= 4)
        {
            return YellowParticleIndex;
        }
        
        return -1;
    }
    
    public void SetParticles(int spriteIndex)
    {
        var particleIndex = GetParticleIndex(spriteIndex) * particleSystems.Length;
        
        for (var i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].material = particlesMaterials[particleIndex + i];
        }
    }

    public void Update()
    {
        _currentTime += Time.deltaTime;
        
        if (_currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
