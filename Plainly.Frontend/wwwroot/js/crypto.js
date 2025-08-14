// crypto.js
window.cryptoFunctions = {
    /**
     * Verifies an ECDSA-signed JWT.
     * @param {string} token The full JWT string.
     * @param {string} base64PublicKey The base64-encoded public key bytes (SPKI format).
     * @returns {Promise<boolean>} A promise that resolves to true if the token is valid, false otherwise.
     */
    verifyEcdsaSignature: async (token, base64PublicKey) => {
        try {
            const parts = token.split('.');
            if (parts.length !== 3) {
                console.error("Invalid JWT format.");
                return false;
            }

            const [header, payload, signature] = parts;
            const dataToVerify = `${header}.${payload}`;

            const publicKeyBytes = base64ToArrayBuffer(base64PublicKey);
            const importedKey = await window.crypto.subtle.importKey(
                "spki",
                publicKeyBytes,
                {
                    name: "ECDSA",
                    namedCurve: "P-256",
                },
                true,
                ["verify"]
            );

            const signatureBytes = base64UrlToArrayBuffer(signature);
            const dataBytes = new TextEncoder().encode(dataToVerify);

            const isValid = await window.crypto.subtle.verify(
                {
                    name: "ECDSA",
                    hash: { name: "SHA-256" },
                },
                importedKey,
                signatureBytes,
                dataBytes
            );

            return isValid;
        } catch (error) {
            console.error("JWT verification failed:", error);
            return false;
        }
    },
};

// Helper function to convert a Base64 string to an ArrayBuffer.
function base64ToArrayBuffer(base64) {
    const binaryString = atob(base64);
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    return bytes.buffer;
}

// Helper function to convert a Base64Url string to an ArrayBuffer.
function base64UrlToArrayBuffer(base64Url) {
    let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    while (base64.length % 4 !== 0) {
        base64 += '=';
    }
    return base64ToArrayBuffer(base64);
}