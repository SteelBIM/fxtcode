namespace Kingsun.ExamPaper.ImportTool
{
    public class ListItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ListItem(string _name, string _value)
        {
            Name = _name;
            Value = _value;
        }
    }
}
