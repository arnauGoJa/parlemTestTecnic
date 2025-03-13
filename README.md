# parlemTestTecnic
 He volgut utilitzar cosmo db com a base de dades ja que mai l'he fet servir i veu comentar que treballeu amb ell, així que tot i que m'ho he estat mirant, no tinc clar d'haverho apliat correctament aquesta part per tot el tema de la partitionKey.
 He fet servir el emulador de cosmo db per fer les proves.
 La part de validacio que he fet no seria la més correcte, si el projecte hagues de creixer i hagues d'anar afegint validacions, feria servir alguna llibreria rollo fluent validation  i posaria una capa d'abtaccio per sobre per poder-ho reutilitzar i no haver de crear tantes clases per a fer el mateix.
 La gestio d'errors també es millorable, l'he fet bastant sencilla pero lo seu seria tenir també codis amb cada missatge i tenir una capa d'abstraccio al controller per a poder retornar un status més adequat segons el codi de forma centralitzada, també en comptes de tenir un unic missatge d'error en el objecte result, tindria un llistat per tal de poder tornar-ne els que fessin falta d'una sola vegada. 
 A part que per el meu desconeixement amb cosmo db he hagut de posar alguns try catch en capes que no m'agrada posar-ne. Lo seu seria no tenir més que un try catch a la capa més externa i poder gestionar la resta d'error de forma controlada amb el objecte Result.
 Si s'anessin fent més serveis, estaria bé anar afegint carpetes dins de cada capa per tal de separar les diferents funcionalitats, per exemple per dominis de negoci o per com es consideres més adequat d'entendre.
 A nivell de vaiables secretes, no es correcte que ho hagi posat tal qual al appsettings, lo seu seria tenir-ho minim encriptat i el millor no tenir cap clau escrita y que s'afegissin al fer deploy des de azure key vault.
 Faltaria també tot el tema d'autenticació per comunicar-se amb el front.
 A nivell de funcionalitats que es podrien afegir de forma rapida per a la pantalla de productes de client, seria la resta del CRUD que queda (Create,Update i Delete).
 
