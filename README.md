# Unity-JWT
Json Web Tokens for Unity.

This library supports generating and decoding [JSON Web Tokens](http://tools.ietf.org/html/draft-jones-json-web-token-10).

## Installation
The easiest way to install, is to download the project and extract to your individual Assets folder, this project relies on [Newtonsoft.Json](https://github.com/SaladLab/Json.Net.Unity3D).

## Usage
### Creating Tokens

```csharp
var payload = new Dictionary<string, object>()
{
    { "claim1", 0 },
    { "claim2", "claim2-value" }
};
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
string token = UnityEngine.JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
Debug.Log(token);
```

Output will be:
    eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s

### Verifying and Decoding Tokens

```csharp
var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s";
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
if(UnityEngine.JWT.JsonWebToken.Verify(token, secretKey)) {
    string jsonPayload = UnityEngine.JWT.JsonWebToken.Decode(token, secretKey);
    Debug.Log(jsonPayload);
} else {
    Debug.Log("Could not verify!");
}
```

Output will be:

    {"claim1":0,"claim2":"claim2-value"}

You can also deserialize the JSON payload directly to a .Net object with DecodeToObject:

```csharp
var payload = UnityEngine.JWT.JsonWebToken.DecodeToObject(token, secretKey) as IDictionary<string, object>;
Debug.Log(payload["claim2"]);
```

which will output:
    
    claim2-value

#### exp claim

As described in the [JWT RFC](https://tools.ietf.org/html/draft-ietf-oauth-json-web-token-32#section-4.1.4) the `exp` "claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing." If an `exp` claim is present and is prior to the current time the token will fail verification. The exp (expiry) value must be specified as the number of seconds since 1/1/1970 UTC.

```csharp
var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
var now = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
var payload = new Dictionary<string, object>()
{
    { "exp", now }
};
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
string token = UnityEngine.JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);

string jsonPayload = UnityEngine.JWT.JsonWebToken.Decode(token, secretKey); // JWT.SignatureVerificationException!
```

### Configure JSON Serialization

By default JSON Serialization is done by System.Web.Script.Serialization.JavaScriptSerializer.  To configure a different one first implement the IJsonSerializer interface.

```csharp
public class CustomJsonSerializer : IJsonSerializer
{
    public string Serialize(object obj)
    {
        // Implement using favorite JSON Serializer
    }

    public T Deserialize<T>(string json)
    {
        // Implement using favorite JSON Serializer
    }
}
```

Next configure this serializer as the JsonSerializer.
```cs
JsonWebToken.JsonSerializer = new CustomJsonSerializer();
```
