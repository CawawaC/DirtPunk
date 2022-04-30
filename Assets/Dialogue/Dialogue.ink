VAR hintEnabled=0
VAR puzzleEnabled=0

//start of main scene
=== PollutionInstructions ====
= PIStitch1
Mother Tree: This plant runs deep but not strong. It is drowsy and unaware of your presence, resting in a chemical slumber which it will soon be unable to rely upon, leaving the plant weak to the rising storm. You must awaken it.
    + [Next]
        -> PIStitch2

= PIStitch2
Mother Tree: What once fed us and let us grow tall, has make the soil an empty cosm. There is no space for others to sing with my child. The residuals still crowd this space.<br><color=yellow>Click and drag the red pollutants off the screen</color>
    + [Start]
        -> END


//each called the more pollutants you remove
=== PollutionRemoval1 ====     
= PRStitch 
Plant: NO! Don’t cut me off, friend. I need my chemicals. I was born with them and I’ll die without them.
    + [padding]
        -> END
 === PollutionRemoval2 ====  
= PRStitch 
Plant: Don’t take them away! You’re killing me!<br>Mother Tree: Keep it up.
    + [padding]
        -> END
=== PollutionRemoval3 ====  
= PRStitch 
Plant: I'm DONE! Wait, a vibration in the soil? Who’s whispering?
    + [padding]
        -> END
=== PollutionRemoval4 ====  
= PRStitch 
Plant: What does it want?
    + [padding]
        -> END
=== PollutionRemoval5 ====
= PRStitch 
Plant: I wish I could cry out and meet it… but how?
    + [padding]
        -> END
=== PollutionRemoval6 ====
= PRStitch 
Plant: Is it just me or do I want to MOVE!?
    + [padding]
        -> END


// Called when plant nodules start growing
=== PlantNodulesGrow ====
= PNGStitch1
Plant: Wow! Look at me! Stretchyyyyyyy.
    + [Next]
        -> PNGStitch2
       
= PNGStitch2
Plant: What are these things for?<br>OH MY GOD WHAT’S THIS???
    + [Next]
        -> PNGStitch3

= PNGStitch3
Mother Tree: Little sproutling, you are ready to sing again. Listen and teach them the microbial melody.<br><color=yellow>Click on the flashing cell to hear a hint</color>
    + [Start]
        ~ hintEnabled=1
        -> END


// Instruction of audio puzzle-triggered by clicking on hint
=== AudioPuzzle1 ====
= APStitch
<color=yellow>Click on the plant nodes to recreate the hint tune</color>
    + [Start]
        ~ puzzleEnabled=1
        -> END

// Plant dialogue called at first node interaction    
=== AudioPuzzle2 ====
 = APStitch
Plant: HELL YEAH I HAVE A VOICE!
    + [padding]
        -> END
    
        
// Microbe dialogue after audio puzzle
// Microbes all spawn at once
=== MicrobesSpawn ====
= MSStitch1
Mother tree: That’s it, you’ve sung in harmony. You've led these microbes home, I feel their excitement again.
    + [Next]
        -> MSStitch2
       
= MSStitch2
Plant: Little ones, you bring a hidden taste to me. I feel... rejuvenated. How could I forget this feeling?
    + [Next]
        -> MSStitch3

= MSStitch3
Microbe1: Clever microbe dialogue that Ashley is incapable of writing 1.
    + [Next]
        -> MSStitch4

= MSStitch4
Microbe2: Clever microbe dialogue that Ashley is incapable of writing 2.
    + [Next]
        -> MSStitch5
        
= MSStitch5
Microbe3: Clever microbe dialogue that Ashley is incapable of writing 3.
    + [Next]
        -> FeedFungi
        
        
//Come straight to fungi feeding sequence after microbe convo
=== FeedFungi ====
= FFStitch2
Fungi: Hello For now, I am small but bring me enough food and I could enwrap the earth.<br><color=yellow>Drag and drop small white particles onto the fuzzy white fungi.</color>
    + [Start]
        -> END
//Dialogue called at various points of fungi feeding
=== FungiFed1 ====
= FFStitch
Fungi: Sweet preserved decay, can you feel that? My tastebuds have been caressed once again. Let me unfold... and... reach.
    + [padding]
        -> END
=== FungiFed2 ====
= FFStitch
Mother tree: No one should be alone, bring company and cooperation to my child. Fill up their emptiness.
    + [padding]
        -> END
=== FungiFed3 ====
= FFStitch
Fungi: What’s this? I feel a joy and song within me, and strength coursing through my cells. Bring me more, for they are bursting with energy.
    + [padding]
        -> ConnectAllThings


// Called after fungi have all been fed
=== ConnectAllThings ====
= CATStitch1
Mother tree: I can sense their new power. But they need more friends to survive and defend themselves. They no longer need to be alone. Plug them into the network, so they can hear and sing as one among many. Let them access….a greater awareness.
    + [Next]
        -> CATStitch2
= CATStitch2
<color=yellow>Hold P and draw a path with your mouse connecting the plant with one of the fungi</color>
    + [Start]
        -> END

// Called after plant has been connected to fungi
=== PlantConnected ====
= PCStitch1
Mother tree: Hold on to your roots, little one. Do you hear that humming murmur rustling through every fiber? That’s the world in waiting.
    + [Next]
        -> PCStitch2

= PCStitch2
Plant: What are these beautiful words, these symphonies of species? Woah, what is this?  Links of the past, present, future. Messages and memories. Advise, warnings and friendship.
    + [Next]
        -> PCStitch3

= PCStitch3
Fungi: How I have missed this sweet embrace. Little sproutling, my tendrils can bring food from beyond your reach and ferry your voice across the cosmos. Let my entangled body shield you from harm.
    + [Next]
        -> PCStitch4

= PCStitch4
Mother tree: My child, how proud you’ve made me. - The louder the song, the more we can suck smog from the sky. Bury it in our roots and the depths of the earth. 
    + [Next]
        -> PCStitch5
        
= PCStitch5
Plant: As a community, we follow birth and death as one, living by each other. Across the web, I have felt roots burn, wither, and drown. The world is changing, to survive this we must be connected in multispecies harmony.
    + [Next]
        -> PCStitch6
        
= PCStitch6
Plant: How I have missed this sweet embrace. Little sproutling, my tendrils can bring food from beyond your reach and ferry your voice across the cosmos. Let my entangled body shield you from harm.
    + [Next]
        -> PCStitch7
        
= PCStitch7
Mother tree: The song needs to cover us all, and thanks to you the quiet void has started to crack open. Take the lead and spread our song, allow it to flourish far and wide. Build soil and song as one.
    + [Finish]
        -> END
        
//////////////////////////////////////////
/////////////////////// TRUE END
//////////////////////////////////////////

