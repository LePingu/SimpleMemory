namespace SimpleMemory.Models
{
    public class ArbitraryObject
    {
        public int Id { get; private set;}

        public string Name { get; private set;}

        public ArbitraryObject(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string UselessMethod(int i, int j){
            System.Console.WriteLine("it should return " + i);
            return null;
        }

    }
}