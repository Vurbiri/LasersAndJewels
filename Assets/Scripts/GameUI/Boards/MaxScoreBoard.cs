
public class MaxScoreBoard : ABoard
{
    private void Start()
    {
        SetValue(_dataGame.MaxScore);
        _dataGame.EventChangeMaxScore += SetValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeMaxScore -= SetValue;
    }
}
