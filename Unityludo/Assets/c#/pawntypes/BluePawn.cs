
public class BluePawn : PawnInputAnalizer
{
    private const PawnType _playerType = PawnType.blue;
    private void Awake()
    {
        pawnType = _playerType;
    }
    public override void OnClick()
    {
        print("clicked :" + _playerType);
        if (DiceController.instance.currentPawn != _playerType)
            return;
        base.OnClick();
    }

}
