using System;
using System.Collections.Generic;

namespace DataCenter
{
	public class DialogueDataManager
	{
		static DialogueDataManager()
		{
			DialogueDataManager.dialogues = DataManager.ParseXmlData<DialogueData>("DialogueData", "DialogueDatas", "DialogueData");
		}

		public static List<DialogueData> GetDialogue(int type, int tag)
		{
			List<DialogueData> list = new List<DialogueData>();
			for (int i = 0; i < DialogueDataManager.dialogues.Count; i++)
			{
				if (DialogueDataManager.dialogues[i].Type == type && DialogueDataManager.dialogues[i].Tag == tag)
				{
					list.Add(DialogueDataManager.dialogues[i]);
				}
			}
			list.Sort((DialogueData a, DialogueData b) => a.Index.CompareTo(b.Index));
			return list;
		}

		private static List<DialogueData> dialogues = new List<DialogueData>();
	}
}
