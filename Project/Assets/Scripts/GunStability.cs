using System;

[Serializable]
public class GunStability
{
	[CNName("水平稳定性--Min")]
	public float stabilityHMin;

	[CNName("水平稳定性--Max")]
	public float stabilityHMax = 0.3f;

	[CNName("垂直稳定性--Min")]
	public float stabilityVMin;

	[CNName("垂直稳定性--Max")]
	public float stabilityVMax = 0.5f;
}
