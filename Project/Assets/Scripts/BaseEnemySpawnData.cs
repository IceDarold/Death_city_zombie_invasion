using System;

[Serializable]
public class BaseEnemySpawnData
{
	public BaseEnemySpawnData()
	{
	}

	public BaseEnemySpawnData(EnemyType _type, EnemyBornAction _bAction, int _startAniID, int _dataID)
	{
		this.type = _type;
		this.bAction = _bAction;
		this.startAniID = _startAniID;
		this.dataID = _dataID;
	}

	public EnemyType type;

	public EnemyBornAction bAction;

	public int startAniID;

	public int dataID = -1;

	public float wi = 1f;
}
