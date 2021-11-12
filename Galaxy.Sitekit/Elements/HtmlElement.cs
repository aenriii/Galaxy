using System.Collections.Generic;

namespace Galaxy.Sitekit.Elements
{
    public class HtmlElement
    {
        public string? TagName { get; set; }
        public string? Content { get; set; }
        public string[]? Classes { get; set; }
        public string? Id { get; set; }
        public Dictionary<string, string>? Properties { get; set; }
        public HtmlElement(string tagname, string[]? classes = null, string? id = null, string? text = null, Dictionary<string, string>? props = null)
        {
            TagName = tagname;
            Classes = classes;
            Id = id;
            Content = text;
            Properties = props;
        }

        public string GetClassesString()
        {
            if (Classes == null)
                return "";
            if (Classes.Length == 0)
                return "";
            return " class=\"" + string.Join(" ", Classes) + "\"";
        }
        public string GetIdString()
        {
            if (Id == null)
                return "";
            return " id=\"" + Id + "\"";
        }
        public string GetPropertiesString()
        {
            if (Properties == null)
                return "";
            if (Properties.Count == 0)
                return "";
            string result = "";
            foreach (var prop in Properties)
            {
                result += " " + prop.Key + "=\"" + prop.Value + "\"";
            }
            return result;
        }
        

        public static implicit operator string(HtmlElement element)
        {
            return $"<{element.TagName} {element.GetClassesString()} {element.GetIdString()} {element.GetPropertiesString()}>\n{element.Content}\n</{element.TagName}>";
        }

        public string ToString()
        {
            return $"<{this.TagName} {this.GetClassesString()} {this.GetIdString()} {this.GetPropertiesString()}>\n{this.Content}\n</{this.TagName}>";
        }
    }
}