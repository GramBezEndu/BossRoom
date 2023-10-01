using System;
using Unity.Netcode;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.GameplayObjects
{
    public class ManaReceiver : NetworkBehaviour
    {
        public event Action<int> ManaReceived;

        [SerializeField]
        DamageReceiver m_DamageReceiver;

        public void ReceiveMana(int mana)
        {
            if (m_DamageReceiver.IsDamageable())
            {
                ManaReceived?.Invoke(mana);
            }
        }
    }
}
