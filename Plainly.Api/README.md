
# 🔐 Production Configuration

Set secrets using environment variables with double underscores (__) for nested config keys.

### Example:

Use:

```sh
ConnectionStrings__DefaultConnection=your-connection-string
```

To set:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```


# 🔑 Generating Asymmetric JWT Keys

Private key:
```sh
openssl genpkey -algorithm RSA -out jwt-private.key -pkeyopt rsa_keygen_bits:2048
```

Public key:
```sh
openssl rsa -in jwt-private.key -pubout -out jwt-public.key
```

- Tip: Use git bash on windows ;)

---

Store keys securely in environment variables:
```
Jwt__PrivateKey=[private key]
Jwt__PublicKey=[public key]
```