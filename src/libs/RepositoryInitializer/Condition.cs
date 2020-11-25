namespace RepositoryInitializer
{
    public class Condition
    {
        public string? Key { get; set; }
        public bool Value { get; set; }

        public Condition()
        {
        }

        public Condition(string key, bool value)
        {
            Key = key;
            Value = value;
        }
    }
}
