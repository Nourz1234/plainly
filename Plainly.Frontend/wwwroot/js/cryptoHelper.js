// wwwroot/cryptoHelper.js

window.jwtHelper = {
    async verifyJwtRS256(token, publicKeyPem) {
        const [headerB64, payloadB64, signatureB64] = token.split(".");

        const enc = new TextEncoder();
        const data = enc.encode(`${headerB64}.${payloadB64}`);

        const binarySignature = Uint8Array.from(atob(signatureB64.replace(/-/g, '+').replace(/_/g, '/')), c => c.charCodeAt(0));

        // Convert PEM to CryptoKey
        const key = await window.jwtHelper.importRsaKey(publicKeyPem);

        const isValid = await crypto.subtle.verify(
            {
                name: "RSASSA-PKCS1-v1_5",
                hash: "SHA-256"
            },
            key,
            binarySignature,
            data
        );

        return isValid;
    },

    async importRsaKey(pem) {
        const pemHeader = "-----BEGIN PUBLIC KEY-----";
        const pemFooter = "-----END PUBLIC KEY-----";
        const pemContents = pem
            .replace(pemHeader, "")
            .replace(pemFooter, "")
            .replace(/\s/g, "");
        const binaryDerString = atob(pemContents);
        const binaryDer = new Uint8Array([...binaryDerString].map(ch => ch.charCodeAt(0)));

        return crypto.subtle.importKey(
            "spki",
            binaryDer,
            {
                name: "RSASSA-PKCS1-v1_5",
                hash: "SHA-256"
            },
            false,
            ["verify"]
        );
    }
};
