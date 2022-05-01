=== MotherTreeIntro ====
// The mother tree gives a quest to our player

= MTIStitch1
<color=lightblue>Mother Tree:</color> Child, can you hear me?
    + [Yes!]
        -> MTIStitch2
       
= MTIStitch2
<color=lightblue>Mother Tree:</color> You are both a listener and a singer, rarities in this cosm of a once swirling, chattering underground. Most are now voiceless and unable to hear each other.
    + [Next]
        -> MTIStitch3
        
= MTIStitch3
<color=lightblue>Mother Tree:</color> I am the Mother, matriarch of this world, birthed and devoured. It is failing. The web has become wicked, our language lost, and the songs are severed.
    + [Next]
        -> MTIStitch4

= MTIStitch4
<color=lightblue>Mother Tree:</color> Go, restore the language to our senses, follow the song and let it grow.
    + [Continue]
        -> END
