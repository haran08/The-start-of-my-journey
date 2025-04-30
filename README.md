# The Final Battle

# This project isn't finished yet, as i'm with little time right now, I just finished the core parts of the game;
# As sad as this is for me how wanted to show how much I learned with this book for his creator, I can't do much
# about his right now... So the best solution that I find after 1 month of seeing what I could do was to write
# in this documentation what I plan to add later on the finished project, both to informe you how is reading this
# documentation and to remember me later when i have gone back here about what needed to be done.
# Missing Parts:
# - Add variety of difficulty levels (for now the difficulty is name only and don't change the gameplay)
#
# - Add Turn base effects and skills that uses theses effects (can be group turn or target turn)
#
# - Add more characters (both monsters and heroes), skills, effects, weapons and armors
#
# - Add more combats (and maybe a dungeon system to explore)
#
# - Add unique AI for differents classes (archer, mage, warrior or tank, damage deal, healer)
#
# - As the last, maybe make this project a real game, putting the database in a MySQL database and migrating
# from the CLI to a windows screen game, adding sprites to each character and things like that. (This last
# one is more like a really maybe one because I'll feel like it's would be better to create a new C++ game
# based on this one to do that.)
#
#############################################################################################################
#
# This is where the real documentation starts, as i should have started this from the beggining, this will be a little
# short in detail, but I'll try to tell at lest how each part of the code connect and work together.
#
# The MAIN:
# Here is where you start to run the program (WOW what amazing idea, everyone should start do the same), all the
# pre-configurations are inicialized here, for now there is only the type of game (AI VS PLAYER or AI VS AI) and
# the game difficulty (that don't do much for now).
# After configure, he calls the main loop of the game with the pre-configurations.
#
# -----------------------------------------------------------------------------------------------------------------
# 
# The Battle Mini Game:
# This class is responsable for the main GameLoop of this game. This game runs a turn for each group,
# one for a group of heroes and one for a group of monsters; Each group turn, all the characters in the group
# have they own time to play, and all actions taked are only runned on the end of the group turn.
# This class stores:
# - the heroes CombatGroup, a EnemiesWave of monsters, the RenderBattle view and others configurations
# things like who is playing the currect turn and is he a monster or a hero, and a event that stores
# all the actions that will be used in the end of a group turn.
#
# This class methods are:
# - Run (how runs the mini game logic)
# - RunTurnLogic (how runs the logic of each groups turn)
# - CheckWin (how check if the group win the game)
# - TryEndWave (how check if a monster wave has been defeated)
# - RemoveDeadCharactes (how remove any character that has 0 HP in the combat)
# - StolenItem (how take the items of 1 group and put they in the inventory of the other)
#
# -----------------------------------------------------------------------------------------------------------------
#
# The Effect:
# This class is the one that everyone will look and ask theyself, WHY? this could be done so much more simples
# and with so much less work. Well, this class was created this way because I wanted to test my own project
# for a magic system, and I must say, THIS PROTOTTPE WAS A SUCCESS!!!! I find that for a 2D game I need to make
# more adjustments that I first thought, and that I needed to reduce ever more of the main parts of the original
# idea... (as a reference, my ideia uses 5 separed parts to create a magic system, I was planning to use 2 in this game
# but as I was build the 2 part, I saw that this game don't needed one of this parts, so I removed one of them).
# Anyway, this was a incredible success, as this class is the core of my magic system, and as this works as expected,
# this means that my idea is possible of create, that the way!
# Ok, now going back, this class main propose is to store the method that does something, so in a simples way of speak,
# she is a transformer (not the type that you can drive), every instance of this class define a new functionalidade
# that can be used by others, as a example (deal damage, heal, skipTurn).
#
# This class stores:
# - IEffectParam (This is the parameter used by her functionality, if she deals damage, her tells how much and in who)
# - ActionTypeEnum (This enumeration tells what the type of effect this is, is a damage Effect, maybe a heal one or a
# strategic effect)
#
# This class methods:
# - Execute (this method runs the effect of this class using the IEffectParam data as parameter)
# - Copy (create a new copy of this effect with a new copy of the EffectParam)
#
# PS: As this class is a bit special, i needed to use inheritance to the IEffectParam property, the problem is
# that as the interface IEffect needed to be versatil; She needed to define this parameter as a interface IEffectParam,
# and as she defines a interface; I can't use a implementation of IEffectParam instead of the interface in the definition
# of the property, So when i create a new Effect i can't define the correct Param for him... I was unable of find a 
# solution for this problem and end up cheating; I simples check in the constructor and in the Execute method
# if the type of the param is right and then I let it's fly. But as this isn't the ideal, if you how is reading this
# know or thought, a you could do just this and pan, it would work without bandages, please send me a message
# of how you did it, this would be so much helpful that I can't ever start to describe.
#
# -----------------------------------------------------------------------------------------------------------------
#
# For now this is it, later I'll continue to add the rest of the documentation with time, as a simples resume for now:
# - The IAction interface is what implement the Effect class, she is the base of the skills and comsumibles items
#
# - Skill class simples has a effect stored and when tried to used, she configure the EffectParam of the effect and 
# call the Execute method, what makes she able of use Effects is the IAction interface.
#
# - The IItems is a interface to create items, all you need to do is add this interface to a method and he will be seeing as
# a item, my Consumables just implement the IAction interface directly togheter with this interface,
# while the Armor and Weapons haven they implementations plus this interface.
#
# -	The database is a simples method to read json files and inicialize they in a dictionary (table class).
# (be careful with the skill class in here, as she needed to be implemented in a unique form. For more reference
# go to the DataBase.cs file and search for the TryDeserialize method)
#
# - The Memory class is just a controller painel to mananger global variables and memory related things (like the database)
#
# - There is 2 versions of character class, one for AI and the other without it, the two are exactly the same, but the
# AI version has a property for the AILogic method, both they have a method to change they type for the other,
# if you want to make a charatectAI from a character, just create a new characterAI passing the character and a AILogic
# method.
#
# - Render is the place where all interface mathods and string are placed, if you need to add anything related with the
# interface, this is the place.

# - The last one is the Miscellaneous folder, here is stored the files that I don't know how to categorize (like magic),
# there is only 2 class's, ID (how i really shound have used from the start...) and BaseMethods (that are a export
# of my own library methods that I was using as a dependence).