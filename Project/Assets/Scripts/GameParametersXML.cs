using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class GameParametersXML
{
	public SpawnConfig Load(string path, int levelNum)
	{
		SpawnConfig spawnConfig = new SpawnConfig();
		spawnConfig.Waves = new List<Wave>();
		Stream stream = null;
		UnityEngine.Debug.Log("Load");
		XmlDocument xmlDocument = new XmlDocument();
		if (path != null)
		{
			UnityEngine.Debug.Log("path not null");
			path = Application.dataPath + path;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			stream = File.Open(path + "config.xml", FileMode.Open);
			xmlDocument.Load(stream);
		}
		Wave wave = null;
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("Config/EnemySpawns/Level");
		if (levelNum <= xmlNodeList.Count)
		{
			levelNum--;
		}
		else
		{
			int num = UnityEngine.Random.Range(xmlNodeList.Count - 10, xmlNodeList.Count);
			levelNum = num;
		}
		UnityEngine.Debug.Log("levelNum" + levelNum);
		XmlNodeList xmlNodeList2 = xmlNodeList[levelNum].SelectNodes("Wave");
		IEnumerator enumerator = xmlNodeList2.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				XmlNode xmlNode = (XmlNode)obj;
				wave = new Wave();
				wave.Rounds = new List<Round>();
				spawnConfig.Waves.Add(wave);
				wave.intermission = (float)int.Parse(xmlNode.Attributes["intermission"].Value);
				XmlNodeList xmlNodeList3 = xmlNode.SelectNodes("Round");
				IEnumerator enumerator2 = xmlNodeList3.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						XmlNode xmlNode2 = (XmlNode)obj2;
						Round round = new Round();
						round.EnemyInfos = new List<EnemyInfo>();
						wave.Rounds.Add(round);
						round.intermission = (float)int.Parse(xmlNode2.Attributes["intermission"].Value);
						XmlNodeList xmlNodeList4 = xmlNode2.SelectNodes("Enemy");
						IEnumerator enumerator3 = xmlNodeList4.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								object obj3 = enumerator3.Current;
								XmlNode xmlNode3 = (XmlNode)obj3;
								EnemyInfo enemyInfo = new EnemyInfo();
								round.EnemyInfos.Add(enemyInfo);
								string value = xmlNode3.Attributes["id"].Value;
								enemyInfo.Count = int.Parse(xmlNode3.Attributes["count"].Value);
								string value2 = xmlNode3.Attributes["from"].Value;
								if (value2 == "grave")
								{
									enemyInfo.From = SpawnFromType.Grave;
								}
								else if (value2 == "door")
								{
									enemyInfo.From = SpawnFromType.Door;
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator3 as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		if (stream != null)
		{
			stream.Close();
		}
		return spawnConfig;
	}
}
