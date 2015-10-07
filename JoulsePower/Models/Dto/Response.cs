namespace JoulsePower.Models.Dto
{
    public class Response<T>
    {
        public T Content { get; set; }

        public string Name { get; protected set; }

        public Response(string name, T content)
        {
            this.Name = name;
            this.Content = content;
        }
    }
}