namespace RepositoryInitializer
{
    public class Variable
    {
        public string? Key { get; set; }
        public string? Value { get; set; }

        public Variable()
        {
        }

        public Variable(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}