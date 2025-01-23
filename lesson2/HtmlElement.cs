using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }=new List<string>();
        public List<string> Classes { get; set; }=new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }= new List<HtmlElement>();
        public HtmlElement()
        {

        }
        public HtmlElement(string id, string name, string innerhtml)
        {
            Parent = new HtmlElement();
            Children = new List<HtmlElement>();
            Id = id;
            Name = name;
            InnerHtml = innerhtml;
            Classes = new List<string>();
            Attributes=new List<string>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            if (Children != null)
            {
                foreach (HtmlElement child in this.Children)
                {
                    q.Enqueue(child);
                }
            }
            while (q.Count > 0)
            {
                HtmlElement temp = q.Dequeue();
                yield return temp;
                foreach (HtmlElement child in temp.Children)
                {
                    q.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement htmlElement = this;

            while (htmlElement != null)
            {
                yield return htmlElement;
                htmlElement = htmlElement.Parent;
            }
        }

        public HashSet<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            FindElementsRecursively(this, selector, result);
            return result;
        }

        private void FindElementsRecursively(HtmlElement currentElement, Selector selector, HashSet<HtmlElement> result)
        {
            var descendants = currentElement.Descendants();
            var filteredDescendants = descendants.Where(e => e.Equals(selector));
            filteredDescendants.ToList().ForEach(d => result.Add(d));
            foreach (var descendant in filteredDescendants)
            {
                FindElementsRecursively(descendant, selector.Child, result);
            }
        }
        public override bool Equals(Object? obj)
        {
            
            if (obj != null&&obj is Selector)
            {
                Selector selector = (Selector)obj;
                if (selector.TagName != "" && selector.TagName != this.Name)
                    return false;
                if (selector.Id != "" && selector.Id != this.Id)
                    return false;
                if (Classes != null)
                {
                    if (!selector.Classes.All(c => Classes.Contains(c)))
                            return false;
                }
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return Name+" "+Id+" "+Classes[0];
        }
    }
}
