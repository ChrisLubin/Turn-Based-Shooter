public class GridObject
{
    private GridPosition _position;
    private Soldier _soldier;

    public GridObject(GridPosition gridPosition)
    {
        this._position = gridPosition;
    }

    public override string ToString()
    {
        return $"{this._position}\n{this._soldier}";
    }

    public void SetSoldier(Soldier soldier)
    {
        this._soldier = soldier;
    }

    public void RemoveSoldier()
    {
        this._soldier = null;
    }

    public Soldier GetSoldier()
    {
        return this._soldier;
    }

    public bool HasSoldier()
    {
        return this._soldier != null;
    }
}
