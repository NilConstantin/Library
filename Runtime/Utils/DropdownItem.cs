namespace Library
{
    public class DropdownItem
    {
        public string Key;
        public string Value;
        public string GroupName;
        
        
        public DropdownItem(string key, string value, string groupName)
        {
            Key = key;
            Value = value;
            GroupName = groupName;
        }
    }
}