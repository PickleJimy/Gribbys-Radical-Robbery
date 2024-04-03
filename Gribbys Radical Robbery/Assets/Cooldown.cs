using UnityEngine;

[System.Serializable]
public class Cooldown : MonoBehaviour
{
    #region Variables

    [SerializeField] public float cooldownTime;
    public float _nextDashAttackTime;

    #endregion

    public bool IsCoolingDown => Time.time < _nextDashAttackTime;
    public void StartCooldown() => _nextDashAttackTime = Time.time + cooldownTime;
}
