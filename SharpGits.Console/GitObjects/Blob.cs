namespace SharpGits.Console.GitObjects;

public class Blob : GitObject
{
    public byte[] Content { get; set; }
}