using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lesson2
{
    internal class Selector
    {
        public string Id { get; set; }
        public string TagName { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes=new List<string>();
        }
        public static Selector ConvertToSelecctor(string str)
        {

            var list = str.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Selector root = new Selector();
            Selector current = root;
            foreach (var item in list)
            {
                char[] separators = new char[] { '#', '.' };
                string[] qualities = item.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string level = item;
                foreach (var quality in qualities)
                {
                    
                    if (level.StartsWith('#'))
                    {
                        
                        current.Id = quality;
                        level = level.Substring(quality.Length + 1);
                    }

                    else if (level.StartsWith('.'))
                    {
                        current.Classes.Add(quality);
                        level = level.Substring(quality.Length + 1);
                    }
                    else
                    {
                        if (HtmlHelper.Instance.HtmlTags.Contains(quality)|| HtmlHelper.Instance.HtmlVoidTags.Contains(quality))
                        {
                            level = level.Substring(quality.Length);
                            current.TagName = quality;
                        }
                        else
                            Console.WriteLine("invalid parameter");
                    }
                }
            if (current.Parent == null)
                root = current;
            Selector childSelector = new Selector();
            current.Child = childSelector;
            childSelector.Parent = current;
            current = childSelector;
        }
        current.Parent.Child = null;
            return root;
        }
    public override string ToString()
    {
        string str = $"id: {Id} name:{TagName} classes:";
        ////foreach(var c in Classes)
        ////{
        ////    str +="class: "+ c;
        ////}
        //str += $"parent: {Parent} child: {Child}";
        return str;
    }
}
}
