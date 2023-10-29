# Server Side Programming

**Student:**        Bram Terlouw    <br/>
**Studentnumber:**  614992          <br/>
**Teacher:**        Raav Schravesande

## DISCLAIMER @Raav
- Alle 'Musts' zijn gedaan conform de opdracht.
- De tweede en derde 'Could' zijn gedaan (_authorization op function level & proces status in table storage_).

## Desciption of codebase:
Project gebouwd in DOTNET 6 ISOLATED, bevat vier azure functions waarvan twee queue triggers en twee Http triggers. Ook wordt gebruik gemaakt van BlobStorage voor het opslaan en ophalen van images.

## Flow:
- Gebruiker doet request naar Http trigger 'GetWeatherHttpTrigger'. Deze trigger, start een job die in de 'jobs queue' wordt gezet. Op deze manier blijft dit endpoint beschikbaar en kunnen meerdere verzoeken gedaan worden. Gebuiker krijgt een link naar de tweede http trigger.
- 'Jobs queue' verwerkt de messages met de job id (timestamp van de request), haalt weer data op m.b.v. een [API](https://data.buienradar.nl/2.0/feed/json), looped over het aantal weer measurements, en voegt elke measurement met het jobId aan de 'write queue'.
- 'Write queue' ontvangt de data en job id. Er wordt een random image van formaat 500x500 opgehaald via [Picsum](https://picsum.photos/500). Op deze image wordt weerdata geschreven met behulp van [ImageMagick](https://imagemagick.org/index.php). De image wordt toegevoegd aan de blob container met het corresponderende job id.
- Gebruiker kan nu met de link, verkregen na de request op 'GetWeatherHttpTrigger', om de weerdata op te halen. Wanneer het proces nog niet klaar is, wordt de gebruiker hierover geinformeerd, anders krijgt gebruiker een lijst met urls te zien. Elke url download een image voor een specifiek weerstation dat te zien is in de browser.

## Deployment:
Function applicatie is gedeployed op Azure. Hiervoor is gebruik gemaakt van 'login.ps1', 'deploy.ps1', 'function.bicep' om te deployen.
- login.ps1 -> Log in op azure naar de juiste subscription.
- deployment.ps1 -> Maak resource group om resources naar te deployen.
- function.bicep -> ARM template waarin resources gedefinieerd worden. Wordt gebruikt door deploy.ps1.
