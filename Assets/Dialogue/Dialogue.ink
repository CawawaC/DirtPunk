VAR hintEnabled=0
VAR puzzleEnabled=0
VAR fadeToBlack=0

//start of main scene
=== PollutionInstructions ====
= PIStitch2
<color=lightblue>Mother Tree:</color> What once fed us and let us grow tall, has choked the soil. There is no room for our songs. Residuals still crowd <font="Dirtpunk SDF">this</font> space. <br><color=yellow>Click and drag the red pollutants off the screen</color>
    + [Start]
        -> END

//each called the more pollutants you remove
=== PollutionRemoval1 ====     
= PRStitch 
<color=green>Plant:</color> What are you DOING? I was born with <font="Dirtpunk SDF">my</font> chemicals. I’ll DIE without <font="Dirtpunk SDF">them</font>. 
    + [padding]
        -> END
        
 === PollutionRemoval2 ====  
= PRStitch 
<color=green>Plant:</color> Don’t cut me off! <font="Dirtpunk SDF">Oh</font>, you’re <i>killing</i> me, you STAMENPISTEL!<br><color=lightblue>Mother Tree:</color> Keep it up.
    + [padding]
        -> END
        
=== PollutionRemoval3 ====  
= PRStitch 
<color=green>Plant:</color> I'm DONE!<br>Wait, is <font="Dirtpunk SDF">that</font> a vibration in the soil? Who is whispering?
    + [padding]
        -> END
        
=== PollutionRemoval4 ====  
= PRStitch 
<color=green>Plant:</color> What does it want? What <font="Dirtpunk SDF">does</font> it mean?
    + [padding]
        -> END
        
=== PollutionRemoval5 ====
= PRStitch 
<color=green>Plant:</color> I wish I could reach out <font="Dirtpunk SDF">to it</font>. But how?
    + [padding]
        -> END
        
=== PollutionRemoval6 ====
= PRStitch 
<color=green>Plant:</color> I feel a tickling, <font="Dirtpunk SDF">like</font> I need to move!
    + [padding]
        -> END


// Called when plant nodules start growing
=== PlantNodulesGrow ====
= PNGStitch1
<color=green>Plant:</color> Look at me! Stretchyyyyyyy.
    + [Next]
        -> PNGStitch2
       
= PNGStitch2
<color=green>Plant:</color> What are these <font="Dirtpunk SDF">things</font> for?<br>SWEET SALTED SAP WHAT’S THIS???
    + [Next]
        -> PNGStitch3

= PNGStitch3
<color=lightblue>Mother Tree:</color> Little sproutling, you are <font="Dirtpunk SDF">ready</font> to sing again. Listen and teach the others the microbial melody.<br><color=yellow>Click on the flashing cell to hear a hint</color>
    + [Start]
        ~ hintEnabled=1
        -> END


// Instruction of audio puzzle-triggered by clicking on hint
=== AudioPuzzle1 ====
= APStitch
<color=lightblue>Mother Tree:</color> Be mindful of what you play. Much lurks in the soil, friends, foe, and <font="Dirtpunk SDF">neither</font>. The song can summon all.<br><color=yellow>Click on the plant nodes to recreate the hint tune</color>
    + [Start]
        ~ puzzleEnabled=1
        -> END

// Plant dialogue called at first node interaction    
=== AudioPuzzle2 ====
 = APStitch
<color=green>Plant:</color> HELL YEAH I HAVE A VOICE!
    + [padding]
        -> END
    
        
// Microbe dialogue after audio puzzle
// Microbes all spawn at once
=== MicrobesSpawn ====
= MSStitch1
<color=lightblue>Mother Tree:</color> Yes, this is the <font="Dirtpunk SDF">true</font> song. The microbes are home, I feel their quivering, shivering joy.
    + [Next]
        -> MSStitch2
       
= MSStitch2
<color=green>Plant:</color> WOW! What are these tiny dots doing? What is this thrilling, filling feeling? 
    + [Next]
        -> MSStitch3

= MSStitch3
<color=purple>Microbe1:</color> Hi hi <font="Dirtpunk SDF">hi</font>! You’re back, we’ll unpack!    
    + [Next]
        -> MSStitch4

= MSStitch4
<color=purple>Microbe2:</color> You forgot? We care, we share! We shake it down we break it down! We nosh and snack and sup! 
    + [Next]
        -> MSStitch5
        
= MSStitch5
<color=purple>Microbe2:</color> We plot for good rot! The good slop is in <font="Dirtpunk SDF">the</font> rot! We split, we breed, we live to feed! 
    + [Next]
        -> FeedFungi
        
        
//Come straight to fungi feeding sequence after microbe convo
=== FeedFungi ====
= FFStitch2
<color=orange>Fungi:</color> I’m <font="Dirtpunk SDF">small</font>, yes. But bring me food and I’ll thread myself through all things and entwine you and the earth. No cap!<br><color=yellow>Drag and drop small white particles onto the fuzzy white fungi.</color>
    + [Start]
        -> END
//Dialogue called at various points of fungi feeding
=== FungiFed1 ====
= FFStitch
<color=orange>Fungi:</color> Sweet preserved decay, can you <i>feeeeeel</i> that? My tastebuds are opening <font="Dirtpunk SDF">again</font>. Let me pull... and… reach.
    + [padding]
        -> END
=== FungiFed2 ====
= FFStitch
<color=lightblue>Mother Tree:</color> No one should be alone, least of all a little fungi. <font="Dirtpunk SDF">Fill</font> up their emptiness and be filled.
    + [padding]
        -> END
=== FungiFed3 ====
= FFStitch
<color=orange>Fungi:</color> I feel a rich spore song emerging, mycelium daydreams coursing, caressing, webbing. 
    + [padding]
        -> ConnectAllThings


// Called after fungi have all been fed
=== ConnectAllThings ====
= CATStitch1
<color=lightblue>Mother Tree:</color> Can you feel the power? <font="Dirtpunk SDF">These</font> are networked beings, they need connection to survive. Plug them in to <font="Dirtpunk SDF">charge</font> their awareness and sing as one among many. 
    + [Next]
        -> CATStitch2
= CATStitch2
<color=yellow>Hold P and draw a path with your mouse connecting the plant with one of the fungi</color>
    + [Start]
        -> END

// Called after plant has been connected to fungi
=== PlantConnected ====
= PCStitch1
<color=lightblue>Mother Tree:</color> Hold on to your roots, little one. Do you hear that humming murmur rustling through every fiber? That’s the world in waiting.
    + [Next]
        -> PCStitch2

= PCStitch2
<color=green>Plant:</color> Woah, what is this? <font="Dirtpunk SDF">Links</font> of the past, present, future?
    + [Next]
        -> PCStitch3

= PCStitch3
<color=purple>Microbe2:</color> Wohoo! Let’s renew! Let’s come through! For you, for me, for we!
    + [Next]
        -> PCStitch4

= PCStitch4
<color=orange>Fungi:</color> Darling sproutling, our tendrils can bring food from beyond your reach and our touch can ferry the song across our cosmos. 
    + [Next]
        -> PCStitch5
        
= PCStitch5
<color=green>Plant:</color> We can sing louder than anyone, we can <i>stamenpistel</i> SING! 
    + [Next]
        -> PCStitch6
        
= PCStitch6
<color=lightblue>Mother Tree:</color> Across the web, I have felt roots burn, wither, and drown. Now that you have found each other, the quiet void has cracked open. The song has returned. 
    + [Next]
        -> PCStitch7
        
= PCStitch7
<color=lightblue>Mother Tree:</color> The louder the song, the more we can pull smog from the sky. Bury it in our roots deep in the earth. And <i>sing</i>.
    + [Next]
        -> PCStitch8

= PCStitch8
<color=lightblue>Mother Tree:</color> Can you feel the silence around the edges of our song? All soil can sing again, if given space and care.<br>In your corner, do you sing?
    + [Finish]
        ~ fadeToBlack=1
        -> END
        
//////////////////////////////////////////
/////////////////////// TRUE END
//////////////////////////////////////////




