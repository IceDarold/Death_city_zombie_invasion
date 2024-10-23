using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace DataCenter
{
	public class DataManager
	{

		private static readonly Dictionary<string, TextAsset> _textAssets = new Dictionary<string, TextAsset>();

		public static void LoadAllResources()
		{
			var assets = Resources.LoadAll<TextAsset>("DataConfig/");
			foreach (var textAsset in assets)
			{
				_textAssets.TryAdd(textAsset.name, textAsset);
			}
		}
		
		public static List<T> ParseXmlData<T>(string name, string root, string tag)
		{
			FieldInfo[] fields = typeof(T).GetFields();
			List<T> list = new List<T>();
			XmlDocument xmlDocument = new XmlDocument();
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			TextAsset textAsset;
			if (_textAssets.TryGetValue(name, out var asset))
			{
				textAsset = asset;
			}
			else
			{
				Debug.Log($"PARSE   {name}");
				textAsset = Resources.Load<TextAsset>("DataConfig/" + name);
				_textAssets.Add(name, textAsset);
			}
			//TODO:Added return
			if (textAsset == null)
			{
				return list;
			}
			xmlDocument.LoadXml(textAsset.text);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode(root).ChildNodes;
			IEnumerator enumerator = childNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlElement xmlElement = (XmlElement)obj;
					T t = Activator.CreateInstance<T>();
					for (int i = 0; i < fields.Length; i++)
					{
						if (xmlElement.Name == tag && xmlElement.GetAttributeNode(fields[i].Name) != null)
						{
							if (fields[i].FieldType == typeof(bool))
							{
								if (xmlElement.GetAttribute(fields[i].Name) == "true" || xmlElement.GetAttribute(fields[i].Name) == "false")
								{
									fields[i].SetValue(t, bool.Parse(xmlElement.GetAttribute(fields[i].Name)));
								}
								else
								{
									UnityEngine.Debug.Log("Error: xml data format error");
								}
							}
							else if (fields[i].FieldType.IsArray)
							{
								fields[i].SetValue(t, DataManager.StringToArray(xmlElement.GetAttribute(fields[i].Name)));
							}
							else if (fields[i].FieldType.IsGenericType)
							{
								fields[i].SetValue(t, DataManager.StringToList(xmlElement.GetAttribute(fields[i].Name)));
							}
							else if (fields[i].FieldType.IsEnum)
							{
								fields[i].SetValue(t, int.Parse(xmlElement.GetAttribute(fields[i].Name)));
							}
							else
							{
								fields[i].SetValue(t, Convert.ChangeType(xmlElement.GetAttribute(fields[i].Name), fields[i].FieldType, CultureInfo.InvariantCulture));
							}
						}
					}
					list.Add(t);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return list;
		}

		public static void SaveToJson<T>(string name, List<T> list)
		{
			string text = "{\"" + name + "\":[";
			for (int i = 0; i < list.Count; i++)
			{
				string str = JsonUtility.ToJson(list[i]);
				if (i < list.Count - 1)
				{
					text = text + str + ",";
				}
				else
				{
					text += str;
				}
			}
			text += "]}";
			PlayerPrefs.SetString(name, text);
		}

		public static List<T> ParseJson<T>(string name)
		{
			List<T> list = new List<T>();
			FieldInfo[] fields = typeof(T).GetFields();
			if (PlayerPrefs.HasKey(name))
			{
				string @string = PlayerPrefs.GetString(name);
				Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
				List<object> list2 = dictionary[name] as List<object>;
				for (int i = 0; i < list2.Count; i++)
				{
					Dictionary<string, object> dictionary2 = list2[i] as Dictionary<string, object>;
					T t = Activator.CreateInstance<T>();
					for (int j = 0; j < fields.Length; j++)
					{
						if (dictionary2.ContainsKey(fields[j].Name))
						{
							if (fields[j].FieldType.IsEnum)
							{
								fields[j].SetValue(t, int.Parse(dictionary2[fields[j].Name].ToString()));
							}
							else
							{
								fields[j].SetValue(t, Convert.ChangeType(dictionary2[fields[j].Name].ToString(), fields[j].FieldType));
							}
						}
					}
					list.Add(t);
				}
			}
			return list;
		}

		public static Dictionary<string, string> LoadLanguage(string name)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			XmlDocument xmlDocument = new XmlDocument();
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			TextAsset textAsset = Resources.Load<TextAsset>(name);
			xmlDocument.LoadXml(textAsset.text);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("Languages").ChildNodes;
			IEnumerator enumerator = childNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlElement xmlElement = (XmlElement)obj;
					dictionary.Add(xmlElement.Name, xmlElement.InnerText);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return dictionary;
		}

		private static int[] StringToArray(string str)
		{
			string[] array = str.Split(new char[]
			{
				','
			});
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
			}
			return array2;
		}

		private static List<int> StringToList(string str)
		{
			string[] array = str.Split(new char[]
			{
				','
			});
			List<int> list = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(int.Parse(array[i]));
			}
			return list;
		}
	}
}
