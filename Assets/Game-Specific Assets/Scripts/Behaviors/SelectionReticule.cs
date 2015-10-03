using UnityEngine;

public enum SelectionState
{
    Hidden,
    Friendly,
    Enemy
}

public class SelectionReticule : DebuggableBehavior
{
    #region Variables / Properties

    public string ColorVariable;
    public Color Hidden;
    public Color Friendly;
    public Color Enemy;

    private Renderer _renderer;
    private Material _material;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    #endregion Hooks

    #region Methods

    public void ChangeAppearance(SelectionState state)
    {
        _material = _renderer.materials[0];
        switch(state)
        {
            case SelectionState.Hidden:
                _material.SetColor(ColorVariable, Hidden);
                break;

            case SelectionState.Friendly:
                _material.SetColor(ColorVariable, Friendly);
                break;

            case SelectionState.Enemy:
                _material.SetColor(ColorVariable, Enemy);
                break;
        }

        _renderer.materials = new[] { _material };
    }

    #endregion Methods
}
