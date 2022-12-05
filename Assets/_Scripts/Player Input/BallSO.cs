using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSO : ScriptableObject
{
    [field: SerializeField] public Material m_material { get; private set; }
    
}
