using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    class TextGen
    {

        public string Compile(string text)
        {
            List<TextCount> list = new List<TextCount>();
            var array = text.Replace(".", " ").Replace("/", " ").Split(' ');
            for (int i = 0; i < array.Length; i++)
            {
                var now = format(array[i]);
                var before = format(array[i == 0 ? 0 : i - 1]);
                if (list.Exists(x => x.value == before)) {
                    if(!string.IsNullOrWhiteSpace(now))
                    {
                        list.Find(x => x.value == before).after.Add(now);

                    }
                } else 
                {
                    list.Add(new TextCount() { value = before, after = new List<string>() { now } });
                }
            }
            return JsonConvert.SerializeObject(list);
        }

        public string Generate(string text, int length)
        {
            List<TextCount> list = JsonConvert.DeserializeObject<List<TextCount>>(text);
            
            var textBuffer = "";
            TextCount last = new TextCount() { value = "error", after = new List<string>() { "error"} };
            try
            {

                for (int i = 0; i < length; i++)
                {
                    if (i == 0)
                    {
                        int random = new Random().Next(list.Count);
                        textBuffer = list[random].value;
                        last = list[random];

                    }
                    else
                    {
                        if (last.after == null)
                        {
                            break;
                        } else if (last.after.Count > 1)
                        {
                            last = list.Find(x => x.value == last.after[new Random().Next(last.after.Count - 1)]);
                        } else if(last.after.Count == 1)
                        {
                            last = list.Find(x => x.value == last.after[0]);
                        }
                        if (!textBuffer.EndsWith(last.value))
                        {
                            
                            textBuffer += " " + last.value;
                        }
                       
                    }
                }
            }
             catch (Exception e) {   }
            return textBuffer;
        }
        public string format(string text)
        {
            var format = text.Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Replace("!", "")
                .Replace(":", "")
                .Replace("\"", "")
                .Replace("?", "")
                .Replace("-","")
                .Replace("–", "")
                .Replace("’","")
                .Replace("„", "")
                .Replace("“", "")
                .Replace(".","");
            return format;
        }
    }
    [Serializable]
    public class TextCount
    {
        [JsonProperty("value")]
        public string value { get; set; }

        [JsonProperty("after")]
        public List<string> after { get; set; }
    }

}
