using lesson2;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;
//var html = await Load("https://translate.google.co.il/?hl=iw&sl=en&tl=iw&op=translate");
//var cleanHtml = new Regex("//s").Replace(html, "");
//var lines = new Regex("<(.*?)>").Split(html).Where(p => p.Length > 0);
List<string> lines = new List<string>();
lines.Add("div id=\"mali\" class=\"j\" ");
HtmlElement root = new HtmlElement();
HtmlElement current = root;
foreach (var line in lines)
{
    var index = line.IndexOf(" "); 
    var InnerName = "";
    if(index == -1&&line.Length>0)
    {
        InnerName = line;
    }
    if (index >= 0)
    {
         InnerName = line.Substring( 0,index);
        Console.WriteLine(InnerName);
    }
    if (InnerName == "/html")
        break;
    if (InnerName == "/")
    {
        current = current.Parent;
    }
    else if (HtmlHelper.Instance.HtmlTags.Contains(InnerName) || HtmlHelper.Instance.HtmlVoidTags.Contains(InnerName))
    {
        HtmlElement newElement = new HtmlElement();
        newElement.Name = InnerName;
        newElement.InnerHtml = line;
        newElement.Attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line).Cast<Match>()
                               .Select(m => m.Value).ToList();
        newElement.Classes = new Regex("class=\"(.*?)\"").Matches(line).Cast<Match>()
                               .Select(m => m.Value.Substring(7,m.Value.Length-8))
                               .ToList();
        var Id = newElement.Attributes.Where(c => c.Remove(2) == "id").FirstOrDefault();
        if (Id != null)
        {
            Console.WriteLine(Id);
            newElement.Id = Id.Substring(4, Id.Length - 5);
        }
        if (!(HtmlHelper.Instance.HtmlVoidTags.Contains(InnerName) || line[line.Length - 1] == '/'))
        {
                current = newElement;
        }

        current.Children.Add(newElement);
         Console.WriteLine(newElement);
    }
    else
    {
        current.InnerHtml += line;
    }
}  

Selector root2 = Selector.ConvertToSelecctor("");
//printSelector(root);
HashSet<HtmlElement> result = new HashSet<HtmlElement>();
result = root.FindElementsBySelector(root2);
//await Console.Out.WriteLineAsync("------res--------");
Console.WriteLine(result.Count());
result.ToList().ForEach(r => Console.WriteLine(r.ToString()));
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}