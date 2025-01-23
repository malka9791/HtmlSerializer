using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public Selector(string str)
        {
            string tag = new Regex("^[^#.]([^#.]+)").Match(str).Value;
            if(tag!="")
            { TagName = tag; }
            var index1 = str.IndexOf('#');
            if (index1 != -1)
            {
                var index2 = str.IndexOf('.');
                if (index2 != -1)
                {
                    Id = str.Substring(index1 + 1, index2 - index1 - 1);
                    str = str.Substring(index2 + 1);
                    Classes= str.Split('.').ToList();
                }
                else { Id = str.Substring(index1 + 1); }
            }
        }
        public Selector()
        {
            
        }
        public static Selector ConvertToSelecctor(string str)
        {
            var list = str.Split(" ");
            Selector root= new Selector();
            Selector current= root;
            foreach(var item in list)
            {
                Selector newElement = new Selector(item);
                current.Child = newElement;
                current = newElement;
            }
            return root;
        }
    }
}
