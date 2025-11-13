# Autentisering og Autorisasjon – Oversikt

## 1. Formål
Dette dokumentet beskriver hvordan autentisering og autorisasjon er implementert i Kollektivsystem-prosjektet.  
Målet er å gi en oversikt over:
- Arkitektur
- Viktige klasser og ansvar
- Flyt for innlogging, utlogging og fornying av tokens
- Begrensninger og videre forbedringer

---

## 2. Arkitektur (High-Level)

### 2.1 Komponenter
- **Web-klient**  
  Frontend som håndterer innlogging ved å kommunisere med APIet. Web-klienten lagrer access/refresh tokens sikkert i local storage og sørger for automatisk fornying før tokens utløper.
- **API (Backend)**  
  Inneholder `/auth` endepunkter som håndterer innlogging, tokenutveksling og refresh. Har også `/user/me` som returnerer informasjon om innlogget bruker basert på access token (sub/bruker-id).
- **OIDC Stub (Mock autorisasjonsserver)**  
  Simulerer `/authorize`, `/token` for intern utvikling. I realistisk produkt ville dette bli byttet ut med egen autorisasjonserver eller noe som google/facebook, etc.
- **Lagring**  
  Lagring av tokens (local storage og db), lagring av brukere og refreshtokens i db.

---

## 3. Autentiseringsflyt

### 3.1 Login (Authorization Code Flow)
1. Bruker klikker "Logg inn".
2. Web-klienten kaller `/auth/login` på APIet.
3. API genererer en redirect til OIDC-stub.
4. OIDC-stub returnerer en `code` tilbake til APIet.
5. API/klient kaller `/token` med `code` og får tilbake:
   - `access_token`
   - `refresh_token`
   - `expires_in`
6. API-et lagrer bruker og refreshtoken i db.
7. Web-klienten lagrer tokens lokalt og kaller `user/me` for å hente brukerinformasjon og oppdatere UI.

### 3.2 Refresh Token Flow
1. Access token er i ferd med å utløpe.
2. Web-klienten kaller `/auth/refresh` med refresh token sendes i request-body.
3. Nytt access token mottas og lagres.
4. UI oppdateres automatisk.

### 3.3 Logout
1. Web-klienten sletter tokens.
2. Videre kall til API gir 401 og krever ny innlogging.

---

## 4. Viktige klasser og ansvar

### 4.1 API
| Klasse | Ansvar |
|--------|--------|
| `AuthEndpooints` | Endepunkter for login, callback og refresh|
| `MockAuthProvider` | Genererer authorize-URL og utfører token exchange |
| `AuthService` | Validerer bruker, oppretter nye brukere og lagrer refresh tokens|

### 4.2 Web-klient
| Klasse | Ansvar |
|--------|--------|
| `AuthTokenService` | Lagring, lesing og fornying av tokens |
| `AuthTokens` | Modell for access/refresh tokens og utløpstid |
| `ProfileClient` | Henter brukerinfo fra API (`/user/me`) |

### 4.3 OIDC Stub
| Klasse | Ansvar |
|--------|--------|
| `OidcTokenService` | Generer nye tokens og validerer at riktig persona er logget inn.|
| `OidcOptions` | Konfigurasjon av OIDC-stub |
---

## 5. Tokenhåndtering

### 5.1 Lagring
- Tokens lagres i web-klient (local storage / session storage).
- APIet er stateless for access tokens (JWT valideres uten databaseoppslag).
- APIet lagrer kun hash av refresh tokens for å kunne validere dem ved fornying.

### 5.2 Utløp og fornying
- Access token fornyes X sekunder før utløp.
- Refresh token brukes til å hente nye tokens.
- Feil ved refresh → bruker logges ut.

### 5.3 Sikkerhet
- Tokens logges aldri.
- Client secret deles aldri til frontend.
- Refresh token behandles som sensitiv data.

---

## 6. Testing
Testene dekker både gyldige og ugyldige scenarier for å sikre robust autentisering og forutsigbar feiloppførsel.

### 6.1 Enhetstester (Unit Tests)
- `AuthTokenService` (expiry, refresh, parsing)
- Validering av påkrevde parametere
- URL-generering for authorize redirect

### 6.2 Integrasjonstester
- Full innloggingsflyt via OIDC-stub
- Refresh token-flyt
- Ugyldig kode → 400/401
- Beskyttede endepunkter krever gyldig token

---

## 7. Logging

### 7.1 Hva vi logger
- Start og slutt av login-flyt
- Mislykkede forsøk på token exchange
- Token utløp og refresh-operasjoner
- Informasjon som hjelper feilsøking (uten sensitive data)

### 7.2 Hva vi ikke logger
- Access tokens
- Refresh tokens
- Client secrets eller passord

---

## 8. Begrensninger
- OIDC-stub er kun for utviklingstest, ikke ekte sikkerhet.
- Tokens signeres statisk, ikke roterende nøkler.
- Ingen rate limiting eller brute-force-beskyttelse.

---

## 9. Forslag til videre forbedringer
- Bytte OIDC-stub med ekte identitetsleverandør.
- Bedre rolle- og rettighetssystem.
- Rotasjon av refresh tokens.
- Bedre monitoring og logging.

---

## 10. Endringslogg

