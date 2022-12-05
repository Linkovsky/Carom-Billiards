using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace CaromBilliards.BallType
{
    [CreateAssetMenu (fileName = "Ball_", menuName = "New Ball", order = 51)]
    public class BallSO : ScriptableObject
    {
        [field: SerializeField] public Material m_material { get; private set; }
        [field: SerializeField] public bool isCueBall { get; private set; }

        public void ApplyMaterial(MeshRenderer meshRenderer)
        {
            meshRenderer.material = m_material;
        }
    }
}

