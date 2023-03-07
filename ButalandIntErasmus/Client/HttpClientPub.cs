namespace ButalandIntErasmus.Client;

public class HttpClientPub
{
	public HttpClient client { get; set; }
	public HttpClientPub(HttpClient client)
	{
		this.client = client;
	}
}
