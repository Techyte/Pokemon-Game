INCLUDE ../../globals.ink

{ givenPokeball == "" : -> main | -> alreadyGiven}

== main ==
Hello
I would like you to take this
It will help you
#giveItem:Poke Ball.20
There you go!
~ givenPokeball = "yes"
-> END

== alreadyGiven ==
I've already given you something!
-> END