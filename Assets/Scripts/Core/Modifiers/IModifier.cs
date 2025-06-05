namespace Core.Modifiers
{
    public interface IModifier<T>
    {
        T Modify(T value);
        bool IsExpired { get; }

        void SetExpired();
    }
}