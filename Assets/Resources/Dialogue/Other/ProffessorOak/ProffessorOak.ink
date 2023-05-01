INCLUDE ../../globals.ink

Hello!

{ starterChosen == "" : -> main | -> starterAlreadyChosen }

=== main ===
Which pokemon do you choose?
+[Charmander]
 #chosenPokemon:Charmander
-> chosen("Charmander")
+[Bulbasaur]
 #chosenPokemon:Bulbasaur
-> chosen("Bulbasaur")
+[Squirtle]
 #chosenPokemon:Squirtle
-> chosen("Squirtle")

=== chosen(pokemonName) ===
~ starterChosen = pokemonName
You chose {pokemonName}!
-> END

=== starterAlreadyChosen ===
You have already chose {starterChosen}!
-> END