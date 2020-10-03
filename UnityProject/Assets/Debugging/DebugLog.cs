
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
	public static class DebugLog
	{
        public class Data
        {
            public string tag;
            public string text;
            public string stack;
            public System.DateTime date;
        }

        static readonly int Max = 1024;
        static Queue<Data> Datas = new Queue<Data>(Max);

        static public void Log(string tag, string text)
        {
            var data = new DebugLog.Data() { tag = tag, text = text, stack = System.Environment.StackTrace, date = System.DateTime.Now };
            if (Datas.Count > Max)
            {
                Datas.Dequeue();
            }
            Datas.Enqueue(data);
            Logger.Log(JsonUtility.ToJson(data));
        }
    }
}
