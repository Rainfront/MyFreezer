
export type JWTTokenType = {
    token: string,
    issuedAt?: Date,
    issuedAtUNIX?: number,
    expiredAt?: Date,
    expiredAtUNIX?: number,
}