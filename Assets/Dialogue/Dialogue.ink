=== MotherTreeIntro ====
// The mother tree gives a quest to our player

= MTIStitch1
Mother Tree: Child, can you hear me?
    + [Yes, I hear you!]
        -> MTIStitch2
       
= MTIStitch2
Mother Tree: You are both a listener and a singer, rarities in this cosm of a once swirling, chattering underground. Most are now voiceless and unable to hear each other.
    + [Next]
        -> MTIStitch3
        
= MTIStitch3
Mother Tree: I am the Mother, matriarch of this world, birthed and devoured. It is failing. The web has become wicked, our language lost, and the songs are severed.
    + [Next]
        -> MTIStitch4

= MTIStitch4
Mother Tree: Go, restore the language to our senses, follow the song and let it grow.
    + [Continue]
        -> END

=== AwakenPlant ====
// The player now must awaken the plant by clicking on it.

= APStitch1
Mother Tree: This plant runs deep but not strong. It is drowsy and unaware of your presence, resting in a chemical slumber which it will soon be unable to rely upon, leaving the plant weak to the rising storm. You must awaken it.
    + [Continue]
        -> END

=== ClickingOnPollution ====
// The mother tree instructs the player to clear the polllution. The plant is addicted to the chemicals, is desperate and dramatic saying it will die

= COPStitch1
Mother Tree: What once fed us and let us grow tall, has make the soil an empty cosm. There is no space for others to sing with my child. The residuals still crowd this space.
    + [Next]
        -> COPStitch2

// As you remove chemicals, the plant is still a little drugged but recovering some sense of hearing for the song, its sense of communication is slowly coming back. Coming out of drug haze.

// This part is related to in game progrress/variables. I've left it like this as instructed, but it's related to in-game logic, it shouldn't be "click through"

= COPStitch2
Plant: NO! Don’t cut me off, friend. I need my chemicals. I was born with them and I’ll die without them.
    + [Next]
        -> COPStitch3
        
= COPStitch3
Plant: Don’t take them away! You’re killing me! Mother Tree: Keep it up.
    + [Next]
        -> COPStitch4

= COPStitch4
Plant: I'm DONE! Wait, a vibration in the soil? Who’s whispering?
    + [Next]
        -> COPStitch5
        
= COPStitch5
What does it want?
    + [Next]
        -> COPStitch6

= COPStitch6
I wish I could cry out and meet it… but how?
    + [Next]
        -> COPStitch7
        
= COPStitch7
Is it just me or do I want to MOVE!?
    + [Continue]
        -> END

=== PlantNodulesGrow ====
// The plant nodules start growing

= PNGStitch1
Plant: Wow! Look at me! Stretchyyyyyyy.
    + [Next]
        -> PNGStitch2
       
= PNGStitch2
What are these things for?
OH MY GOD WHAT’S THIS???
    + [Continue]
        -> END

=== AudioPuzzle ====
// Start of audio puzzle

= APStitch1
Mother Tree: Little sproutling, you are ready to sing again. Listen and teach them the microbial melody. 
    + [Next]
        -> APStitch2
        
// This part is *not* click through. As you click on the nodules, these messages appear

= APStitch2
Plant: HELL YEAH I HAVE A VOICE.
    + [Next]
        -> APStitch3
        
= APStitch3
What are these murmurs and mumbles. I like it.
    + [Next]
        -> APStitch4

= APStitch4
Plant: I can hum, in this usually quiet void... and something else, an echo of a melodic memory. 
    + [Next]
        -> APStitch5

= APStitch5
Is it just me or do I want to MOVE!?
    + [Next]
        -> APStitch6

// if you play the wrong melody, one of the following three warning message appears (APStitch6, APStitch7, APStitch8)

= APStitch6
Mother tree: Be mindful of what you play, for who knows what it might conjure. Much lurks in the soil, friends, foe, and neither. This song can summon all…
    + [Next]
        -> APStitch7
        
= APStitch7
Mother tree: Listen carefully to my song
    + [Next]
        -> APStitch8
        
= APStitch8
Mother tree: Listen carefully, it goes like this 1aaa aaaaaa aa aaaa aa
    + [Next]
        -> APStitch9
 
 // else, when you play it correctly
 
 = APStitch9
Mother Tree: That’s it, you’ve sung in harmony. You've led these microbes home, I feel their excitement again
    + [Continue]
        -> END


=== MicrobesSpawn ====
// Audio puzzle complete, now the player is interacting with the microbes

= MSStitch1
Mother tree: That’s it, you’ve sung in harmony. You've led these microbes home, I feel their excitement again.
    + [Next]
        -> MSStitch2
        
// Everytime more microbes spawn we get a new message. These messages happen as we click.

       
= MSStitch2
Plant: Little ones, you bring a hidden taste to me. I feel... rejuvenated. How could I forget this feeling?
    + [Next]
        -> MSStitch3
        
= MSStitch3
Plant: Feeling?
    + [Next]
        -> MSStitch4

= MSStitch4
Microbe: Come, come. We stay for trade.
    + [Next]
        -> MSStitch5

= MSStitch5
Microbe: Forgotten host, have a sip and show me your goods.
    + [Next]
        -> MSStitch6
        
= MSStitch6
Microbe: Refreshing exchange, your feed is fantastic. What can I do for you?
    + [Next]
        -> MSStitch7

= MSStitch7
Microbe: ONLY IF WE DON’T MAKE THE BEST CASE?
    + [Next]
        -> MSStitch8
        
= MSStitch8
Microbe: You forgot about us, and suckled from another. We are the makers of the world. We will always survive but your life is short without us. 
    + [Continue]
        -> END

=== FeedFungi ====

= FFStitch1
Mother Tree: Click on the fungi that have appeared
    + [Next]
        -> FFStitch2
        
// This is not clickthrough, this is based on the player dragging and dropping organic matter on the fungi

= FFStitch2
Fungi: Hello For now, I am small but bring me enough food and I could enwrap the earth.
    + [Next]
        -> FFStitch3

= FFStitch3
Fungi: Sweet preserved decay, can you feel that? My tastebuds have been caressed once again. Let me unfold... and... reach.
    + [Next]
        -> FFStitch4
        
// BEST CASE OPTIONAL: Player drags and drops microbes on slots, and FFStitch4, 5, and 6 appear one by one.

= FFStitch4
Mother tree: No one should be alone, bring company and cooperation to my child. Fill up their emptiness.
    + [Next]
        -> FFStitch5
        
= FFStitch5
What’s this? I feel a joy and song within me, and strength coursing through my cells. Bring me more, for they are bursting with energy.
    + [Next]
        -> FFStitch6

= FFStitch6
You forgot about us, and suckled from another. We are the makers of the world. We will always survive but your life is short without us. Feed us and we will bring you strength, good looks and resilience.
    + [Next]
        -> FFStitch7
        
= FFStitch7
No one is an individual but a thriving system of many. Few see this anymore.
    + [Next]
        -> FFStitch8
        
= FFStitch8
No one is an individual but a thriving system of many. Few see this anymore.
    + [Next]
        -> FFStitch9

// Root grow


= FFStitch9
Plant: My journey grows longer, but it’ll be a lonesome exploration by all accounts…

    + [Continue]
        -> END

// END OF BEST CASE
// Root grows/Audio puzzle complete

=== ConnectAllThings ====
// The player connects all things together


= MTIStitch1
Mother tree: I can sense their new power. But they need more friends to survive and defend themselves. They no longer need to be alone. Plug them into the network, so they can hear and sing as one among many. Let them access….a greater awareness.
    + [Next]
        -> MTIStitch2
     
// Root grows
= MTIStitch2
Root: I feel naked and weak, without any support. Who will guide me? 
    + [Next]
        -> MTIStitch3
      
// Click on the fungi: Fungi
= MTIStitch3
I am ready to entwine, and repair the severed ties.  
    + [Next]
        -> MTIStitch4

// The plant is connected to the WWW: Fungi
= MTIStitch4
Hold on to your roots, little one. Do you hear that humming murmur rustling through every fiber? That’s the world in waiting.
    + [Next]
        -> MTIStitch5

// Plant is connected to the WWW: Plant
= MTIStitch5
What are these beautiful words, these symphonies of species? Woah, what is this?  Links of the past, present, future. Messages and memories. Advise, warnings and friendship.
    + [Next]
        -> MTIStitch6

// Plant is connected to the WWWt: Fungi
= MTIStitch6
How I have missed this sweet embrace. Little sproutling, my tendrils can bring food from beyond your reach and ferry your voice across the cosmos. Let my entangled body shield you from harm.
    + [Next]
        -> MTIStitch7

// Plant is connected to the WWW: Mother tree]
= MTIStitch7
My child, how proud you’ve made me. - The louder the song, the more we can suck smog from the sky. Bury it in our roots and the depths of the earth. 
    + [Next]
        -> MTIStitch8
        
= MTIStitch8
How I have missed this sweet embrace. Little sproutling, my tendrils can bring food from beyond your reach and ferry your voice across the cosmos. Let my entangled body shield you from harm.
    + [Next]
        -> MTIStitch9
        
= MTIStitch9
As a community, we follow birth and death as one, living by each other. Across the web, I have felt roots burn, wither, and drown. The world is changing, to survive this we must be connected in multispecies harmony.
    + [Next]
        -> MTIStitch10
        
    = MTIStitch10
For the artificial streams that made us drowsy will become a sporadic splatter, and to thrive while still providing for the 1bipeds1, we must rekindle past knowledge and kinship.
    + [Next]
        -> MTIStitch11
        
    = MTIStitch11
How I have missed this sweet embrace. Little sproutling, my tendrils can bring food from beyond your reach and ferry your voice across the cosmos. Let my entangled body shield you from harm.
    + [Next]
        -> MTIStitch12
        
= MTIStitch12
The song needs to cover us all, and thanks to you the quiet void has started to crack open. Take the lead and spread our song, allow it to flourish far and wide. Build soil and song as one.
    + [Continue]
        -> END
        
//////////////////////////////////////////
/////////////////////// TRUE END
//////////////////////////////////////////

