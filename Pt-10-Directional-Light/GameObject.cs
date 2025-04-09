namespace OpenTK_Game;

public class GameObject
{
    private readonly List<Component> _components = [];

    public GameObject()
    {
        Transform = new Transform(this);
        _components.Add(Transform);
    }

    public Transform Transform { get; }

    public bool IsActive { get; private set; } = true;

    public void SetActive(bool value)
    {
        if (IsActive == value) return;
        IsActive = value;
        foreach (var component in _components)
            component.IsEnabled = value;
    }

    public T AddComponent<T>() where T : Component
    {
        if (typeof(T) == typeof(Transform))
            throw new InvalidOperationException("GameObject already has a component of type Transform");

        var component = (T)Activator.CreateInstance(typeof(T), this)!;
        _components.Add(component);
        if (IsActive) component.IsEnabled = true;
        return component;
    }

    public void RemoveComponent<T>() where T : Component
    {
        if (typeof(T) == typeof(Transform))
            throw new InvalidOperationException("Cannot remove a component of type Transform");

        var component = GetComponent<T>();
        if (component == null) return;
        component.OnDestroy();
        _components.Remove(component);
    }

    public void RemoveComponent(Component component)
    {
        if (component == Transform) throw new InvalidOperationException("Cannot remove a component of type Transform");

        if (!_components.Contains(component)) return;
        component.OnDestroy();
        _components.Remove(component);
    }

    public T? GetComponent<T>() where T : Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetComponents<T>() where T : Component
    {
        return _components.OfType<T>();
    }
    
    public void StartComponents()
    {
        if (!IsActive) return;
        foreach (var component in _components.Where(component => component.IsEnabled)) component.Start();
    }

    public void UpdateComponents()
    {
        if (!IsActive) return;
        foreach (var component in _components.Where(component => component.IsEnabled)) component.Update();
    }
}