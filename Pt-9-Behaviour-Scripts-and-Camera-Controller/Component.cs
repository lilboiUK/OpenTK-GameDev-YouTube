namespace OpenTK_Game;

public class Component(GameObject gameObject)
{
    private bool _isEnabled = true;
    public readonly GameObject GameObject = gameObject;
    public readonly Transform Transform = gameObject.Transform;
    
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled == value) return;

            _isEnabled = value;

            if (_isEnabled)
                OnEnable();
            else
                OnDisable();
        }
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnDestroy()
    {
    }
}