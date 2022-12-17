namespace Library
{
    public class EnumItem
    {
        public string Key;
        public string Value;
        public string GroupName;
        
        
        public EnumItem(string key, string value, string groupName)
        {
            Key = key;
            Value = value;
            GroupName = groupName;
        }
    }
}