public class GridTileSoldierController
{
    private Soldier _soldier;
    public GridTileSoldierController() => this._soldier = null;
    public override string ToString() => $"{this._soldier}";
    public void SetSoldier(Soldier soldier) => this._soldier = soldier;
    public void RemoveSoldier() => this._soldier = null;
    public Soldier GetSoldier() => this._soldier;
    public bool HasSoldier() => this._soldier != null;
    public bool HasEnemySoldier() => this.HasSoldier() && this._soldier.GetIsEnemy();
}
