![Flush][logo]

## What is this?

Flush is a tool for facilitating and enhancing Agile planning activities; built with speed, ease of use and reliability in mind.

## Prerequisites

- Docker 17.06+
- Docker Compose 3.0+

## Getting Started

All of the following steps assume you have checked out the application and `PWD` is the root of such.

### Pull Submodules

Please issue `git submodule init && git submodule update` to ensure the latest versions of the submodules are present at build time.

### Configure Application Security

Flush uses the SQLCipher provider to enable cryptographically secured backing stores in EFCore.

You will need to specify keys in the `FlushDb`, `IdentityDb` and `JwtAuthentication` sections of `appsettings.json`.

You may use any alphanumeric value you wish for these keys however a script, `gen_keys.sh`, is provided to automatically generate and insert 2048-bit keys from the local random source.

The keys you select are used in SQLCipher as the input to a SHA-3 512-bit key derivation function. This means that the actual encryption key used by the database will be different to your input key. The application, however, will still only require the input key to decrypt it.

### Run Flush

To build the container image and start Flush, run `docker-compose up` at the root of your checkout.

## Enabling HTTPS

By default Flush assumes it is run behind a mature reverse proxy and, as a result, HTTPS is not enabled. To enable HTTPS, please add the following to the `Kestrel.EndPoints` section of `appsettings.json`.

```
"HttpsInlineCertFile": {
  "Url": "https://<host>:<port>",
  "Certificate": {
    "Path": "/app/tls/cert.pfx",
    "Password": "<password for your cert.pfx>"
  }
}
```

In the above section, replace the variables with values that suit your environment. If your certificate does not have a password, set `Password` to a blank string. The certificate should be stored at the root of your checkout, in a folder named `certs`.

If you are running a development copy of this application, you can use the default dotnet development certificates by substituting the above with the below snippet. Again, please replace the variables with values that suit your environment.

```
"HttpsDefaultCert": {
  "Url": "https://<host>:<port>"
}
```

## Changelog

See [this document](Docs/CHANGELOG.md).

## Known Issues

See [this document](Docs/KNOWNISSUES.md).

[logo]: Docs/flush-logo.png
