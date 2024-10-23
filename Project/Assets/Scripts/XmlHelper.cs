using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;

public class XmlHelper
{
	private static Encoding GetEncoding(string input)
	{
		Match match = XmlHelper.RegEncode.Match(input);
		if (match.Success)
		{
			try
			{
				return Encoding.GetEncoding(match.Result("${enc}"));
			}
			catch (Exception)
			{
			}
		}
		return XmlHelper.DefEncoding;
	}

	public static T ParseFileTextAsset<T>(string fileName) where T : class, new()
	{
		TextAsset textAsset = Resources.Load("data/" + fileName) as TextAsset;
		string text = textAsset.text.ToString();
		Encoding encoding = XmlHelper.GetEncoding(text);
		if (!object.Equals(encoding, XmlHelper.DefEncoding))
		{
			byte[] bytes = XmlHelper.DefEncoding.GetBytes(text);
			byte[] bytes2 = Encoding.Convert(XmlHelper.DefEncoding, encoding, bytes);
			text = encoding.GetString(bytes2);
		}
		return XmlHelper.Parse<T>(text, encoding);
	}

	public static T ParseFile<T>(string fileName) where T : class, new()
	{
		FileInfo fileInfo = new FileInfo(fileName);
		if (!fileInfo.Extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase) || !fileInfo.Exists)
		{
			throw new ArgumentException("输入的文件名有误！");
		}
		string text = fileName + "temp";
		FileInfo fileInfo2 = new FileInfo(text);
		DirectoryInfo directory = fileInfo2.Directory;
		if (directory != null && !directory.Exists)
		{
			directory.Create();
		}
		File.Copy(fileName, text);
		string text2;
		using (Stream stream = File.Open(text, FileMode.Open, FileAccess.Read))
		{
			using (TextReader textReader = new StreamReader(stream, XmlHelper.DefEncoding))
			{
				text2 = textReader.ReadToEnd();
			}
		}
		File.Delete(text);
		Encoding encoding = XmlHelper.GetEncoding(text2);
		if (!object.Equals(encoding, XmlHelper.DefEncoding))
		{
			byte[] bytes = XmlHelper.DefEncoding.GetBytes(text2);
			byte[] bytes2 = Encoding.Convert(XmlHelper.DefEncoding, encoding, bytes);
			text2 = encoding.GetString(bytes2);
		}
		return XmlHelper.Parse<T>(text2, encoding);
	}

	public static void SaveFile(string fileName, object obj)
	{
		XmlHelper.SaveFile(fileName, obj, XmlHelper.DefEncoding);
	}

	public static bool SaveFile(string fileName, object obj, Encoding encoding)
	{
		FileInfo fileInfo = new FileInfo(fileName);
		if (!fileInfo.Extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
		{
			throw new ArgumentException("输入的文件名有误！");
		}
		if (obj == null)
		{
			return false;
		}
		Type type = obj.GetType();
		XmlSerializer serializer = XmlHelper.GetSerializer(type);
		using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
		{
			using (TextWriter textWriter = new StreamWriter(stream, encoding))
			{
				serializer.Serialize(textWriter, obj);
			}
		}
		return true;
	}

	private static XmlSerializer GetSerializer(Type type)
	{
		string fullName = type.FullName;
		XmlSerializer xmlSerializer;
		bool flag = XmlHelper.Parsers.TryGetValue(fullName, out xmlSerializer);
		if (!flag || xmlSerializer == null)
		{
			XmlAttributes attributes = new XmlAttributes
			{
				XmlRoot = new XmlRootAttribute(type.Name)
			};
			XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
			xmlAttributeOverrides.Add(type, attributes);
			try
			{
				xmlSerializer = new XmlSerializer(type, xmlAttributeOverrides);
			}
			catch (Exception arg)
			{
				throw new Exception("类型声明错误！" + arg);
			}
			XmlHelper.Parsers[fullName] = xmlSerializer;
		}
		return xmlSerializer;
	}

	public T Parse<T>(string body) where T : class, new()
	{
		Encoding encoding = XmlHelper.GetEncoding(body);
		return XmlHelper.Parse<T>(body, encoding);
	}

	public static T Parse<T>(string body, Encoding encoding) where T : class, new()
	{
		Type typeFromHandle = typeof(T);
		string rootElement = XmlHelper.GetRootElement(body);
		string fullName = typeFromHandle.FullName;
		XmlSerializer serializer = XmlHelper.GetSerializer(typeFromHandle);
		object obj;
		using (Stream stream = new MemoryStream(encoding.GetBytes(body)))
		{
			obj = serializer.Deserialize(stream);
		}
		if (obj == null)
		{
			return (T)((object)null);
		}
		T result;
		try
		{
			T t = (T)((object)obj);
			result = t;
		}
		catch (InvalidCastException)
		{
			T t2 = Activator.CreateInstance<T>();
			PropertyInfo[] properties = typeof(T).GetProperties();
			PropertyInfo[] properties2 = obj.GetType().GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo2 = array[i];
				PropertyInfo info1 = propertyInfo2;
				using (IEnumerator<object> enumerator = (from propertyInfo in properties2
				where info1.Name.Equals(propertyInfo.Name)
				select propertyInfo.GetValue(obj, null)).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						object value = enumerator.Current;
						propertyInfo2.SetValue(t2, value, null);
					}
				}
			}
			result = t2;
		}
		return result;
	}

	private static XmlSerializer BuildSerializer(Type type)
	{
		XmlAttributes attributes = new XmlAttributes
		{
			XmlRoot = new XmlRootAttribute(type.Name)
		};
		XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
		xmlAttributeOverrides.Add(type, attributes);
		XmlSerializer result;
		try
		{
			result = new XmlSerializer(type, xmlAttributeOverrides);
		}
		catch (Exception arg)
		{
			throw new Exception("类型声明错误！" + arg);
		}
		return result;
	}

	public object ParseUnknown(string body, Encoding encoding)
	{
		string rootElement = XmlHelper.GetRootElement(body);
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Type type = null;
		foreach (Assembly assembly in assemblies)
		{
			type = assembly.GetType(rootElement, false, true);
			if (type != null)
			{
				break;
			}
		}
		if (type == null)
		{
			return null;
		}
		XmlSerializer serializer = XmlHelper.GetSerializer(type);
		object result;
		using (Stream stream = new MemoryStream(encoding.GetBytes(body)))
		{
			result = serializer.Deserialize(stream);
		}
		return result;
	}

	public string Serialize(object obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		Type type = obj.GetType();
		XmlSerializer serializer = XmlHelper.GetSerializer(type);
		StringBuilder stringBuilder = new StringBuilder();
		using (TextWriter textWriter = new StringWriter(stringBuilder))
		{
			serializer.Serialize(textWriter, obj);
		}
		return stringBuilder.ToString();
	}

	private static string GetRootElement(string body)
	{
		Match match = XmlHelper.RegRoot.Match(body);
		if (match.Success)
		{
			return match.Groups[1].ToString();
		}
		throw new Exception("Invalid XML format!");
	}

	private const string EncodePattern = "<[^>]+?encoding=\"(?<enc>[^<>\\s]+)\"[^>]*?>";

	private static readonly Encoding DefEncoding = Encoding.GetEncoding("UTF-8");

	private static readonly Regex RegRoot = new Regex("<(\\w+?)[ >]");

	private static readonly Regex RegEncode = new Regex("<[^>]+?encoding=\"(?<enc>[^<>\\s]+)\"[^>]*?>", RegexOptions.IgnoreCase);

	private static readonly Dictionary<string, XmlSerializer> Parsers = new Dictionary<string, XmlSerializer>();
}
