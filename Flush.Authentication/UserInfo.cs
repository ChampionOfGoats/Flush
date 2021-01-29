namespace Flush.Authentication
{
    public class UserInfo
    {
        public string UniqueId { get; set; }
        public string DisplayName { get; set; }

        public override bool Equals(object obj)
        {
            var rhs = obj as UserInfo;
            return rhs is not null &&
                UniqueId == rhs.UniqueId &&
                DisplayName == rhs.DisplayName;
        }
    }
}
