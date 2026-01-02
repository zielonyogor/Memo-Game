// Define the variable outside any function to ensure state persists
let isProcessing = false;

// This function must be directly at the top level, not inside another function
async function flipCard(element, cardId) {
    const cardObj = document.getElementById(`card-${cardId}`);
    const imgObj = document.getElementById(`img-${cardId}`);

    // 1. Guard Clauses: Block interactions if animating or already visible
    if (isProcessing) return;
    if (cardObj.classList.contains('is-flipped')) return;

    isProcessing = true;

    try {
        // 2. Server Request
        const response = await fetch(`/Game/Flip?id=${cardId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) throw new Error("Server error");

        const data = await response.json();

        // 3. Update DOM based on Server Response
        if (data.imageUrl) {
            imgObj.src = data.imageUrl;     // Inject the URL only now
            imgObj.style.display = "block"; // Make the image visible

            // Force a browser reflow to ensure the image is ready before flipping (optional but smoother)
            void cardObj.offsetWidth;

            cardObj.classList.add('is-flipped');
        }

        // 4. Handle Logic (Mismatch Reset)
        if (data.resetRequired) {
            // Wait 1 second for user to see the mismatch, then flip both back
            setTimeout(async () => {
                // Reset current card
                unflipCard(cardObj, imgObj);

                // Reset the other card (Server should provide ID, or we track it)
                // NOTE: The server logic for "otherCardId" is not implemented in the controller yet, 
                // but the BL tracks it. The UI might need to know which card to flip back if it doesn't track it.
                // However, for now, let's assume the UI might not know the other card ID if we don't send it.
                // But wait, the previous card is already flipped.
                // We need to find the other flipped card that is NOT matched.
                
                // Simple UI-side fix: find all flipped cards that are NOT matched and unflip them.
                const allFlipped = document.querySelectorAll('.is-flipped');
                allFlipped.forEach(card => {
                     // We need to check if it's matched. We can add a class 'is-matched' when a match occurs.
                     if (!card.classList.contains('is-matched')) {
                         const img = card.querySelector('img');
                         unflipCard(card, img);
                     }
                });

                // Call ResolveMismatch on server
                await fetch('/Game/ResolveMismatch', { method: 'POST' });

                isProcessing = false; // Unlock only after animation finishes
            }, 1000);
        } else {
            if (data.isMatch) {
                 cardObj.classList.add('is-matched');
                 // Also mark the other card as matched?
                 // We might need to find the other card.
                 // Or just mark all currently flipped cards as matched?
                 // Since we only flip 2 at a time, if it's a match, the other one is the only other flipped one.
                 const allFlipped = document.querySelectorAll('.is-flipped');
                 allFlipped.forEach(c => c.classList.add('is-matched'));
            }
            // Match or First Card: Release lock immediately
            isProcessing = false;
        }

    } catch (err) {
        console.error("Flip failed:", err);
        isProcessing = false;
    }
}

// Helper to clean up the card state securely
function unflipCard(cardNode, imgNode) {
    cardNode.classList.remove('is-flipped');

    // Security: Clear the source after flipping back so "Inspect Element" reveals nothing
    setTimeout(() => {
        if (imgNode) {
            imgNode.src = "";
            imgNode.style.display = "none";
        }
    }, 300); // Wait for half the flip animation (300ms)
}