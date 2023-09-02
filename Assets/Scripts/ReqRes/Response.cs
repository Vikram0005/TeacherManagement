[System.Serializable]
public class Response<T> 
{
    public string message;
    public string status;
    public T data;
}

[System.Serializable]
public class EmptyResponse
{

}
