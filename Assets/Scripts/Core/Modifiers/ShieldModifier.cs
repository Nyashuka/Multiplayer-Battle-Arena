namespace Core.Modifiers
{
    public class ShieldModifier : IModifier<int>
    {
        private readonly float _damageReductionMultiplier;
        
        public ShieldModifier(float damageReductionMultiplier)
        {
            _damageReductionMultiplier = damageReductionMultiplier;   
        }
        
        public int Modify(int damage)
        {
            if (IsExpired)
                return damage;
            
            damage -= (int)(damage * _damageReductionMultiplier);

            return damage;
        }

        public bool IsExpired { get; private set; }

        public void SetExpired()
        {
            IsExpired = true;
        }
    }
}