
public class HintBoard : ABoard
{
    private void Start()
    {
        SetValue(_dataGame.Hint);
        _dataGame.EventChangeHint += SetValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeHint -= SetValue;
    }
}
