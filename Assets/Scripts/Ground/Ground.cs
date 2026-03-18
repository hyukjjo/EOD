using UnityEngine;

public enum GroundState
{
    Default = 0,
    Interaction,
    InteractionFail
}

public class Ground : MonoBehaviour
{
    private GroundState _state = GroundState.Default;

    [SerializeField] private Material _defaultMat;
    [SerializeField] private Material _interactionMat;
    [SerializeField] private Material _interactionFailMat;
    private MeshRenderer _Renderer;

    private void Start()
    {
        _Renderer = GetComponent<MeshRenderer>();
    }

    public void SetInteraction(GroundState state)
    {
        _state = state;

        switch (_state)
        {
            case GroundState.Default:
                _Renderer.material = _defaultMat;                
                break;
            case GroundState.Interaction:
                _Renderer.material = _interactionMat;
                break;
            case GroundState.InteractionFail:
                _Renderer.material = _interactionFailMat;
                break;
            default:
                break;
        }
    }

    public GroundState GetState() => _state;
}
