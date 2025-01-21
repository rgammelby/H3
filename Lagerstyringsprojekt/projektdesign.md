# Projektet

Vi har valgt at acceptere Rasmus' idé om at bygge et lagerstyringssytem.

Systemet skal kunne understøtte udlån af hardware, f.eks. skærme, tastaturer mv.

# Framework WebAPI

Vi har valgt frameworket ASP.NET, da vi senere skal have om det alligevel, og et par af os har perifer erfaring med det på forhånd.

# Framework Frontend

Vi har endnu ikke bestemt et framework for projektets frontend. Vi har flere forskellige kompetencer og endnu flere muligheder. Vi vælger et frontend framework, når vores WebAPI er opstillet.

# Databasen

Vi har arbejdet sammen om at skitsere og reviewe et [databasedesign](https://github.com/rgammelby/H3/blob/sascha-lagerstyring/Lagerstyringsprojekt/db.md). Hvorvidt vi skriver i MSSQL eller MySQL er endnu ikke bestemt.

Databasen vil indeholde en del triggers, som skriver logs til den samlede logtabel. Derudover har vi planlagt en større mængde Stored Procedures, som vil stå for samtlige processer. 

Der vil også findes en proces for hash/salt af brugeres passwords, så de sikkert kan opbevares i databasen.

Der vil blive udarbejdet et ERD diagram til databasen.

# Server

Vi opsætter ikke en fysisk server, så vores projekt vil blive hostet på en eller flere af vores personlige maskiner. 

# Basic Site Functionality

Vi har udarbejdet en række User Stories m. Acceptance Criteria for at fastslå nødvendig funktionalitet for hjemmesiden.

Du kan læse vores User Stories [hér](https://github.com/rgammelby/H3/blob/sascha-lagerstyring/Lagerstyringsprojekt/user_stories.md).