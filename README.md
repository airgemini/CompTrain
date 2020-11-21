# Applicazione Blazor WebAssembly con autenticazione JWT e IdentyServer

   Progetto d'esempio che per la realizzazione di un’applicazione completa per la gestione degli allenamenti che prevede 3 livelli di accesso:
   - Anonimo: il sito consentirà di visualizzare le pagine pubbliche
   - Atleta: il cliente che usufruisce dei servizi erogati
   - Admin: colui che gestisce il sito
   
   Per regolamentare le tipologie di accesso sopra descritte, è necessario, oltre alla registrazione dell’utente, anche un sistema di autorizzazioni.
   Utilizzeremo quindi:
   -	Blazor WebAssembly per il frontend
   -	ASP.NET Core per le API ed autenticazione/autorizzazione
   -	Identity Server per la gestione degli utenti
   -	Database MySQL/MariaDB
   -	Entity Framework come ORM
   -	JWT Token

*La versione di Blazor WASM usata in questo repository è la 3.2.0-preview4.*
Sono presenti i miei articoli inerenti l'autenticazione su [Blazor Developer Italiani](http://blazordev.it/).
