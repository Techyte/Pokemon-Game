INCLUDE ../../globals.ink

Hello!

{ starterChosen == "" : -> main | -> starterAlreadyChosen }

=== main ===
Which pokemon do you choose?
+[Charmander]
 #givebattler:Charmander.5
-> chosen("Charmander")
+[Bulbasaur]
 #givebattler:Bulbasaur.5
-> chosen("Bulbasaur")
+[Squirtle]
 #givebattler:Squirtle.5
-> chosen("Squirtle")

=== chosen(pokemonName) ===
~ starterChosen = pokemonName
You chose {pokemonName}!
-> END

=== starterAlreadyChosen ===
You have already chose {starterChosen}!
-> END