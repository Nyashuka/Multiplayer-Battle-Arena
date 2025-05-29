using System.Collections.Generic;
using System.Linq;

namespace Core.Modifiers
{
    public class ModifierStack<T>
    {
        private readonly List<IModifier<T>> _modifiers = new();

        public void AddModifier(IModifier<T> modifier)
        {
            _modifiers.Add(modifier);
        }

        public T ApplyModifiers(T value)
        {
            var result = value;

            foreach (var modifier in _modifiers.ToList())
            {
                if (modifier.IsExpired)
                {
                    _modifiers.Remove(modifier);
                    continue;
                }
                
                result = modifier.Modify(value);
                
                if (modifier.IsExpired)
                    _modifiers.Remove(modifier);
            }

            return result;
        }
    }
}