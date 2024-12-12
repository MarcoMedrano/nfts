namespace NftSample.Dtos.Auth;

public class AuthenticateRequest
{
    public string SiweEncodedMessage { get; set; }
    public string Signature { get; set; }
}