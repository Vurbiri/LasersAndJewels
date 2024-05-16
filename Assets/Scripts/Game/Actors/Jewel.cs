using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Jewel : AJewel<Jewel>, IMouseClick
{ 
    [Space]
    [SerializeField] private TMP_Text _textCount;

    private Collider2D _collider;
    private readonly LoopArray<TurnData> _turnData = new(TurnData.Direction);
    private Transform _transformSprite;
    private bool _hint = false;
    private CheatController _cheatController;

    public override bool IsEnd => false;

    public event Action EventSelected;

    public override void Initialize()
    {
        _collider = GetComponent<Collider2D>();
        _cheatController = CheatController.InstanceF;
        _transformSprite = _spriteModule.Transform;
        _textCount.gameObject.SetActive(false);

        _collider.enabled = false;
        _cheatController.EventChangeCheat += OnChangeCheat;
        base.Initialize();
    }

    public void Setup(Vector2Int index, int idType, int count, int group)
    {
        BaseSetup(index, idType);

        _hint = count == 1;

        _textCount.text = count.ToString();
        _textCount.color = _colors[group].Brightness(_brightnessParticle);
        
        Turn(_turnData.Default);
    }

    public override bool ShowHint()
    {
        if (_hint) return false;

        _textCount.gameObject.SetActive(_hint = true);
        return true;
    }

    protected override void Run_Wait_FinalAction()
    {
        _collider.enabled = true;
        _textCount.gameObject.SetActive(_cheatController.IsCheat);
    }

    public override void Deactivate()
    {
        _textCount.gameObject.SetActive(false);
        base.Deactivate();
    }

    public override IEnumerator Deactivate_Coroutine()
    {
        _textCount.gameObject.SetActive(false);

        return base.Deactivate_Coroutine();
    }

    protected override void On(bool isLevelComplete)
    {
        if (isLevelComplete) _collider.enabled = false;

        BaseOn();
    }

    private void Turn(TurnData turnData)
    {
        _transformSprite.localRotation = turnData.Turn;
        _orientation = turnData.Orientation;
    }

    public void OnMouseClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Back : _turnData.Forward);

        if (_isOn) EventSelected?.Invoke();
    }

    private void OnChangeCheat(bool isCheat) => _textCount.gameObject.SetActive(_hint || isCheat);

    private void OnDestroy()
    {
        _cheatController.EventChangeCheat -= OnChangeCheat;
    }
}
