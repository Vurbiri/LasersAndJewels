
public class ScoreBoard : ABoard
{
    private void Start()
    {
        SetValue(_dataGame.Score);
        _dataGame.EventChangeScore += SetValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= SetValue;
    }

    
}
